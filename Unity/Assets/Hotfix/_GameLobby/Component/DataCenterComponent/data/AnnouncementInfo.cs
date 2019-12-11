//////////////////////////////////////////////////////////
/////公告信息
/////zhouyu 2019.5.13
/////////////////////////////////////////////////////////

namespace ETHotfix
{

    public class AnnouncementInfo
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        public string Title = "";

        /// <summary>
        /// 公告内容
        /// </summary>
        public string Content = "";

        /// <summary>
        /// 时间戮
        /// </summary>
        public string Timestamp = "";

        public void SetData(string title, string content, string timeStamp)
        {
            this.Title = title;

            this.Content = content;

            this.Timestamp = timeStamp;
        }
    }
}
