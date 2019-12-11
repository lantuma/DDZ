using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ETModel
{
	public static class MD5Helper
	{
        public static string key = "4ace05b47e8f45491402f45c3c7eeaca";

        public static string FileMD5(string filePath)
		{
			byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
			{
				MD5 md5 = new MD5CryptoServiceProvider();
				retVal = md5.ComputeHash(file);
			}
			return retVal.ToHex("x2");
		}

        public static string MakeSign(Dictionary<string, string> post)
        {
            var str = "";

            var keys = post.Keys.ToList();
            keys.Sort(string.CompareOrdinal);

            foreach (var k in keys)
            {
                if (k != "sign" && k != "attach")
                {
                    str += $"{k}={post[k]}&";
                }
            }

            str += "key=" + key;
            var res = MD5Helper.GetMD5(str);
            return res;
        }


        public static string MakeSign(int timer)
        {
            var str = "";

            str = $"{timer}{key}";

            var res = MD5Helper.GetMD5(str);

            return res.ToLower();
        }

        public static string MakeSign(string timer)
        {
            var str = "";

            str = $"{timer}{key}";

            var res = MD5Helper.GetMD5(str);

            return res.ToLower();
        }


        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(myString.ToLower());
            byte[] targetData = md5.ComputeHash(fromData);
            md5.Clear();

            var byte2String = new StringBuilder();
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String.Append(targetData[i].ToString("X2"));
            }

            return byte2String.ToString();
        }
    }
}
