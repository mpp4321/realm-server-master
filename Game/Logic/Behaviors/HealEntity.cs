using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class HealEntity : Behavior
    {
        private static Random Random = new Random();

        //State storage: cooldown timer

        private readonly float _range;
        private readonly string _name;
        private Cooldown _coolDown;
        private readonly int? _amount;

        public HealEntity(float range, string name = null, int? healAmount = null, Cooldown coolDown = new Cooldown())
        {
            _range = range;
            _name = name;
            _coolDown = coolDown.Normalize();
            _amount = healAmount;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = 0;
        }

        public override bool Tick(Entity host)
        {
            var cool = (int)host.StateObject[Id];

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned)) return false;

                foreach (var entity in GameUtils.GetNearbyEntities(host, _range).Where(
                    a => a.Desc.Group?.Equals(_name) ?? false || a.Desc.Id.Equals(_name)
                ).OfType<Enemy>())
                {
                    int newHp = entity.Desc.MaxHp;
                    if (_amount != null)
                    {
                        var newHealth = (int)_amount + entity.Hp;
                        if (newHp > newHealth)
                            newHp = newHealth;
                    }
                    if (newHp != entity.Hp)
                    {
                        int n = newHp - entity.Hp;
                        entity.Hp = newHp;
                        entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect(
                            ShowEffectIndex.Heal,
                            entity.Id,
                            0xffffffff
                        ), entity.Position);
                        entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect(
                            ShowEffectIndex.Line,
                            entity.Id,
                            0xffffffff,
                            entity.Position
                        ), entity.Position);
                        entity.Parent.BroadcastPacketNearby(GameServer.Notification(
                            entity.Id,
                            "+" + n,
                            0xff00ff00
                        ), entity.Position);
                    }
                }
                cool = _coolDown.Next(Random);
            }
            else
                cool -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = cool;
            return true;
        }

    }
}
