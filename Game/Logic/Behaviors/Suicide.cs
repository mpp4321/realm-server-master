using RotMG.Game.Entities;
using RotMG.Utils;
using System;

namespace RotMG.Game.Logic.Behaviors
{
    class Suicide : Behavior
    {
        public Suicide()
        {
        }

        public override bool Tick(Entity host)
        {
#if DEBUG
            if (!(host is Enemy))
                throw new NotSupportedException("Use Decay instead");
#endif
            var player = GameUtils.GetNearestPlayer(host, 16) as Player;
            if (player == null) return false;
            (host as Enemy).Death(player);
            return true;
        }
    }
}
