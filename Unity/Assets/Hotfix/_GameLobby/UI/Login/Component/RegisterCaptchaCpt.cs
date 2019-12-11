using System.Text;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ETHotfix
{
    [ObjectSystem]
    public class RegisterCaptchaCptAwakeSystem : AwakeSystem<RegisterCaptchaCpt, ReferenceCollector>
    {
        public override void Awake(RegisterCaptchaCpt self, ReferenceCollector a)
        {
            self.Awake(a);
        }
    }

    public class RegisterCaptchaCpt : Component
    {
        private RectTransform _disturbArea;
        private RectTransform _captchaItem1;
        private RectTransform _captchaItem2;
        private RectTransform _captchaItem3;
        private RectTransform _captchaItem4;
        private RectTransform _disturbLine1;
        private RectTransform _disturbLine2;
        private RectTransform _disturbLine3;

        private StringBuilder _currentCaptcha;

        public void Awake(ReferenceCollector rc)
        {
            GameObject = rc.Get<GameObject>("CaptchaBg");

            _disturbArea = rc.GetComponent<RectTransform>("DisturbArea");
            _captchaItem1 = rc.GetComponent<RectTransform>("CaptchaItem1");
            _captchaItem2 = rc.GetComponent<RectTransform>("CaptchaItem2");
            _captchaItem3 = rc.GetComponent<RectTransform>("CaptchaItem3");
            _captchaItem4 = rc.GetComponent<RectTransform>("CaptchaItem4");
            _disturbLine1 = rc.GetComponent<RectTransform>("DisturbLine1");
            _disturbLine2 = rc.GetComponent<RectTransform>("DisturbLine2");
            _disturbLine3 = rc.GetComponent<RectTransform>("DisturbLine3");

            _currentCaptcha = new StringBuilder();
        }

        /// <summary>
        /// 验证输入验证码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Validation(string str)
        {
            if (string.IsNullOrEmpty(_currentCaptcha.ToString()))
            {
                return "验证码超时,请刷新验证码!";
            }

            if (string.IsNullOrEmpty(str))
            {
                return "验证码不能为空!";
            }

            if (string.Equals(str, _currentCaptcha.ToString()))
            {
                return null;
            }
            return "验证码错误,请重新输入!";
        }

        /// <summary>
        /// 显示/刷新 验证码
        /// </summary>
        public void ShowCaptcha()
        {
//            if (!GameObject.activeSelf) GameObject.SetActive(true);

            // 刷新验证码
            RefreshCaptcha();

            // 刷新干扰线
            DisturbLine();

            // 刷新其他干扰
            OtherDisturb();
        }

        /// <summary>
        /// 隐藏验证码
        /// </summary>
        public void HideCaptcha()
        {
//            GameObject.SetActive(false);
            _currentCaptcha.Clear();
        }

        /// <summary>
        /// 生成其他干扰元素
        /// </summary>
        private void OtherDisturb()
        {
            foreach (RectTransform item in _disturbArea)
            {
                item.GetComponent<Text>().text = ((char)RandomHelper.GetRandom().Next(33, 48)).ToString();
                RandomDisturbItem(item);
            }
        }

        /// <summary>
        /// 生成干扰线
        /// </summary>
        private void DisturbLine()
        {
            RandomDisturbLine(_disturbLine1);
            RandomDisturbLine(_disturbLine2);
            RandomDisturbLine(_disturbLine3);
        }

        /// <summary>
        /// 随机生成干扰线
        /// </summary>
        /// <param name="line"></param>
        private void RandomDisturbLine(RectTransform line)
        {
            var size = _disturbArea.sizeDelta / 2;
            var x = RandomHelper.GetRandom().Next((int)-size.x, (int)size.x);
            var y = RandomHelper.GetRandom().Next((int)-size.y, (int)size.y);

            line.localPosition = _disturbArea.localPosition + new Vector3(x, y, 0);
            line.localRotation = Quaternion.AngleAxis(RandomHelper.GetRandom().Next(0, 361), Vector3.forward);
            line.GetComponent<Text>().text = RandomLine('~', RandomHelper.GetRandom().Next(10, 21));
            line.GetComponent<Text>().color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 1f, 1f);
        }

        /// <summary>
        /// 获取指定长度的某字符串
        /// </summary>
        /// <param name="c"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string RandomLine(char c, int length)
        {
            var res = new char[length];
            while (length-- > 0)
            {
                res[length] = c;
            }

            return new string(res);
        }

        /// <summary>
        /// 刷新验证码
        /// </summary>
        private void RefreshCaptcha()
        {
            var captcha = GetRandomCaptcha();

            if (_currentCaptcha.Length != 0) _currentCaptcha.Clear();
            foreach (var c in captcha)
            {
                _currentCaptcha.Append(c);
            }

            _captchaItem1.GetComponent<Text>().text = captcha[0].ToString();
            _captchaItem2.GetComponent<Text>().text = captcha[1].ToString();
            _captchaItem3.GetComponent<Text>().text = captcha[2].ToString();
            _captchaItem4.GetComponent<Text>().text = captcha[3].ToString();

            RandomCaptchaStyle(_captchaItem1);
            RandomCaptchaStyle(_captchaItem2);
            RandomCaptchaStyle(_captchaItem3);
            RandomCaptchaStyle(_captchaItem4);
        }

        /// <summary>
        /// 随机调整位置和角度
        /// </summary>
        /// <param name="item"></param>
        private void RandomCaptchaStyle(RectTransform item)
        {
            var x = RandomHelper.GetRandom().Next(-5, 6);
            var y = RandomHelper.GetRandom().Next(-10, 11);

            item.anchoredPosition = new Vector2(x, y);
            item.localRotation = Quaternion.AngleAxis(RandomHelper.GetRandom().Next(-10, 31), Vector3.forward);
            item.localScale = Vector3.one * (RandomHelper.GetRandom().Next(90, 121) / 100f);
            item.GetComponent<Text>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 0.9f);
        }

        /// <summary>
        /// 随机干扰元素
        /// </summary>
        /// <param name="item"></param>
        private void RandomDisturbItem(RectTransform item)
        {
            var size = _disturbArea.sizeDelta / 2;
            var x = RandomHelper.GetRandom().Next((int)-size.x, (int)size.x);
            var y = RandomHelper.GetRandom().Next((int)-size.y, (int)size.y);

            item.anchoredPosition = new Vector2(x, y);
            item.localRotation = Quaternion.AngleAxis(RandomHelper.GetRandom().Next(0, 361), Vector3.forward);
            item.localScale = Vector3.one * (RandomHelper.GetRandom().Next(50, 80) / 100f);
            var color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.1f, 0.2f);
            color.a = RandomHelper.GetRandom().Next(30, 51) / 100f;
            item.GetComponent<Text>().color = color;
        }

        /// <summary>
        /// 获取随机验证码
        /// </summary>
        /// <returns></returns>
        private char[] GetRandomCaptcha()
        {
            var res = new char[4];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = (char)RandomHelper.GetRandom().Next(48, 58);
            }

            return res;
        }
    }
}