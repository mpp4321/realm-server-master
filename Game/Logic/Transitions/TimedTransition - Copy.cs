﻿using RotMG.Common;
using RotMG.Utils;
using System;

namespace RotMG.Game.Logic.Transitions
{
    public class TimedRandomTransition : Transition
    {
        public static Random Random = new Random();
        public readonly int Time;
        public readonly string[] targets;

        public TimedRandomTransition(int time = 1000, params string[] targets) : base(targets[0])
        {
            Time = time;
            this.targets = targets;
        }

        public override void Enter(Entity host)
        {
            host.StateCooldown[Id]  = Time;
        }

        public override bool Tick(Entity host)
        {
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                host.StateCooldown[Id] = Time;
                StringTargetState = targets[ MathUtils.NextInt(0, targets.Length) ].ToLower();
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