using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_TouristsLogin_ReqHandler : AMRpcHandler<C2G_TouristsLogin_Req, G2C_TouristsLogin_Res>
    {
        protected override async void Run(Session session, C2G_TouristsLogin_Req message, Action<G2C_TouristsLogin_Res> reply)
        {
            G2C_TouristsLogin_Res response = new G2C_TouristsLogin_Res();
            try
            {
                AccountInfo newAccount = ComponentFactory.CreateWithId<AccountInfo>(IdGenerater.GenerateId());
                newAccount.Account = GetRandomString(6);
                newAccount.Password = MakePassword(6);
                
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
               
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>(_account => _account.Account == newAccount.Account);

                if (result.Count > 0) newAccount.Account += RandomHelper.GetRandom().Next(0, 9);
               
                ETModel.UserInfo newUser = UserInfoFactory.Create(newAccount.Id, session);

                newUser.PlayerId = RandomHelper.GenerateRandomCode(6);
                newUser.Account = newAccount.Account;
                newUser.Nickname = $"游客{ RandomHelper.GenerateRandomCode(4):0000}";
                newUser.HeadId = RandomHelper.GetRandom().Next(1, 11);
                if (newUser.HeadId < 6)
                    newUser.Gender = 1;
                else
                    newUser.Gender = 2;
                newUser.Gold = 0;
                newUser.IsTourist = true;
             
                await dbProxy.Save(newAccount);
                await dbProxy.Save(newUser);
                
                response.Account = newAccount.Account;
                response.Password = newAccount.Password;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }


        #region 随机生成账号密码

        // 随机昵称
        string GetRandomString(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        
        //随机生成密码
        string MakePassword(int pwdLength)
        {
            //声明要返回的字符串    
            string tmpstr = "";
            //密码中包含的字符数组    
            string pwdchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //数组索引随机数    
            int iRandNum;

            //随机数生成器    
            for (int i = 0; i < pwdLength; i++)
            {
                iRandNum = RandomHelper.GetRandom().Next(pwdchars.Length);
                //tmpstr随机添加一个字符     
                tmpstr += pwdchars[iRandNum];
            }
            return tmpstr;
        }

        #endregion
    }
}
