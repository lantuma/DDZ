﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class PlayerSystem : AwakeSystem<Player, long>
    {
        public override void Awake(Player self, long a)
        {
            self.Awake(a);
        }
    }

    public sealed class Player: Entity
    {
        public long UnitId { get; set; }

        public void Awake(long accountId)
        {
            this.UnitId = accountId;
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
