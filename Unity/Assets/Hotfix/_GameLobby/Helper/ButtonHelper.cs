using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class ButtonHelper
    {
        public static Button RegisterButtonEvent(ReferenceCollector rc, string  name,UnityAction call)
        {
            var btn = rc.Get<GameObject>(name).Get<Button>();

            btn.onClick.AddListener(call);

            return btn;
        }
    }
}
