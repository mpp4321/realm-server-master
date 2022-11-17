using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;

namespace RotMG.Game.Logic.Behaviors
{
    class Suicide : Behavior
    {
        int time;
        public Suicide(int time = 0)
        {
            this.time = time;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = time;
        }

        public override bool Tick(Entity host)
        {
            int cool = (int) host.StateObject[Id];

            cool -= Settings.MillisecondsPerTick;

            if(cool <= 0)
            {
    #if DEBUG
                if (!(host is Enemy))
                    throw new NotSupportedException("Use Decay instead");
    #endif
                var player = GameUtils.GetNearestPlayer(host, 16) as Player;
                if (player == null) return false;
                Manager.StartOfTickAction(() =>
                {
                    (host as Enemy).Death(player);
                });
                return true;
            }

            host.StateObject[Id] = cool;
            return false;
        }
    }
}
