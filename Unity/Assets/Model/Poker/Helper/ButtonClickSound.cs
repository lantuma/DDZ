using UnityEngine;

namespace ETModel
{
    public static class ButtonClickSound
    {
        public static void RegisterPlayButtonSound()
        {
            ButtonAnimationEffect.ButtonPointerUpAction -= PlayClickSound;

            ButtonAnimationEffect.ButtonPointerUpAction += PlayClickSound;
        }

        private static void PlayClickSound()
        {
            SoundComponent.Instance?.PlayClip("soundclick");
        }
    }
}
