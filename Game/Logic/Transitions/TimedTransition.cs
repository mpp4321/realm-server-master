using RotMG.Common;
using System;

namespace RotMG.Game.Logic.Transitions
{
    public class TimedTransition : Transition
    {
        public static Random Random = new Random();
        public readonly int Time;
        public readonly bool Randomized;

        public TimedTransition(string targetState, int time = 1000, bool randomized=false) : base(targetState)
        {
            Time = time;
            Randomized = randomized;
        }

        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = Time;
        }

        public override bool Tick(Entity host)
        {
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                host.StateCooldown[Id] = Randomized ? Random.Next(Time) : Time;
                return true;
            }
            return false;
        }

        public override void Exit(Entity host)
        { 
            host.StateCooldown.Remove(Id);
        }
    }
}
