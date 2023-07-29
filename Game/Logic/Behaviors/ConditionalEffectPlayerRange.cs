using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class ConditionalEffectPlayerRange : Behavior
    {
        ConditionEffectIndex effect;
        int duration;
        Cooldown cd;
        int cdtime;
        int requiredRange;

        public ConditionalEffectPlayerRange(ConditionEffectIndex effect, int requiredRange, int duration = 1000)
        {
            this.effect = effect;
            this.duration = duration;
            this.requiredRange = requiredRange;
            this.cd = new Cooldown(duration, 0);
        }

        public override void Enter(Entity host)
        {
            if(GameUtils.GetNearbyPlayers(host, requiredRange).Count() == 0)
            {
                host.ApplyConditionEffect(
                    effect,
                    duration
                );
                this.cdtime = this.cd.NextNoVariance();
            }
        }

        public override bool Tick(Entity host)
        {
            this.cdtime -= Settings.MillisecondsPerTick;

            if (this.cdtime > 0) return false;

            if(GameUtils.GetNearbyPlayers(host, requiredRange).Count() == 0)
            {
                host.ApplyConditionEffect(effect, duration);
                this.cdtime = cd.NextNoVariance();
                return true;
            }
            return false;
        }

        public override void Exit(Entity host)
        {
            host.ApplyConditionEffect(effect, 0);
        }

    }
}
