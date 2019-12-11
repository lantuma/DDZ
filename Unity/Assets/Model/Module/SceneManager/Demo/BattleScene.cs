using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleScene:SceneBase
    {
        private SceneConfig sceneConfig;

        //基类有带参数的构造方法，所以必须传递实参给基类
        public BattleScene(SceneConfig config) : base(config)
        {
            this.sceneConfig = config;
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnEnd()
        {
            base.OnEnd();
        }
    }
}
