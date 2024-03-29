﻿using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Game.Worlds;
using RotMG.Game.Logic.ItemEffs;

namespace RotMG.Game.Entities
{
    public class Enemy : Entity
    {
        public Dictionary<Player, int> DamageStorage;
        public Terrain Terrain;

        public bool IsElite = false;

        public Enemy(ushort type) : base(type)
        {
            DamageStorage = new Dictionary<Player, int>();
        }

        public void ApplyPoison(Player hitter, ConditionEffectDesc[] effects, int damage, int damageLeft, uint color=0xffddff00)
        {
            if (HasConditionEffect(ConditionEffectIndex.Invincible) || 
                HasConditionEffect(ConditionEffectIndex.Stasis))
                return;

            var poison = GameServer.ShowEffect(ShowEffectIndex.Poison, Id, color);
            foreach (var j in Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                if (j is Player k && k.Client.Account.Effects)
                    k.Client.Send(poison);

            Damage(hitter, damage, effects, true, true);
            if (damageLeft <= 0) return;
            Manager.AddTimedAction(1000, () =>
            {
                damageLeft -= damage;
                if (damageLeft < 0)
                    damage = Math.Abs(damageLeft);

                if (hitter.Parent != null && Parent != null) //These have to be here in case enemy dies before poison is applied
                    ApplyPoison(hitter, effects, damage, damageLeft, color);
            });
        }

        public void Death(Player killer)
        {
#if DEBUG
            if (killer == null)
                throw new Exception("Undefined killer");
            //if (Dead == true)
            //    throw new Exception("Already dead");
#endif
#if RELEASE

            if (Dead == true)
                return;
#endif

            Parent?.EnemyKilled(this, killer);

            var baseExp = (int)Math.Ceiling(MaxHp / 10f) * Desc.XpMult;
            if (baseExp != 0)
            {
                List<Entity> l;
                foreach (var en in l = Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                {
                    if (!(en is Player player) || Desc.NoXp || IsSpawned) 
                        continue;
                    var exp = baseExp;
                    //if (exp > Player.GetNextLevelEXP(player.Level) / 10)
                    //   exp = Player.GetNextLevelEXP(player.Level) / 10;
                    if (player.GainEXP((int)exp))
                        foreach (var p in l)
                            if (!p?.Equals(player) ?? false)
                            {
                                if((p as Player)?.FameStats != null)
                                    (p as Player).FameStats.LevelUpAssists++;
                            } 
                                
                }
            }
            
            if (Behavior != null && Behavior.Loot.Count > 0)
                Behavior.Loot.Handle(this, killer);

            killer.FameStats.MonsterKills++;
            if (Desc.Cube) killer.FameStats.CubeKills++;
            if (Desc.Oryx) killer.FameStats.OryxKills++;
            if (Desc.God) killer.FameStats.GodKills++;

            if (Behavior != null)
            {
                foreach (var b in Behavior.Behaviors)
                    b.Death(this);
                foreach (var s in CurrentStates)
                    foreach (var b in s.Behaviors)
                        b.Death(this);
            }

            Dead = true;
            // If world was removed in the same tick as death
            Parent?.RemoveEntity(this);
        }

        public bool Damage(Player hitter, int damage, ConditionEffectDesc[] effects, bool pierces, bool showToHitter = false)
        {
#if DEBUG
            if (hitter == null)
                throw new Exception("Undefined hitter");
#endif
            if (effects == null)
                effects = new ConditionEffectDesc[] { };

            if (HasConditionEffect(ConditionEffectIndex.Invincible) || HasConditionEffect(ConditionEffectIndex.Stasis)
                || HasConditionEffect(ConditionEffectIndex.Invulnerable))
                return false;

            if (Dead || Desc.Friendly)
            {
                return false;
            }

            foreach (var eff in effects)
                ApplyConditionEffect(eff.Effect, eff.DurationMS, eff.Probability);

            if (HasConditionEffect(ConditionEffectIndex.ArmorBroken))
                pierces = true;

            var damageWithDefense = this.GetDefenseDamage(damage, Desc.Defense, pierces);

            if (HasConditionEffect(ConditionEffectIndex.Invulnerable))
                damageWithDefense = 0;

            Hp -= damageWithDefense;

            if (DamageStorage.ContainsKey(hitter))
                DamageStorage[hitter] += damageWithDefense;
            else DamageStorage.Add(hitter, damageWithDefense);

            hitter.FameStats.DamageDealt += damageWithDefense;

            var packet = GameServer.Damage(Id, new ConditionEffectIndex[0], damageWithDefense);
            foreach (var en in Parent?.PlayerChunks?.HitTest(Position, Player.SightRadius))
                if (en is Player player && player.Client.Account.AllyDamage && !player.Equals(hitter))
                    player.Client.Send(packet);

            if (showToHitter)
                hitter.Client.Send(packet);

            if (Hp <= 0 && !Dead)
            {
                Death(hitter);
                return true;
            }
            return false;
        }

        public override bool HitByProjectile(Projectile projectile)
        {
#if DEBUG 
            if (projectile.Owner == null || (!(projectile.Owner is Player) || projectile.Owner.PlayerOwner is Player))
            throw new Exception("Projectile owner is not player");
#endif

            // This goes here to avoid checking condition effects too much
            if (HasConditionEffect(ConditionEffectIndex.Invincible)
                || HasConditionEffect(ConditionEffectIndex.Stasis))
                return false;

            (projectile.Owner as Player).FameStats.ShotsThatDamage++;
            if(projectile.OnHitDelegate != null)
                projectile.OnHitDelegate(this);

            var beforeModifiedDamage = projectile.Damage;
            if(projectile.UniqueEffects != null)
            {
                foreach(var eff in projectile.UniqueEffects)
                    eff.OnEnemyHit(this, projectile, ref beforeModifiedDamage);
            }

            return Damage(projectile.Owner as Player, beforeModifiedDamage, projectile.Desc.Effects, projectile.Desc.ArmorPiercing);
        }

        public override void Dispose()
        {
            DamageStorage.Clear();
            base.Dispose();
        }

        public void MakeElite()
        {
            IsElite = true;
            MaxHp = (int)(MaxHp * 1.75f);
            Hp = MaxHp;
            Glow = 0xff0000;
            Size = (int) (Desc.Size * 1.25f);
        }

    }
}
