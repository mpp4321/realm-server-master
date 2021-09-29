using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Worlds
{
    class SnakePit : World
    {
        public SnakePit(Map map, WorldDesc desc) : base(map, desc)
        {
        }

        private Cooldown bigCooldown = new Cooldown(30000, 0);
        private Cooldown smallCooldown = new Cooldown(10000, 0);

        private int bigCd = 0;
        private int smallCd = 0;

        public override void Tick()
        {
            base.Tick();
            if (bigCd <= 0)
            {
                bigCd = bigCooldown.Next(MathUtils.GetStaticRandom());
                foreach (var point in this.GetAllRegion(Region.Enemy2))
                {
                    if (MathUtils.Next(3) != 0) continue;
                    Enemy ent = new Enemy(Resources.Id2Object["Snake"].Type);
                    AddEntity(ent, point.ToVector2());
                }
            }

            if (smallCd <= 0)
            {
                smallCd = smallCooldown.Next(MathUtils.GetStaticRandom());
                foreach (var point in this.GetAllRegion(Region.Enemy1))
                {
                    if (MathUtils.Next(3) != 0) continue;
                    Enemy ent = new Enemy(Resources.Id2Object["Greater Pit Snake"].Type);
                    AddEntity(ent, point.ToVector2());
                }
            }

            bigCd -= Settings.MillisecondsPerTick;
            smallCd -= Settings.MillisecondsPerTick;
        }
    }
}
