namespace ETModel
{
	public static partial class ErrorCode
	{
		public const int ERR_Success = 0;
		
		// 1-11004 是SocketError请看SocketError定义
		//-----------------------------------
		// 100000 以上，避免跟SocketError冲突
		public const int ERR_MyErrorCode = 100000;
		
		public const int ERR_ActorNoMailBoxComponent = 100003;
		public const int ERR_ActorRemove = 100004;
		public const int ERR_PacketParserError = 100005;
		
		public const int ERR_KcpCantConnect = 102005;
		public const int ERR_KcpChannelTimeout = 102006;
		public const int ERR_KcpRemoteDisconnect = 102007;
		public const int ERR_PeerDisconnect = 102008;
		public const int ERR_SocketCantSend = 102009;
		public const int ERR_SocketError = 102010;
		public const int ERR_KcpWaitSendSizeTooLarge = 102011;

		public const int ERR_WebsocketPeerReset = 103001;
		public const int ERR_WebsocketMessageTooBig = 103002;
		public const int ERR_WebsocketError = 103003;
		public const int ERR_WebsocketConnectError = 103004;
		public const int ERR_WebsocketSendError = 103005;
		public const int ERR_WebsocketRecvError = 103006;
		
		public const int ERR_RpcFail = 102001;
		public const int ERR_ReloadFail = 102003;
	
		public const int ERR_ActorLocationNotFound = 102004;
		//-----------------------------------
		// 小于这个Rpc会抛异常，大于这个异常的error需要自己判断处理，也就是说需要处理的错误应该要大于该值
		public const int ERR_Exception = 200000;
		
		public const int ERR_NotFoundActor = 200002;

       /******************LTMPoker**************************/

        public const int ERR_SessionError = 210004;                       //Session断开

        public const int ERR_LoginError = 210005;                          //登录错误

        public const int ERR_AccountDoesnExist = 210006;           //账号密码不存在

        public const int ERR_ConnectGateKeyError = 200105;        //获取登录秘钥失败

        public const int ERR_AccountAlreadyRegister = 200106;   //注册失败

        public const int ERR_ChangerUserError = 200107;             //修改用户信息失败

        public const int ERR_GetGameListError = 200108;              //获取游戏列表息失败

        public const int ERR_GetAreaError = 200109;                      //获取场信息失败

        public const int ERR_GetUserInfoError = 200110;                //获取用户信息失败
         
        public const int ERR_JionRoomError = 200111;                   //加入房间失败

        public const int ERR_GetRoomInfoError = 200112;             //获取房间信息失败

        public const int ERR_QuitRoomError = 200113;                  //退出房间信息失败

        public const int ERR_XiaZhuangError = 200114;                 //请求下庄失败

        public const int ERR_StandUpError = 200115;                    //请求站起失败

        public const int ERR_BettingError = 200207;                      //下注失败

        public const int ERR_GetRoadListError = 200208;              //获取百家乐场路单信息失败

        public const int ERR_GetTrendChartError = 200209;          //获取牛牛场走势图信息失败

        public const int ERR_LookCardError = 200210;                  //看牌失败

        public const int ERR_DiscardsError = 200211;                     //看牌失败

        public const int ERR_ComparisonCardError = 200212;       //比牌失败

        public const int ERR_GetRankListError = 200213;               //获取排行榜失败

        public const int ERR_CancelSZWaitError = 200214;            //取消上庄排队失败

        public const int ERR_ChangerRoomError = 200215;            //请求换桌失败

        public const int ERR_GetRoomListError = 200216;               //获取房间列表失败

        public const int ERR_GetAnnouncementError = 200217;      //获取公告失败

        public const int ERR_TouristUpgradeVIPError = 200218;      //升级成为vip用户失败

        public const int ERR_PullAnOrderError = 200219;                 //下订单失败

        public const int ERR_ResetPasswordPError = 200220;           //修改密码失败

        public const int ERR_PromotionError = 200221;                    //获取代理失败

        public const int ERR_AddChipsError = 200222;                      //加注失败

        public const int ERR_DDZPrepareError = 200223;                 //斗地主准备失败

        public const int ERR_DDZqdzError = 200224;                         //斗地主抢地主失败

        public const int ERR_DDZplayCardError = 200225;                //斗地主出牌失败

        public const int ERR_CheckError = 200226;                           //请求过牌错误

        public const int ERR_RewardError = 200227;                         //打赏失败

        public const int ERR_RobdoZhuang = 200228;                     //请求抢庄失败

        public const int ERR_AddBetsError = 200229;                      //抢庄牛牛加注失败

        public const int ERR_StartGameError = 200230;                   //开始游戏失败

        public const int ERR_RechargeChannelError = 200231;       //获取充值渠道错误

        public const int ERR_FruitGuessNumError = 200232;          //水果机猜大小错误

        public const int ERR_NickNameError = 200233;                 //昵称已存在

        public const int ERR_YuEBaoDataError = 200234;              //获取余额宝详情错误

        public const int ERR_YuEBaoDetailError = 200235;              //获取余额宝明细失败

        public const int ERR_YuEBaoIntoError = 200236;              //余额宝转入失败

        public const int ERR_YuEBaoRollOutError = 200237;              //余额宝转出失败

        public const int ERR_IsBoltbackError = 200238;                 //获取重回信息失败

        public const int ERR_GetUserError = 200239;                  //获取用户信息失败

        public const int ERR_DirectlyAskError = 200240;               //直属查询失败

        public const int ERR_FindDirectlyError = 200241;            //直属搜索失败

        public const int ERR_PerformanceAskError = 200242;           //业绩查询失败

        public const int ERR_FindPerformanceError = 200243;          //业绩搜索失败

        public const int ERR_ReceiveRewardError = 200244;            //领取佣金失败

        public const int ERR_ReceiveRecordError = 200245;             //获取领取记录失败

        public const int ERR_RechargeRecordAskError = 200246;          //获取充值记录失败

        public const int ERR_BindBankCardAskError = 200247;             //绑定银行卡失败

        public const int ERR_WithdrawError = 200248;                     //提现失败

        public const int ERR_WithdrawScheduleError = 200249;             //获取提现记录失败

        public const int ERR_MobileVerifyCodeError = 200250;             //绑定手机号吗失败

        public const int ERR_IsBoltBackError = 200251;                   //重回失败

        //-----------------------------------
        public static bool IsRpcNeedThrowException(int error)
		{
			if (error == 0)
			{
				return false;
			}

			if (error > ERR_Exception)
			{
				return false;
			}

			return true;
		}
	}
}