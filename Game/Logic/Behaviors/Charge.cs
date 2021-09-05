using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Charge : Behavior
    {

        private static Random Random = new Random();

        //State storage: charge host.StateObject[Id]
        public class ChargeState
        {
            public Vector2 Direction;
            public int RemainingTime;
            public Player from;
        }

        private readonly float _speed;
        private readonly float _range;
        private Cooldown _coolDown;
        private readonly bool _targetPlayers;
        private readonly Action<Entity, Entity, ChargeState> _callB;

        public Charge(double speed = 4, float range = 10, Cooldown coolDown = new Cooldown(), bool targetPlayers = true,
            Action<Entity, Entity, ChargeState> callback = null
        )
        {
            _speed = (float)speed;
            _range = range;
            _coolDown = coolDown.Normalize(2000);
            _targetPlayers = targetPlayers;
            _callB = callback;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }

        public override bool Tick(Entity host)
        {
            var s = (host.StateObject[Id] == null) ?
                new ChargeState() :
                (ChargeState)host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if (s.RemainingTime <= 0)
            {
                if (s.Direction == new Vector2(0, 0))
                {
                    Entity player = GameUtils.GetNearestSmart(host, _range, _targetPlayers);
                    if (player != null && player.Position != host.Position)
                    {
                        s.Direction = player.Position - host.Position;
                        var d = s.Direction.Length();
                        if (d < 1)
                        {
                            //s.from = host.Get();
                            if (_callB != null)
                                _callB(host, player, s);
                            //Cheaty way of later setting s.RemainingTime to 0
                            d = 0;
                        }
                        s.Direction.Normalize();
                        //s.RemainingTime = _coolDown.Next(Random);
                        //if (d / host.GetSpeed(_speed) < s.RemainingTime)
                        s.RemainingTime = (int)(d / host.GetSpeed(_speed) * 1000);
                    }
                }
                else
                {
                    s.Direction = new Vector2(0,0);
                    s.RemainingTime = _coolDown.Next(Random);
                }
            }

            if (s.Direction != Vector2.Zero)
            {
                float dist = host.GetSpeed(_speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(host.Position + dist * s.Direction);
            }

            s.RemainingTime -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = s;
            return true;
        }

    }
}
