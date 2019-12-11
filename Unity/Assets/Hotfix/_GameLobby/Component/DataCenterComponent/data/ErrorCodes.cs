////////////////////////////////////////////////////////
///错误码\协议匹配
///zhouyu 2019.3.12
///////////////////////////////////////////////////////

using System.Collections.Generic;

namespace ETHotfix
{
    public partial class ErrorCodes
    {
        private Dictionary<string, string> mecthCode;

        private Dictionary<string, string> mecthProto;

        /// <summary>
        /// 是否允许输出信息
        /// </summary>
        public bool printInfo = true;

        public ErrorCodes()
        {
            mecthCode = new Dictionary<string, string>();

            mecthProto = new Dictionary<string, string>();
        }

        /// <summary>
        /// 匹对错误码
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getCodeText(string proto, int code)
        {
            var codes = proto + (code >= 0 ? "," + code : "");

            return mecthCode[codes] != null ? mecthCode[codes] : "未定义的错误码:" + codes;
        }

        /// <summary>
        /// 配对协议
        /// </summary>
        /// <param name="proto"></param>
        /// <returns></returns>
        public string cn(string proto)
        {
            return mecthProto[proto] != null ? mecthProto[proto] : "未定义协议码:" + proto;
        }

        public void PrintInfo(string proto,int code = -1)
        {
            if (!printInfo) return;

            string _codeStr = code == -1 ? "" :" 错误码:" + getCodeText(proto, code);

            UnityEngine.Debug.Log("::(" + proto + ")>>>" + cn(proto)+" 协议;" + " >>>>" + _codeStr);
        }
    }
}
