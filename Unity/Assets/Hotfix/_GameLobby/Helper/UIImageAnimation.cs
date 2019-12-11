namespace ETHotfix
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;
    using ETModel;


    [ObjectSystem]
    public class UIImageAnimationUpdateSystem : UpdateSystem<UIImageAnimation>
    {
        public override void Update(UIImageAnimation self)
        {
            self.Update();
        }
    }

    public class UIImageAnimation : Component
    {
        /// <summary>
        /// 播放帧率
        /// </summary>
        private int _FPS = 25;
        /// <summary>
        /// 是否循环
        /// </summary>
        private bool _Loop = false;
        /// <summary>
        /// 是否可以播放
        /// </summary>
        private bool _IsCanPlay = false;
        /// <summary>
        /// 数量
        /// </summary>
        private int _ImageCount;
        /// <summary>
        /// 当前播放动画的的Image
        /// </summary>
        private Image _CurrentImage;
        /// <summary>
        /// 当前索引
        /// </summary>
        private int _CurrentIndex = 0;

        private float _DeltaTime = 0f;
        private string _SpriteName;
        /// <summary>
        /// 图集名
        /// </summary>
        private string _AtlasName;

        public void SetGameObject(GameObject go)
        {
            this.GameObject = go;
            _CurrentImage= go.Get<Image>();
        }

        /// <summary>
        /// 开始播放序列帧特效 
        /// </summary>
        /// <param name="isLoop">If set to <c>true</c> is loop.</param>
        public void PlayAnimation(string atlasName, string spriteName, int spriteCount, int fps = 25, bool isLoop = false)
        {
            _AtlasName = atlasName;
            _ImageCount = spriteCount;
            _SpriteName = spriteName;
            _Loop = isLoop;
            //_CurrentImage = this.GameObject.Get<Image>();
            _CurrentImage.SetNativeSize();
            _IsCanPlay = true;
            _CurrentIndex = 0;
            _FPS = fps;
            _CurrentImage.sprite = GameHelper.GetSprite(_AtlasName, string.Format("{0}0{1}", _SpriteName, 0));
        }

        public void Update()
        {
            if (_IsCanPlay == true)
            {
                _DeltaTime += Time.deltaTime;
                float rate = 1f / _FPS;
                if (rate < _DeltaTime)
                {
                    if (rate > 0f)
                    {
                        _DeltaTime = _DeltaTime - rate;
                    }
                    else
                    {
                        _DeltaTime = 0f;
                    }
                    _CurrentIndex++;
                    if (_CurrentIndex >= _ImageCount)
                    {
                        _IsCanPlay = _Loop;
                        if (_IsCanPlay == false)
                            return;
                        _CurrentIndex = 0;
                    }
                    if (_CurrentIndex > 8)
                    {
                        _CurrentImage.sprite = GameHelper.GetSprite(_AtlasName, string.Format("{0}{1}", _SpriteName, _CurrentIndex + 1));
                    }
                    else
                    {
                        _CurrentImage.sprite = GameHelper.GetSprite(_AtlasName, string.Format("{0}0{1}", _SpriteName, _CurrentIndex + 1));
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.GameObject.SetActive(false);
        }

    }
}