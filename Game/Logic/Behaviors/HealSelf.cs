using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class HealSelf : Behavior
    {
        //State storage: cooldown timer

        private static Random Random = new Random();

        private Cooldown _coolDown;
        readonly int? _amount;

        public HealSelf(Cooldown cooldown = new Cooldown(), int? amount = null)
        {
            _coolDown = cooldown.Normalize();
            _amount = amount;
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
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return false;

                var entity = host;

                if (entity == null)
                    return false;

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
                    entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect
                    (
                        ShowEffectIndex.Heal,
                        entity.Id,
                        0xffffffff
                    ), entity.Position);
                    entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect
                    (
                        ShowEffectIndex.Flow,//Originally trail?
                        host.Id,
                        0xffffffff,
                        entity.Position
                    ), entity.Position);
                    entity.Parent.BroadcastPacketNearby(GameServer.Notification
                    (
                        entity.Id,
                        "+" + n,
                        0xff00ff00
                    ), entity.Position);
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
