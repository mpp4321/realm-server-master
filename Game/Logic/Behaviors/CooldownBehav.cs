using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    public class CooldownBehav : Behavior
    {

        private Cooldown Cooldown { get; set; }
        private Behavior Behavior { get; set; }

        public CooldownBehav(Cooldown cooldown, Behavior behav)
        {
            Cooldown = cooldown;
            Behavior = behav;
        }


        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = Cooldown.Next(MathUtils.GetStaticRandom());
            Behavior?.Enter(host);
        }

        public override bool Tick(Entity host)
        {
            Behavior?.Tick(host);
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            return host.StateCooldown[Id] <= 0;
        }

        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
            Behavior?.Exit(host);
        }

    }
}
