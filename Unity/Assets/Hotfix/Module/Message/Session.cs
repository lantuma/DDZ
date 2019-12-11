using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class SessionAwakeSystem : AwakeSystem<Session, ETModel.Session>
	{
		public override void Awake(Session self, ETModel.Session session)
		{
			self.session = session;
			SessionCallbackComponent sessionComponent = self.session.AddComponent<SessionCallbackComponent>();
			sessionComponent.MessageCallback = (s, opcode, memoryStream) => { self.Run(s, opcode, memoryStream); };
			sessionComponent.DisposeCallback = s => { self.Dispose(); };
		}
	}

	/// <summary>
	/// 用来收发热更层的消息
	/// </summary>
	public class Session: Entity
	{
		public ETModel.Session session;

		private static int RpcId { get; set; }
		private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (Action<IResponse> action in this.requestCallback.Values.ToArray())
			{
				action.Invoke(new ResponseMessage { Error = this.session.Error });
			}

			this.requestCallback.Clear();

			this.session.Dispose();
		}

		public void Run(ETModel.Session s, ushort opcode, MemoryStream memoryStream)
		{
			OpcodeTypeComponent opcodeTypeComponent = Game.Scene.GetComponent<OpcodeTypeComponent>();
			object instance = opcodeTypeComponent.GetInstance(opcode);
			object message = this.session.Network.MessagePacker.DeserializeFrom(instance, memoryStream);

            //屏蔽心跳日志输出 10009，和广播玩家下注日志 10044  && opcode!= 10044
            if (OpcodeHelper.IsNeedDebugLogMessage(opcode) && opcode != 10009 && opcode != 10044)
			{
				Log.Msg(message,1);
			}

			IResponse response = message as IResponse;

            //临时解决斗地主,没法收到消息的问题
            if (response != null  && response.Error == 200225)
            {
                Game.EventSystem.Run(EventIdType.DDZOutCardError,response.Message);
            }

            if (response == null)
			{
				Game.Scene.GetComponent<MessageDispatcherComponent>().Handle(session, new MessageInfo(opcode, message));
				return;
			}
			
			Action<IResponse> action;
			if (!this.requestCallback.TryGetValue(response.RpcId, out action))
			{
				throw new Exception($"not found rpc, response message: {StringHelper.MessageToStr(response)}");
			}
			this.requestCallback.Remove(response.RpcId);

			action(response);
		}

		public void Send(IMessage message)
		{
			ushort opcode = Game.Scene.GetComponent<OpcodeTypeComponent>().GetOpcode(message.GetType());
			this.Send(opcode, message);
		}

        public void Send(ushort opcode, IMessage message)
        {
            if (OpcodeHelper.IsNeedDebugLogMessage(opcode) && opcode != 10008)
            {
                Log.Msg(message, 0);
            }
            session.Send(opcode, message);
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

			cancellationToken.Register(() => { this.requestCallback.Remove(rpcId); });

			request.RpcId = rpcId;

			this.Send(request);
			return tcs.Task;
		}
	}
}
