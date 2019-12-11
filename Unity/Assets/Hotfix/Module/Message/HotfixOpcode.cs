using ETModel;
namespace ETHotfix
{
	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.Actor_Test1_Ntt)]
	public partial class Actor_Test1_Ntt : IActorMessage {}

// 心跳包
	[Message(HotfixOpcode.PingRequest)]
	public partial class PingRequest : IRequest {}

	[Message(HotfixOpcode.PingResponse)]
	public partial class PingResponse : IResponse {}

	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.UserInfo)]
	public partial class UserInfo : IMessage {}

	[Message(HotfixOpcode.C2G_MobileVeirfyCode_Req)]
	public partial class C2G_MobileVeirfyCode_Req : IRequest {}

	[Message(HotfixOpcode.G2C_MobileVeirfyCode_Res)]
	public partial class G2C_MobileVeirfyCode_Res : IResponse {}

	[Message(HotfixOpcode.C2G_MobileLogin_Req)]
	public partial class C2G_MobileLogin_Req : IRequest {}

	[Message(HotfixOpcode.G2C_MobileLogin_Res)]
	public partial class G2C_MobileLogin_Res : IResponse {}

	[Message(HotfixOpcode.C2G_TokenLogin_Req)]
	public partial class C2G_TokenLogin_Req : IRequest {}

	[Message(HotfixOpcode.G2C_TokenLogin_Res)]
	public partial class G2C_TokenLogin_Res : IResponse {}

	[Message(HotfixOpcode.C2R_Register_Req)]
	public partial class C2R_Register_Req : IRequest {}

	[Message(HotfixOpcode.R2C_Register_Res)]
	public partial class R2C_Register_Res : IResponse {}

	[Message(HotfixOpcode.C2G_GetUserInfo_Req)]
	public partial class C2G_GetUserInfo_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetUserInfo_Res)]
	public partial class G2C_GetUserInfo_Res : IResponse {}

	[Message(HotfixOpcode.C2G_ChangerUserInfo_Req)]
	public partial class C2G_ChangerUserInfo_Req : IRequest {}

	[Message(HotfixOpcode.G2C_ChangerUserInfo_Res)]
	public partial class G2C_ChangerUserInfo_Res : IResponse {}

	[Message(HotfixOpcode.GameInfo)]
	public partial class GameInfo : IMessage {}

	[Message(HotfixOpcode.C2G_GetGameList_Req)]
	public partial class C2G_GetGameList_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetGameList_Res)]
	public partial class G2C_GetGameList_Res : IResponse {}

	[Message(HotfixOpcode.AreaInfo)]
	public partial class AreaInfo : IMessage {}

	[Message(HotfixOpcode.C2G_GetAreaList_Req)]
	public partial class C2G_GetAreaList_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetAreaList_Res)]
	public partial class G2C_GetAreaList_Res : IResponse {}

	[Message(HotfixOpcode.RoomListInfo)]
	public partial class RoomListInfo : IMessage {}

	[Message(HotfixOpcode.C2G_GetRoomList_Req)]
	public partial class C2G_GetRoomList_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetRoomLis_Res)]
	public partial class G2C_GetRoomLis_Res : IResponse {}

	[Message(HotfixOpcode.PlayerData)]
	public partial class PlayerData : IMessage {}

	[Message(HotfixOpcode.RoomData)]
	public partial class RoomData : IMessage {}

	[Message(HotfixOpcode.C2G_JionRoom_Req)]
	public partial class C2G_JionRoom_Req : IRequest {}

	[Message(HotfixOpcode.G2C_JionRoom_Res)]
	public partial class G2C_JionRoom_Res : IResponse {}

	[Message(HotfixOpcode.Actor_JionRoom_Ntt)]
	public partial class Actor_JionRoom_Ntt : IActorMessage {}

	[Message(HotfixOpcode.C2G_GetRoomInfo_Req)]
	public partial class C2G_GetRoomInfo_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetRoomInfo_Res)]
	public partial class G2C_GetRoomInfo_Res : IResponse {}

	[Message(HotfixOpcode.Actor_CountDown_Ntt)]
	public partial class Actor_CountDown_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Card)]
	public partial class Card : IMessage {}

	[Message(HotfixOpcode.C2G_QuitRoom_Req)]
	public partial class C2G_QuitRoom_Req : IRequest {}

	[Message(HotfixOpcode.G2C_QuitRoom_Res)]
	public partial class G2C_QuitRoom_Res : IResponse {}

	[Message(HotfixOpcode.Actor_OtherPlayerSitDown_Ntt)]
	public partial class Actor_OtherPlayerSitDown_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_OtherPlayerStand_Ntt)]
	public partial class Actor_OtherPlayerStand_Ntt : IActorMessage {}

	[Message(HotfixOpcode.C2G_GetRankList_Req)]
	public partial class C2G_GetRankList_Req : IRequest {}

	[Message(HotfixOpcode.G2C_GetRankList_Res)]
	public partial class G2C_GetRankList_Res : IResponse {}

	[Message(HotfixOpcode.C2G_ChangerRoom_Req)]
	public partial class C2G_ChangerRoom_Req : IRequest {}

	[Message(HotfixOpcode.G2C_ChangerRoom_Res)]
	public partial class G2C_ChangerRoom_Res : IResponse {}

	[Message(HotfixOpcode.AnnounceInfo)]
	public partial class AnnounceInfo : IMessage {}

	[Message(HotfixOpcode.C2G_Announcement_Req)]
	public partial class C2G_Announcement_Req : IRequest {}

	[Message(HotfixOpcode.G2C_Announcement_Res)]
	public partial class G2C_Announcement_Res : IResponse {}

	[Message(HotfixOpcode.Record)]
	public partial class Record : IMessage {}

	[Message(HotfixOpcode.C2G_MyRecord_Req)]
	public partial class C2G_MyRecord_Req : IRequest {}

	[Message(HotfixOpcode.G2C_MyRecord_Res)]
	public partial class G2C_MyRecord_Res : IResponse {}

	[Message(HotfixOpcode.C2G_TouristsLogin_Req)]
	public partial class C2G_TouristsLogin_Req : IRequest {}

	[Message(HotfixOpcode.G2C_TouristsLogin_Res)]
	public partial class G2C_TouristsLogin_Res : IResponse {}

	[Message(HotfixOpcode.C2G_ResetPassword_Req)]
	public partial class C2G_ResetPassword_Req : IRequest {}

	[Message(HotfixOpcode.G2C_ResetPassword_Res)]
	public partial class G2C_ResetPassword_Res : IResponse {}

	[Message(HotfixOpcode.Actor_PlayerOffline_Ntt)]
	public partial class Actor_PlayerOffline_Ntt : IActorMessage {}

	[Message(HotfixOpcode.DDZPlayerInfo)]
	public partial class DDZPlayerInfo : IMessage {}

	[Message(HotfixOpcode.DDZRoomData)]
	public partial class DDZRoomData : IMessage {}

	[Message(HotfixOpcode.C2G_DDZPrepare_Req)]
	public partial class C2G_DDZPrepare_Req : IRequest {}

	[Message(HotfixOpcode.G2C_DDZPrepare_Res)]
	public partial class G2C_DDZPrepare_Res : IResponse {}

	[Message(HotfixOpcode.C2G_DDZAskScore_Req)]
	public partial class C2G_DDZAskScore_Req : IRequest {}

	[Message(HotfixOpcode.G2C_DDZAskScore_Res)]
	public partial class G2C_DDZAskScore_Res : IResponse {}

	[Message(HotfixOpcode.Actor_DDZMakeLord_Ntt)]
	public partial class Actor_DDZMakeLord_Ntt : IActorMessage {}

	[Message(HotfixOpcode.DDZCard)]
	public partial class DDZCard : IMessage {}

	[Message(HotfixOpcode.Actor_DDZDealHandCard_Ntt)]
	public partial class Actor_DDZDealHandCard_Ntt : IActorMessage {}

	[Message(HotfixOpcode.C2G_DDZPlayCard_Req)]
	public partial class C2G_DDZPlayCard_Req : IRequest {}

	[Message(HotfixOpcode.G2C_DDZPlayCard_Res)]
	public partial class G2C_DDZPlayCard_Res : IResponse {}

	[Message(HotfixOpcode.Actor_DDZhSettlement_Req_Ntt)]
	public partial class Actor_DDZhSettlement_Req_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_FirstQdz_Req_Ntt)]
	public partial class Actor_FirstQdz_Req_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_DDZTurnPlayer_Ntt)]
	public partial class Actor_DDZTurnPlayer_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_DDZLord_Ntt)]
	public partial class Actor_DDZLord_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_OtherPlayCard_Ntt)]
	public partial class Actor_OtherPlayCard_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_JionDDZRoom_Ntt)]
	public partial class Actor_JionDDZRoom_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_QuitDDZHRoom_Ntt)]
	public partial class Actor_QuitDDZHRoom_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_OtherPrepare_Ntt)]
	public partial class Actor_OtherPrepare_Ntt : IActorMessage {}

	[Message(HotfixOpcode.Actor_OtherQdz_Ntt)]
	public partial class Actor_OtherQdz_Ntt : IActorMessage {}

	[Message(HotfixOpcode.C2G_IsBoltback_Req)]
	public partial class C2G_IsBoltback_Req : IRequest {}

	[Message(HotfixOpcode.G2C_IsBoltback_Res)]
	public partial class G2C_IsBoltback_Res : IResponse {}

	[Message(HotfixOpcode.Actor_UpdateUserInfo_Ntt)]
	public partial class Actor_UpdateUserInfo_Ntt : IActorMessage {}

	[Message(HotfixOpcode.MailInfo)]
	public partial class MailInfo : IMessage {}

	[Message(HotfixOpcode.C2G_MailAsk_Req)]
	public partial class C2G_MailAsk_Req : IRequest {}

	[Message(HotfixOpcode.G2C_MailReturn_Res)]
	public partial class G2C_MailReturn_Res : IResponse {}

	[Message(HotfixOpcode.Actor_ScrollToNotice_Ntt)]
	public partial class Actor_ScrollToNotice_Ntt : IActorMessage {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort G2C_TestHotfixMessage = 10001;
		 public const ushort C2M_TestActorRequest = 10002;
		 public const ushort M2C_TestActorResponse = 10003;
		 public const ushort PlayerInfo = 10004;
		 public const ushort C2G_PlayerInfo = 10005;
		 public const ushort G2C_PlayerInfo = 10006;
		 public const ushort Actor_Test1_Ntt = 10007;
		 public const ushort PingRequest = 10008;
		 public const ushort PingResponse = 10009;
		 public const ushort C2R_Login = 10010;
		 public const ushort R2C_Login = 10011;
		 public const ushort C2G_LoginGate = 10012;
		 public const ushort G2C_LoginGate = 10013;
		 public const ushort UserInfo = 10014;
		 public const ushort C2G_MobileVeirfyCode_Req = 10015;
		 public const ushort G2C_MobileVeirfyCode_Res = 10016;
		 public const ushort C2G_MobileLogin_Req = 10017;
		 public const ushort G2C_MobileLogin_Res = 10018;
		 public const ushort C2G_TokenLogin_Req = 10019;
		 public const ushort G2C_TokenLogin_Res = 10020;
		 public const ushort C2R_Register_Req = 10021;
		 public const ushort R2C_Register_Res = 10022;
		 public const ushort C2G_GetUserInfo_Req = 10023;
		 public const ushort G2C_GetUserInfo_Res = 10024;
		 public const ushort C2G_ChangerUserInfo_Req = 10025;
		 public const ushort G2C_ChangerUserInfo_Res = 10026;
		 public const ushort GameInfo = 10027;
		 public const ushort C2G_GetGameList_Req = 10028;
		 public const ushort G2C_GetGameList_Res = 10029;
		 public const ushort AreaInfo = 10030;
		 public const ushort C2G_GetAreaList_Req = 10031;
		 public const ushort G2C_GetAreaList_Res = 10032;
		 public const ushort RoomListInfo = 10033;
		 public const ushort C2G_GetRoomList_Req = 10034;
		 public const ushort G2C_GetRoomLis_Res = 10035;
		 public const ushort PlayerData = 10036;
		 public const ushort RoomData = 10037;
		 public const ushort C2G_JionRoom_Req = 10038;
		 public const ushort G2C_JionRoom_Res = 10039;
		 public const ushort Actor_JionRoom_Ntt = 10040;
		 public const ushort C2G_GetRoomInfo_Req = 10041;
		 public const ushort G2C_GetRoomInfo_Res = 10042;
		 public const ushort Actor_CountDown_Ntt = 10043;
		 public const ushort Card = 10044;
		 public const ushort C2G_QuitRoom_Req = 10045;
		 public const ushort G2C_QuitRoom_Res = 10046;
		 public const ushort Actor_OtherPlayerSitDown_Ntt = 10047;
		 public const ushort Actor_OtherPlayerStand_Ntt = 10048;
		 public const ushort C2G_GetRankList_Req = 10049;
		 public const ushort G2C_GetRankList_Res = 10050;
		 public const ushort C2G_ChangerRoom_Req = 10051;
		 public const ushort G2C_ChangerRoom_Res = 10052;
		 public const ushort AnnounceInfo = 10053;
		 public const ushort C2G_Announcement_Req = 10054;
		 public const ushort G2C_Announcement_Res = 10055;
		 public const ushort Record = 10056;
		 public const ushort C2G_MyRecord_Req = 10057;
		 public const ushort G2C_MyRecord_Res = 10058;
		 public const ushort C2G_TouristsLogin_Req = 10059;
		 public const ushort G2C_TouristsLogin_Res = 10060;
		 public const ushort C2G_ResetPassword_Req = 10061;
		 public const ushort G2C_ResetPassword_Res = 10062;
		 public const ushort Actor_PlayerOffline_Ntt = 10063;
		 public const ushort DDZPlayerInfo = 10064;
		 public const ushort DDZRoomData = 10065;
		 public const ushort C2G_DDZPrepare_Req = 10066;
		 public const ushort G2C_DDZPrepare_Res = 10067;
		 public const ushort C2G_DDZAskScore_Req = 10068;
		 public const ushort G2C_DDZAskScore_Res = 10069;
		 public const ushort Actor_DDZMakeLord_Ntt = 10070;
		 public const ushort DDZCard = 10071;
		 public const ushort Actor_DDZDealHandCard_Ntt = 10072;
		 public const ushort C2G_DDZPlayCard_Req = 10073;
		 public const ushort G2C_DDZPlayCard_Res = 10074;
		 public const ushort Actor_DDZhSettlement_Req_Ntt = 10075;
		 public const ushort Actor_FirstQdz_Req_Ntt = 10076;
		 public const ushort Actor_DDZTurnPlayer_Ntt = 10077;
		 public const ushort Actor_DDZLord_Ntt = 10078;
		 public const ushort Actor_OtherPlayCard_Ntt = 10079;
		 public const ushort Actor_JionDDZRoom_Ntt = 10080;
		 public const ushort Actor_QuitDDZHRoom_Ntt = 10081;
		 public const ushort Actor_OtherPrepare_Ntt = 10082;
		 public const ushort Actor_OtherQdz_Ntt = 10083;
		 public const ushort C2G_IsBoltback_Req = 10084;
		 public const ushort G2C_IsBoltback_Res = 10085;
		 public const ushort Actor_UpdateUserInfo_Ntt = 10086;
		 public const ushort MailInfo = 10087;
		 public const ushort C2G_MailAsk_Req = 10088;
		 public const ushort G2C_MailReturn_Res = 10089;
		 public const ushort Actor_ScrollToNotice_Ntt = 10090;
	}
}
