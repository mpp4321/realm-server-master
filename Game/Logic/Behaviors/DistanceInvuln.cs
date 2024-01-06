using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class DistanceInvuln : Behavior
    {

        private float Distance { get; set; }

        public DistanceInvuln(float distance)
        {
            Distance = distance;
        }

        public override void Enter(Entity host)
        {
        }

        public override bool Tick(Entity host)
        {
            if(host.GetNearbyPlayers(Distance).Count() == 0)
            {
                host.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 200);
                return true;
            }
            return false;
        }
    }
}
