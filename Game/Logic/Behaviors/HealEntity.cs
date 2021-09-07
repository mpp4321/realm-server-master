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
        private readonly int? _mpHealAmount;

        public HealEntity(float range, string name = null, int? healAmount = null, int? mpHealAmount = null, Cooldown coolDown = new Cooldown())
        {
            _range = range;
            _name = name;
            _coolDown = coolDown.Normalize();
            _amount = healAmount;
            _mpHealAmount = mpHealAmount;
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

                IEnumerable<Entity> entityGroup;
                
                if(_name?.Equals("Players") ?? false)
                {
                    entityGroup = GameUtils.GetNearbyPlayers(host, _range);
                } else
                {
                    entityGroup = GameUtils.GetNearbyEntities(host, _range).Where(
                        a => a.Desc.Group?.Equals(_name) ?? false || a.Desc.Id.Equals(_name)
                        ).OfType<Enemy>();
                }
                foreach (var entity in entityGroup)
                {

                    int newHp = entity.Desc.MaxHp;
                    int newMp = 0;
                    if(entity is Player)
                    {
                        newHp = (int)(entity as Player).SVs[StatType.MaxHp];
                        newMp = (int)(entity as Player).SVs[StatType.MaxMp];
                    }
                    if (_amount != null)
                    {
                        var newHealth = (int)_amount + entity.Hp;
                        if (newHp > newHealth)
                            newHp = newHealth;
                    }

                    if (_mpHealAmount != null)
                    {
                        var newMana = (int)_mpHealAmount + entity.Hp;
                        if (newMp > newMana)
                            newMp = newMana;
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

                    if(entity is Player)
                    {
                        var mpCu = (entity as Player).MP;
                        if (newMp != mpCu)
                        {
                            int n = newMp - mpCu;
                            (entity as Player).MP = newMp;
                            entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect(
                                ShowEffectIndex.Heal,
                                entity.Id,
                                0xff0000ff
                            ), entity.Position);
                            entity.Parent.BroadcastPacketNearby(GameServer.ShowEffect(
                                ShowEffectIndex.Line,
                                entity.Id,
                                0xff0000ff,
                                entity.Position
                            ), entity.Position);
                            entity.Parent.BroadcastPacketNearby(GameServer.Notification(
                                entity.Id,
                                "+" + n,
                                0xff0000ff
                            ), entity.Position);
                        }
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
