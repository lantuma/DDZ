using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    public class UITipCptAwakeSystem : AwakeSystem<UITipCpt,object>
    {
        public override void Awake(UITipCpt self, object arg)
        {
            self.Awake(arg);
        }
    }

    public class UITipCpt : Component
    {
        private ReferenceCollector rc;

        private Text Text;

        private string message = "";

        public void Awake(object arg)
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            Text = rc.Get<GameObject>("Text").GetComponent<Text>();

            if (arg != null)
            {
                message = arg as string;

                Text.text = message;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

        }
    }
}
