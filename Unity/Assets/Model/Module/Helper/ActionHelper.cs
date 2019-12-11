using System;
using UnityEngine.UI;

namespace ETModel
{
	public static class ActionHelper
	{
		public static void Add(this Button.ButtonClickedEvent buttonClickedEvent, Action action)
		{
			buttonClickedEvent.AddListener(() => { action(); });
		}

        public static void Add(this Slider.SliderEvent SliderValueChangedEvent, Action<float> action)
        {
            SliderValueChangedEvent.AddListener((float value) => action(value));
        }
	}
}