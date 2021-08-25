using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Game.Worlds;

namespace RotMG.Game.Entities
{
    public class Enemy : Entity
    {
        public Dictionary<Player, int> DamageStorage;
        public Terrain Terrain;

        public Enemy(ushort type) : base(type)
        {
            DamageStorage = new Dictionary<Player, int>();
        }

        public void ApplyPoison(Player hitter, ConditionEffectDesc[] effects, int damage, int damageLeft)
        {
            if (HasConditionEffect(ConditionEffectIndex.Invincible) || 
                HasConditionEffect(ConditionEffectIndex.Stasis))
                return;

            var poison = GameServer.ShowEffect(ShowEffectIndex.Poison, Id, 0xffddff00);
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
                    ApplyPoison(hitter, effects, damage, damageLeft);
            });
        }

        public void Death(Player killer)
        {
#if DEBUG
            if (killer == null)
                throw new Exception("Undefined killer");
#endif

            if (Parent is Realm realm)
                realm.EnemyKilled(this, killer);

            var baseExp = (int)Math.Ceiling(MaxHp / 10f);
            if (baseExp != 0)
            {
                List<Entity> l;
                foreach (var en in l = Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                {
                    if (!(en is Player player)) 
                        continue;
                    var exp = baseExp;
                    if (exp > Player.GetNextLevelEXP(player.Level) / 10)
                        exp = Player.GetNextLevelEXP(player.Level) / 10;
                    if (player.GainEXP(exp))
                        foreach (var p in l)
                            if (!p.Equals(player)) 
                                (p as Player).FameStats.LevelUpAssists++;
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
            Parent.RemoveEntity(this);
        }

        public bool Damage(Player hitter, int damage, ConditionEffectDesc[] effects, bool pierces, bool showToHitter = false)
        {
#if DEBUG
            if (HasConditionEffect(ConditionEffectIndex.Invincible))
                throw new Exception("Entity should not be damaged if invincible");
            if (HasConditionEffect(ConditionEffectIndex.Stasis))
                throw new Exception("Entity should not be damaged if stasised");
            if (effects == null)
                throw new Exception("Null effects");
            if (hitter == null)
                throw new Exception("Undefined hitter");
#endif


            foreach (var eff in effects)
                ApplyConditionEffect(eff.Effect, eff.DurationMS);

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
            foreach (var en in Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                if (en is Player player && player.Client.Account.AllyDamage && !player.Equals(hitter))
                    player.Client.Send(packet);

            if (showToHitter)
                hitter.Client.Send(packet);

            if (Hp <= 0)
            {
                Death(hitter);
                return true;
            }
            return false;
        }

        public override bool HitByProjectile(Projectile projectile)
        {
#if DEBUG
            if (projectile.Owner == null || !(projectile.Owner is Player))
                throw new Exception("Projectile owner is not player");
#endif
            (projectile.Owner as Player).FameStats.ShotsThatDamage++;
            return Damage(projectile.Owner as Player, projectile.Damage, projectile.Desc.Effects, projectile.Desc.ArmorPiercing);
        }

        public override void Dispose()
        {
            DamageStorage.Clear();
            base.Dispose();
        }
    }
}
