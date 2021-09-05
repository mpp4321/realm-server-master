using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Buzz : Behavior
    {
        private static Random _Random = new Random();

        //State storage: direction & remain
        class BuzzStorage
        {
            public Vector2 Direction;
            public float RemainingDistance;
            public int RemainingTime;
        }


        float speed;
        float dist;
        Cooldown coolDown;

        public Buzz(double speed = 2, double dist = 0.5, Cooldown coolDown = new Cooldown())
        {
            this.speed = (float)speed;
            this.dist = (float)dist;
            this.coolDown = coolDown.Normalize(1);
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new BuzzStorage();
        }

        public override bool Tick(Entity host)
        {
            BuzzStorage storage = (BuzzStorage) host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if (storage.RemainingTime > 0)
            {
                storage.RemainingTime -= Settings.MillisecondsPerTick;
            }
            else
            {
                if (storage.RemainingDistance <= 0)
                {
                    do
                    {
                        storage.Direction = new Vector2(_Random.Next(-1, 2), _Random.Next(-1, 2));
                    } while (storage.Direction.X == 0 && storage.Direction.Y == 0);
                    storage.Direction.Normalize();
                    storage.RemainingDistance = this.dist;
                }
                float dist = host.GetSpeed(speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(storage.Direction * dist + host.Position);

                storage.RemainingDistance -= dist;
            }

            host.StateObject[Id] = storage;
            return true;
        }
    }
}
