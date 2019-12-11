using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class SessionAwakeSystem : AwakeSystem<Session, AChannel>
    {
        public override void Awake(Session self, AChannel b)
        {
            self.Awake(b);
        }
    }

    public sealed class Session : Entity
    {
        private static int RpcId { get; set; }
        public AChannel channel;

        private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();
        private readonly byte[] opcodeBytes = new byte[2];

        public NetworkComponent Network
        {
            get
            {
                return this.GetParent<NetworkComponent>();
            }
        }

        public int Error
        {
            get
            {
                return this.channel.Error;
            }
            set
            {
                this.channel.Error = value;
            }
        }

        public void Awake(AChannel aChannel)
        {
            this.channel = aChannel;
            this.requestCallback.Clear();
            long id = this.Id;
            channel.ErrorCallback += (c, e) =>
            {
                if (this.Network != null)  //[MyAppend]
                {//[MyAppend]
                    this.Network.Remove(id);
                }//[MyAppend]
            };
            channel.ReadCallback += this.OnRead;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.Network.Remove(this.Id);

            base.Dispose();

            foreach (Action<IResponse> action in this.requestCallback.Values.ToArray())
            {
                action.Invoke(new ResponseMessage { Error = this.Error });
            }

            //int error = this.channel.Error;
            //if (this.channel.Error != 0)
            //{
            //	Log.Trace($"session dispose: {this.Id} ErrorCode: {error}, please see ErrorCode.cs!");
            //}

            this.channel.Dispose();

            this.requestCallback.Clear();
        }

        public void Start()
        {
            this.channel.Start();
        }

        public IPEndPoint RemoteAddress
        {
            get
            {
                return this.channel.RemoteAddress;
            }
        }

        public ChannelType ChannelType
        {
            get
            {
                return this.channel.ChannelType;
            }
        }

        public MemoryStream Stream
        {
            get
            {
                return this.channel.Stream;
            }
        }

        public void OnRead(MemoryStream memoryStream)
        {
            try
            {
                this.Run(memoryStream);
            }
            catch (Exception e)
            {
                Log.Error(e + myDebug(memoryStream));
            }
        }

        //------------//[MyAppend begin]------------
        private String myDebug(MemoryStream memoryStream)
        {
            if (memoryStream == null)
            {
                return "[MyDebug - memoryStream==null]\n\n";
            }
            else
            {
                String hexString = toHexString(memoryStream.GetBuffer(), memoryStream.Length);
                return "[MyDebug - RemoteAddress.Address: " + this.channel.RemoteAddress.Address.ToString()
                        + ", memoryStream.Length: " + memoryStream.Length
                        + ", memoryStream.GetBuffer().hexString: " + hexString
                        + "]\n\n";
            }
        }
        private String myDebug(ushort opcode)
        {
                return "[MyDebug - RemoteAddress.Address: "
                        + this.channel.RemoteAddress.Address.ToString()
                        + ", Invalid opcode: " + opcode + "]\n\n";
        }
        private string toHexString(byte[] bytes, long length) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                //                int length = bytes.Length;
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    strB.Append(bytes[i].ToString("X2")).Append(" ");
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        
        // private 

        private bool checkOpcode(ushort opcode)
        {
            bool result = (opcode >= 10001 && opcode <= 12031) || (opcode >= 1001 && opcode <= 1045) || (opcode >= 101 && opcode <= 116);
            return result;
        }
        //------------//[MyAppend end]------------

        private void Run(MemoryStream memoryStream)
        {
            memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);

            //------------//[MyAppend begin]------------
            if (!checkOpcode(opcode))
            {
                myDebug(opcode);
                this.Error = ErrorCode.ERR_PacketParserError;
                this.Network.Remove(this.Id);
                return;
            }
            //------------//[MyAppend end]------------

#if !SERVER
            if (OpcodeHelper.IsClientHotfixMessage(opcode))
            {
                this.GetComponent<SessionCallbackComponent>().MessageCallback.Invoke(this, opcode, memoryStream);
                return;
            }
#endif

            object message;
            try
            {
                OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
                object instance = opcodeTypeComponent.GetInstance(opcode);
                message = this.Network.MessagePacker.DeserializeFrom(instance, memoryStream);
            }
            catch (Exception e)
            {
                // 出现任何消息解析异常都要断开Session，防止客户端伪造消息
                //                Log.Error($"opcode: {opcode} {this.Network.Count} {e} " + myDebug(memoryStream));
                //Log.Error(e + ", opcode: " + opcode + ", this.Network.Count: " + this.Network.Count + ", " + myDebug(memoryStream));
                this.Error = ErrorCode.ERR_PacketParserError;
                this.Network.Remove(this.Id);
                return;
            }

            IResponse response = message as IResponse;
            if (response == null)
            {
                //屏蔽心跳日志输出 10008
                //屏蔽广播玩家下注日志 10050
                //屏蔽广播推送的消息 request
                //1011数据存储
                if (OpcodeHelper.IsNeedDebugLogMessage(opcode)
                    && opcode != 10008
                    && opcode != 10050
                    && opcode != 1011
                    && opcode != 1012
                    && opcode != 1009
                    && opcode != 1010
                    && (message as IActorMessage) == null
                    )
                {
                    Log.Msg(message);
                }
                this.Network.MessageDispatcher.Dispatch(this, opcode, message);
                return;
            }

            Action<IResponse> action;
            if (!this.requestCallback.TryGetValue(response.RpcId, out action))
            {
                throw new Exception($"not found rpc, response message: {StringHelper.MessageToStr(response)}");
            }
            this.requestCallback.Remove(response.RpcId);

            action(response);

            //屏蔽心跳日志输出 10008
            //屏蔽广播玩家下注日志 10050
            if (OpcodeHelper.IsNeedDebugLogMessage(opcode) && opcode != 10008 && opcode != 10050 && opcode != 1011 && opcode != 1012 && opcode != 1010 && opcode != 1009)
            {
                Log.Msg(message);
            }
        }

        public ETTask<IResponse> Call(IRequest request)
        {
            int rpcId = ++RpcId;
            var tcs = new ETTaskCompletionSource<IResponse>();

            this.requestCallback[rpcId] = (response) =>
            {
                try
                {
                    if (ErrorCode.IsRpcNeedThrowException(response.Error))
                    {
                        throw new RpcException(response.Error, response.Message);
                    }

                    tcs.SetResult(response);
                }
                catch (Exception e)
                {
                    tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
                }
            };

            request.RpcId = rpcId;
            this.Send(request);
            return tcs.Task;
        }

        public ETTask<IResponse> Call(IRequest request, CancellationToken cancellationToken)
        {
            int rpcId = ++RpcId;
            var tcs = new ETTaskCompletionSource<IResponse>();

            this.requestCallback[rpcId] = (response) =>
            {
                try
                {
                    if (ErrorCode.IsRpcNeedThrowException(response.Error))
                    {
                        throw new RpcException(response.Error, response.Message);
                    }

                    tcs.SetResult(response);
                }
                catch (Exception e)
                {
                    tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
                }
            };

            cancellationToken.Register(() => this.requestCallback.Remove(rpcId));

            request.RpcId = rpcId;
            this.Send(request);
            return tcs.Task;
        }

        public void Reply(IResponse message)
        {
            if (this.IsDisposed)
            {
                throw new Exception("session已经被Dispose了");
            }
            this.Send(message);
        }

        public void Send(IMessage message)
        {
            OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
            ushort opcode = opcodeTypeComponent.GetOpcode(message.GetType());
            Send(opcode, message);
        }

        public void Send(ushort opcode, object message)
        {
            if (this.IsDisposed)
            {
                throw new Exception("session已经被Dispose了");
            }

            MemoryStream stream = this.Stream;

            stream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
            stream.SetLength(Packet.MessageIndex);
            this.Network.MessagePacker.SerializeTo(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            opcodeBytes.WriteTo(0, opcode);
            Array.Copy(opcodeBytes, 0, stream.GetBuffer(), 0, opcodeBytes.Length);
            /*
            if (this.channel.RemoteAddress.Port != 20002 && opcode == HotfixOpcode.Actor_CountDown_Ntt)
            {
                int temp = 0;
                temp++;
            }
            */

#if SERVER
            // 如果是allserver，内部消息不走网络，直接转给session,方便调试时看到整体堆栈
            if (this.Network.AppType == AppType.AllServer)
            {
                Session session = this.Network.Entity.GetComponent<NetInnerComponent>().Get(this.RemoteAddress);
                session.Run(stream);
                return;
            }
#endif
            if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
            {
#if !SERVER
                if (OpcodeHelper.IsClientHotfixMessage(opcode))
                {
                }
                else
#endif
                {
                    //屏蔽心跳日志输出 10009
                    //屏蔽广播玩家下注日志 10050
                    if (opcode != 10009 && opcode != 10050 && opcode != 1012 && opcode != 1011 && opcode != 1010 && opcode != 1009)
                    {
                        Log.Msg(message);
                    }
                }
            }

            this.Send(stream);
        }

        public void Send(MemoryStream stream)
        {
            channel.Send(stream);
        }
    }
}