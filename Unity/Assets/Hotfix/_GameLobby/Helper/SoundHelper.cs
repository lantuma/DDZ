using System;
using System.Threading;
using ETModel;

namespace ETHotfix
{
    public static class SoundHelper
    {
        #region 渐入/渐出 播放人声

        private static float cacheBgSoundValue = 0.5f;

        private static string cacheSoundName = "";

        private static TimerComponent timer = ETModel.Game.Scene.GetComponent<TimerComponent>();

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();

        private static CancellationToken cancelToken;

        /// <summary>
        /// 打开UI面板时，压低背景音乐音量后，再播放人声.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="durationTime"></param>
        public static async void FadeInPlaySound(string soundName, int durationTime = 3000)
        {
            if (SoundComponent.Instance.SoundVolume == 0 && SoundComponent.Instance.SfxVolume == 0)
            {
                return;
            }

            FadeOutPlaySound();

            cancelToken = tokenSource.Token;

            cacheBgSoundValue = SoundComponent.Instance.SoundVolume;

            cacheSoundName = soundName;

            SoundComponent.Instance.SoundVolume = 0.2f;

            SoundComponent.Instance.PlayClip(soundName);

            await timer.WaitAsync(durationTime, cancelToken);

            SoundComponent.Instance.SoundVolume = cacheBgSoundValue;
        }

        /// <summary>
        /// 关闭UI面板时，恢复背景音乐音量并且关闭人声。
        /// </summary>
        public static void FadeOutPlaySound()
        {
            if (SoundComponent.Instance.SoundVolume == 0 && SoundComponent.Instance.SfxVolume == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(cacheSoundName)) { return; }

            tokenSource.Cancel();

            tokenSource = new CancellationTokenSource();

            cancelToken = tokenSource.Token;

            SoundComponent.Instance.Stop(cacheSoundName);

            SoundComponent.Instance.SoundVolume = cacheBgSoundValue;
        }

        #endregion

        #region 子游戏静音控制

        /// <summary>
        /// 缓存，用于退出子游戏时恢复音效
        /// </summary>
        private static float SubGamecacheBgSoundValue = 0.5f;

        private static float SubGamecacheFxSoundValue = 0.5f;

        /// <summary>
        /// 初始化音效按钮
        /// </summary>
        /// <param name="closeAction"></param>
        /// <param name="showAction"></param>
        public static void  InitSubGameSound(Action closeAction,Action showAction)
        {
            SubGamecacheBgSoundValue = SoundComponent.Instance.SoundVolume;

            SubGamecacheFxSoundValue = SoundComponent.Instance.SfxVolume;

            if (SoundComponent.Instance.SoundVolume > 0 || SoundComponent.Instance.SfxVolume > 0)
            {
                showAction();
            }
            else
            {
                closeAction();
            }
            
        }

        /// <summary>
        /// 点击音效按钮
        /// </summary>
        /// <param name="closeAction"></param>
        /// <param name="showAction"></param>
        public static void ToggleSubGameSound(Action closeAction, Action showAction)
        {
            if (SoundComponent.Instance.SoundVolume > 0 || SoundComponent.Instance.SfxVolume > 0)
            {
                SoundComponent.Instance.SoundVolume = 0f;

                SoundComponent.Instance.SfxVolume = 0f;

                closeAction();
            }
            else
            {
                SoundComponent.Instance.SoundVolume = SubGamecacheBgSoundValue == 0?1:SubGamecacheBgSoundValue;

                SoundComponent.Instance.SfxVolume = SubGamecacheFxSoundValue == 0 ? 1 : SubGamecacheFxSoundValue;

                showAction();
            }
        }

        /// <summary>
        /// 退出子游戏时，恢复至原来的声音大小
        /// </summary>
        public static void SubGameExitResetSound()
        {
            SoundComponent.Instance.SoundVolume = SubGamecacheBgSoundValue;

            SoundComponent.Instance.SfxVolume = SubGamecacheFxSoundValue;
        }

        #endregion
    }
}
