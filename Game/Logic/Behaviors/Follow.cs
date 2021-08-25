using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Follow : Behavior
    {
        //State storage: follow state

        private static Random _Random = new Random();

        class FollowState
        {
            public F State;
            public int RemainingTime;
        }

        enum F
        {
            DontKnowWhere,
            Acquired,
            Resting
        }

        float speed;
        float acquireRange;
        float range;
        int duration;
        Cooldown coolDown;

        public Follow(float speed, float acquireRange = 10, float range = 6,
            int duration = 0, Cooldown coolDown = new Cooldown())
        {
            this.speed = speed;
            this.acquireRange = acquireRange;
            this.range = range;
            this.duration = duration;
            this.coolDown = coolDown.Normalize(duration == 0 ? 0 : 1000);
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }

        public override bool Tick(Entity host)
        {
            var state = host.StateObject[Id];
            FollowState s;
            if (state == null) s = new FollowState();
            else s = (FollowState)state;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var player = GameUtils.GetNearestPlayer(host, acquireRange);

            Vector2 vect;
            switch (s.State)
            {
                case F.DontKnowWhere:
                    if (player != null && s.RemainingTime <= 0)
                    {
                        s.State = F.Acquired;
                        if (duration > 0)
                            s.RemainingTime = duration;
                        goto case F.Acquired;
                    }
                    else if (s.RemainingTime > 0)
                        s.RemainingTime -= Settings.MillisecondsPerTick;
                    break;
                case F.Acquired:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = 0;
                        break;
                    }
                    else if (s.RemainingTime <= 0 && duration > 0)
                    {
                        s.State = F.DontKnowWhere;
                        s.RemainingTime = coolDown.Next(_Random);
                        break;
                    }
                    if (s.RemainingTime > 0)
                        s.RemainingTime -= Settings.MillisecondsPerTick;

                    vect = player.Position - host.Position;
                    if (vect.Length() > range)
                    {
                        vect.X -= _Random.Next(-2, 2) / 2f;
                        vect.Y -= _Random.Next(-2, 2) / 2f;
                        vect.Normalize();
                        float dist = host.GetSpeed(speed) * (Settings.MillisecondsPerTick / 1000f);
                        host.ValidateAndMove(vect * dist + host.Position);
                    }
                    else
                    {
                        s.State = F.Resting;
                        s.RemainingTime = 0;
                    }
                    break;
                case F.Resting:
                    if (player == null)
                    {
                        s.State = F.DontKnowWhere;
                        if (duration > 0)
                            s.RemainingTime = duration;
                        break;
                    }
                    vect = player.Position - host.Position;
                    if (vect.Length() > range + 1)
                    {
                        s.State = F.Acquired;
                        s.RemainingTime = duration;
                        goto case F.Acquired;
                    }
                    break;

            }

            host.StateObject[Id] = s;
            return true;
        }
    }
}
