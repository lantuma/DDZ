using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class GameHelper
    {
        private static Dictionary<GameType, GameInfo> GameInfos = new Dictionary<GameType, GameInfo>();

        public static Dictionary<int, List<AreaInfo>> AreaList = new Dictionary<int, List<AreaInfo>>();

        public static Dictionary<int, List<RoomListInfo>> roomInfoList = new Dictionary<int, List<RoomListInfo>>();

        public static GameInfo CurrentGameInfo;

        public static AreaInfo CurrentAreaInfo;

        /// <summary>
        /// 当前房间ID
        /// </summary>
        public static int CurrentRoomId;

        /// <summary>
        /// 当前场Id
        /// </summary>
        public static int CurrentFieldId;

        /// <summary>
        /// 是否接收游戏广播的消息
        /// </summary>
        public static bool IsReciveGameNoticeHandle;

        /// <summary>
        /// 是否处理后台
        /// </summary>
        public static bool ApplicationIsPause = false;
        /// <summary>
        /// 当前房间级别
        /// </summary>
        public static int CurrRoomLevel;


        #region 私有变量

        //        private static MessageBoxCpt _messageBoxCpt;

        #endregion

        /// <summary>
        /// 获取游戏信息
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public static GameInfo GetGameInfo(GameType gameType)
        {
            if (!GameInfos.ContainsKey(gameType))
            {
                return null;
            }
            return GameInfos[gameType];
        }

        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<GameType, GameInfo>> GetGameList()
        {
            try
            {
                if (GameInfos != null && GameInfos.Count > 0) return GameInfos;

                Game.PopupComponent.ShowLoadingLockUI(DataCenterComponent.Instance.tipInfo.GetGameListTip);

                G2C_GetGameList_Res getGameList = (G2C_GetGameList_Res)await SessionComponent.Instance.Session.Call(new C2G_GetGameList_Req());

                Game.PopupComponent.CloseLoadingLockUI();

                if (getGameList.Error != 0)
                {
                    Debug.Log(getGameList.Message);
                    //                    ShowMessageBox(getGameList.Message);
                    Game.PopupComponent.ShowMessageBox(getGameList.Message);
                    return null;
                }

                var gameInfo = getGameList.GameInfo.ToList();
                if (GameInfos == null) GameInfos = new Dictionary<GameType, GameInfo>();
                foreach (var info in gameInfo)
                {
                    if (!GameInfos.ContainsKey((GameType)info.GameId))
                    {
                        GameInfos.Add((GameType)info.GameId, info);
                    }
                    else
                    {
                        GameInfos[(GameType)info.GameId] = info;
                    }
                }
                return GameInfos;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取游戏的房间列表
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        /// <returns></returns>
        public static async Task<List<AreaInfo>> GetGameAreaList(int gameId)
        {
            try
            {
                G2C_GetAreaList_Res areaList = (G2C_GetAreaList_Res)await SessionComponent.Instance.Session.Call
                        (new C2G_GetAreaList_Req() { GameId = gameId });

                if (areaList.Error != 0)
                {
                    UnityEngine.Debug.Log(areaList.Message);
                    //                    ShowMessageBox(areaList.Message);
                    Game.PopupComponent.ShowMessageBox(areaList.Message);
                    return null;
                }

                var areas = areaList.AreaInfo.ToList();

                if (AreaList.ContainsKey(gameId))
                {
                    AreaList[gameId] = areas;
                }
                else
                {
                    AreaList.Add(gameId, areas);
                }

                //foreach (var area in areas)
                //{
                //    UnityEngine.Debug.Log($"ID: {area.AreaId},TYPE: {area.AreaType},SCORE: {area.Score},GAMEID: {area.GameId}");
                //}

                return areas;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="AreadId"></param>
        /// <returns></returns>
        public static async Task<List<RoomListInfo>> GetRoomList(int GameId, int AreadId)
        {
            try
            {
                G2C_GetRoomLis_Res response = (G2C_GetRoomLis_Res)await SessionComponent.Instance.Session.Call
                        (new C2G_GetRoomList_Req() { GameId = GameId, AreaId = AreadId });

                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowTips(response.Message);

                    return null;
                }

                var roomList = response.RoomList.ToList();

                if (roomInfoList.ContainsKey(GameId))
                {
                    roomInfoList[GameId] = roomList;
                }
                else
                {
                    roomInfoList.Add(GameId, roomList);
                }

                return roomList;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return null;
            }
        }

        #region 通用正则表达式验证
        //        private readonly string AccountRegexStr = @"^(?![^A-z]+$)(?!\D+$)[A-z\d]{6,10}$"; // 数字和字母混合(至少包含一个数字和字母),首位必须为字母,6-10位
        private const string AccountRegexStr = @"^[A-z\d]{6,10}$";   // 数字或字母组成,6-10位
        private const string PasswordRegexStr = @"^[A-z\d]{6,10}$";   // 数字或字母组成,6-10位

        /// <summary>
        /// 验证账号是否合法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool VerifyAccountText(string str, out string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                Log.Warning("账号不能为空!");
                message = "账号不能为空!";
                return false;
            }

            if (!Regex.IsMatch(str, AccountRegexStr))
            {
                Log.Warning("账号必须由长度为6-10位的数字或字母组成!");
                message = "账号必须由长度为6-10位的数字或字母组成!";
                return false;
            }
            message = "";
            return true;
        }

        /// <summary>
        /// 验证密码是否合法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool VerifyPasswordText(string str, out string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                Log.Warning("密码不能为空!");
                message = "密码不能为空!";
                return false;
            }

            if (!Regex.IsMatch(str, PasswordRegexStr))
            {
                Log.Warning("密码必须由长度为6-10位的数字或字母组成!");
                message = "密码必须由长度为6-10位的数字或字母组成!";
                return false;
            }
            message = "";
            return true;
        }

        #endregion
        
        #region 获取图片Sprite

        /// <summary>
        /// 卡牌图集预设名称
        /// </summary>
        public const string AB_NAME = "poker";

        /// <summary>
        /// 牌型名称
        /// </summary>
        public const string PaiXing_NAME = "paixing";
        

        public static Sprite GetSprite(string abName, string spriteName)
        {
            try
            {
                var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                res.LoadBundle(abName.StringToAB());
                if (Define.UseEditorResouces)
                {
                    var texture = (Texture2D)res.GetAsset(abName.StringToAB(), spriteName);

                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                else
                {
                    return (Sprite)res.GetAsset(abName.StringToAB(), spriteName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"not found {abName.StringToAB()} {spriteName}");
                throw e;
            }
        }

        /// <summary>
        /// 获取扑克牌
        /// </summary>
        /// <param name="poker"></param>
        /// <param name="isPokerBg"></param>
        /// <returns></returns>
        public static Sprite GetPokerSprite(int poker, bool isPokerBg = false)
        {
            if (isPokerBg)
            {
                return GetSprite(AB_NAME, "poker_paiBG@2x");
            }
            //0方块 1红桃 2梅花 3黑桃
            int hua = poker / 13;
            int pokerValue = poker % 13;
            string huaMsg = "";
            switch (hua)
            {
                case 3:
                    huaMsg = "f";
                    break;
                case 1:
                    huaMsg = "t";
                    break;
                case 2:
                    huaMsg = "w";
                    break;
                case 0:
                    huaMsg = "h";
                    break;
            }
            return GetSprite(AB_NAME, string.Format("poker_{0}{1}", huaMsg, pokerValue + 1));
        }

        /// <summary>
        /// 获取卡牌精灵
        /// </summary>
        /// <param name="cardValue">牌值</param>
        /// <returns></returns>
        public static Sprite GetPokerSprite(string cardName)
        {
            return GetSprite(AB_NAME, "poker_" + cardName);
        }

        /// <summary>
        /// 获取卡牌精灵
        /// </summary>
        /// <param name="cardValue">牌值</param>
        /// <returns></returns>
        public static Sprite GetPaiXingSprite(int index, bool isPlay)
        {
            if (isPlay)
                SoundComponent.Instance.PlayClip($"male_niu_{index - 10}");
            return GetSprite(PaiXing_NAME, string.Format("msgtips_type{0}@2x", index - 10));
        }

        /// <summary>
        /// 获取tips Sprite
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Sprite GetTipsSprite(string msg)
        {
            return GetSprite(PaiXing_NAME, msg);
        }

        public static Sprite GetChipsSprite(string msg)
        {
            return GetSprite("chips", msg);
        }

        #endregion

        
        
        public static string ConvertCoinToString(float coin, bool isScore = false)
        {
            if (!isScore && coin < 0) return "0.00";
            if (Mathf.Abs(coin) < 1e4) return $"{coin:###0.00}";
            // ReSharper disable once PossibleLossOfFraction
            if (Mathf.Abs(coin) < 1e8) return $"{Math.Floor(coin / 1e3) / 1e1:F1}万";
            return $"∞";
        }

        private static string _replaceNameWarp = "...";
        
        public static string ConvertNickNameWarp(string name, int count = 5, string replace = null)
        {
            return name.Length <= count ? name : name.Replace(name.Substring(count - 1), string.IsNullOrEmpty(replace) ? _replaceNameWarp : replace);
        }
        
        public static string GetGameTypeName(int type)
        {
            switch (type)
            {
                case 1:
                    return "斗地主";
                default:
                    return $"游戏ID {type}";
            }
        }
        public static string GetShareQRCodeURL()
        {
            string shareQRCodeUrl = "";
            return shareQRCodeUrl;
        }
        
    }

    /// <summary>
    /// 游戏类型
    /// </summary>
    public enum GameType
    {
        DouDiZhu = 1,
        
    }

}