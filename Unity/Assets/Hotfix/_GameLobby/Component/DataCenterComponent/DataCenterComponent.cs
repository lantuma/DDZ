/******************************************************************************************
*         【模块】{ 数据中心模块 }                                                                                                                      
*         【功能】{ 游戏数据统一管理}                                                                                                                   
*         【修改日期】{ 2019年3月12日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DataCenterComponent_AwakeSystem : AwakeSystem<DataCenterComponent>
    {
        public override void Awake(DataCenterComponent self)
        {
            self.Awake();
        }
    }

    public class DataCenterComponent:Component
    {
        public static DataCenterComponent Instance { get; private set; }

        /// <summary>
        /// 色彩信息
        /// </summary>
        public ColorInfo colorInfo;

        /// <summary>
        /// 用户信息
        /// </summary>
        public EUserInfo userInfo;

        /// <summary>
        /// 房间信息
        /// </summary>
        public RoomInfo roomInfo;

        /// <summary>
        /// 错误码信息
        /// </summary>
        public ErrorCodes errorCodes;

        /// <summary>
        /// 声音信息
        /// </summary>
        public SoundInfo soundInfo;

        /// <summary>
        /// 游戏信息
        /// </summary>
        public GameInfos gameInfo;

        /// <summary>
        /// 推广码信息
        /// </summary>
        public QRCodeInfo QRcodeInfo;

        /// <summary>
        /// 公告信息
        /// </summary>
        public AnnouncementInfo AnnouncementInfo;

        /// <summary>
        /// 提示文字信息
        /// </summary>
        public TipInfo tipInfo;

        /// <summary>
        /// 游戏重回信息
        /// </summary>
        public GameReBackInfo GameReBackInfo;

        /// <summary>
        /// 跑马灯信息
        /// </summary>
        public MarqueeInfo MarqueeInfo;

        /// <summary>
        /// 客服信息
        /// </summary>
        public CustomerInfo CustomerInfo;

        /// <summary>
        /// 后台配置信息
        /// </summary>
        public WebConfigInfo WebConfigInfo;

        public void Awake()
        {
            Instance = this;

            colorInfo = new ColorInfo();

            userInfo = new EUserInfo();

            roomInfo = new RoomInfo();

            errorCodes = new ErrorCodes();

            soundInfo = new SoundInfo();

            gameInfo = new GameInfos();

            QRcodeInfo = new QRCodeInfo();

            AnnouncementInfo = new AnnouncementInfo();

            tipInfo = new TipInfo();

            GameReBackInfo = new GameReBackInfo();

            MarqueeInfo = new MarqueeInfo();

            CustomerInfo = new CustomerInfo();

            WebConfigInfo = new WebConfigInfo();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Awake();
        }
    }
}
