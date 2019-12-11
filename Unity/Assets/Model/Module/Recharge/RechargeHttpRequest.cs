namespace ETModel
{
    public class RechargeHttpRequest
    {
        /// <summary>
        /// 商户ID,在后台获取
        /// </summary>
        public string merchant_no;
        /// <summary>
        /// 参考微信支付的签名,详情可以查看demo
        /// </summary>
        public string sign;
        /// <summary>
        /// 公众号支付:wx_pub,微信h5支付:wx_h5
        /// </summary>
        public string trade_type;
        /// <summary>
        /// 订单金额,单位为元
        /// </summary>
        public string money;
        /// <summary>
        /// 异步接收支付结果通知的回调地址，通知url必须为外网可访问的url。
        /// </summary>
        public string notify_url;
        /// <summary>
        /// 支付页面,无论支付结果如何都会返回该链接.
        /// </summary>
        public string back_url;
        /// <summary>
        /// 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|* 且在同一个商户号下唯一。
        /// </summary>
        public string out_trade_no;
    }

    public class RechargeHttpResponse
    {
        /// <summary>
        /// 0代表请求失败,1代表成功
        /// </summary>
        public int code;
        /// <summary>
        /// 返回的消息
        /// </summary>
        public string msg;
        /// <summary>
        /// code为1的才会返回该参数,在微信直接跳转到这个链接进行支付
        /// </summary>
        public string pay_url;
        /// <summary>
        /// code为1的才会返回该参数,此为平台的订单号
        /// </summary>
        public string back_trade_no;

        public string pay_info;

        public string status;
    }

    public class RechargeRecord
    {
        //充值玩家
        public long UserId { get; set; }

        //签名
        public string Sign { get; set; }
        //充值金额
        public int Amount { get; set; }

        //商户订单号
        public string OrderNumber { get; set; }

        //渠道号
        public int ChannelNumber { get; set; }

        //充值创建时间
        public long CreateTick { get; set; }

        //付款时间
        public string PaymentTime { get; set; }
    }

    public class RechargeResponse
    {
        /// <summary>
        /// 0代表请求失败,1代表成功
        /// </summary>
        public int code;
        /// <summary>
        /// 返回的消息
        /// </summary>
        public string msg;
    }

    /// <summary>
    /// 用于JSON解析
    /// </summary>
    public class GetCustomerUrlResponse
    {
        public int id;

        public string name;

        public string link;

        public string create_time;
    }

    /// <summary>
    /// 游戏后台配置解析
    /// </summary>
    public class WebConfigResponse
    {
        /// <summary>
        /// 百人牛牛
        /// </summary>
        public int brnn;

        /// <summary>
        /// 百家乐
        /// </summary>
        public int bjl;

        /// <summary>
        /// 龙虎斗
        /// </summary>
        public int lhd;

        /// <summary>
        /// 红黑大战
        /// </summary>
        public int hhdz;

        /// <summary>
        /// 斗地主
        /// </summary>
        public int ddz;

        /// <summary>
        /// 水果机
        /// </summary>
        public int sgj;

        /// <summary>
        /// 抢庄牛牛
        /// </summary>
        public int qznn;

        /// <summary>
        /// 炸金花
        /// </summary>
        public int zjh;

        /// <summary>
        /// 德州扑克
        /// </summary>
        public int dzpk;

        /// <summary>
        /// 广告页
        /// </summary>
        public int ggy;

        /// <summary>
        /// 余额宝
        /// </summary>
        public int yeb;

        /// <summary>
        /// 推广
        /// </summary>
        public int tg;

        /// <summary>
        /// 提现
        /// </summary>
        public int tx;

        /// <summary>
        /// 战绩
        /// </summary>
        public int zj;

        /// <summary>
        /// 客服
        /// </summary>
        public int kf;

        /// <summary>
        /// 公告
        /// </summary>
        public int gg;

        /// <summary>
        /// 设置
        /// </summary>
        public int sz;

        /// <summary>
        /// 大厅背景图
        /// </summary>
        public string dtbjt;

        /// <summary>
        /// 广告页(用,分隔)
        /// </summary>
        public string ggytp;

        /// <summary>
        /// 客服
        /// </summary>
        public string kfwa;

        /// <summary>
        /// 预留1
        /// </summary>
        public string dy1;

        /// <summary>
        /// 预留2
        /// </summary>
        public string dy2;

        /// <summary>
        /// 预留3
        /// </summary>
        public string dy3;

        /// <summary>
        /// 预留4
        /// </summary>
        public string dy4;

        /// <summary>
        /// 预留5
        /// </summary>
        public string dy5;

        /// <summary>
        /// 资源版本号
        /// </summary>
        public string zybbh;

        /// <summary>
        /// 大版本号
        /// </summary>
        public string dbbh;

        /// <summary>
        /// 大厅顶部背景图
        /// </summary>
        public string dtdbtp;

        /// <summary>
        /// 大厅底部背景图
        /// </summary>
        public string dtdibtp;

        /// <summary>
        /// 登陆背景图
        /// </summary>
        public string dlbjt;

        /// <summary>
        /// 登陆Logo
        /// </summary>
        public string dllogo;
    }

}