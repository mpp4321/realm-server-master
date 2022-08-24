using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;

namespace RotMG.Utils
{
    public static class GameUtils
    {
        public static int GetDefenseDamage(this Entity entity, int damage, int defense, bool pierces, int protection=95)
        {
#if DEBUG
            if (entity == null)
                throw new Exception("Undefined entity");
#endif
            if (pierces)
                defense = 0;

            if (entity.HasConditionEffect(ConditionEffectIndex.Armored))
                defense *= 2;

            var cursedMod = 1.0f;
            if (entity.HasConditionEffect(ConditionEffectIndex.CursedHigher))
            {
                cursedMod = 1.75f;
            } else if (entity.HasConditionEffect(ConditionEffectIndex.CursedLower))
            {
                cursedMod = 1.25f;
            }
            damage = (int)(damage * cursedMod);

            var min = (int)(damage * (1.0f - (protection / 100.0f)));
            var d = Math.Max(min, damage - defense);
            return d;
        }

        public static Entity GetNearestSmart(this Entity entity, float radius, bool targetPlayers, Func<Entity, bool> pred = null)
        {
            return targetPlayers ? GetNearestPlayer(entity, radius, pred) : GetNearestEnemy(entity, radius);
        }

        public static Entity GetNearestEntity(this Entity entity, float radius)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static IEnumerable<Entity> GetNearbyEntities(this Entity entity, float radius)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            return entity.Parent.EntityChunks.HitTest(entity.Position, radius);
        }

        public static IEnumerable<Entity> GetNearbyPlayers(this Entity entity, float radius)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif
            return entity.Parent.PlayerChunks.HitTest(entity.Position, radius);
        }




        public static Entity GetNearestEntity(this Entity entity, float radius, ushort objId)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (en.GetObjectDefinition().ObjectType != objId) continue;
                if (entity == en) continue;
                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEntity(this Entity entity, float radius, float angle, float cone)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy) || en.Desc.Friendly)
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible) ||
                    en.HasConditionEffect(ConditionEffectIndex.Stasis))
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, float angle, float cone, Vector2 target)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible) ||
                    en.HasConditionEffect(ConditionEffectIndex.Stasis))
                    continue;

                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                float d;
                if ((d = target.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, float angle, float cone, Vector2 target, HashSet<Entity> exclude)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
                throw new Exception();
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible) ||
                    en.HasConditionEffect(ConditionEffectIndex.Stasis))
                    continue;

                if (Math.Abs(angle - MathF.Atan2(en.Position.Y - entity.Position.Y, en.Position.X - entity.Position.X)) > cone)
                    continue;

                if (exclude.Contains(en))
                    continue;

                float d;
                if ((d = target.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Entity GetNearestEnemy(this Entity entity, float radius, HashSet<Entity> exclude)
        {
#if DEBUG
            if (entity == null || entity.Parent == null || radius <= 0)
            {
                throw new Exception();
            }
#endif

            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent.EntityChunks.HitTest(entity.Position, radius))
            {
                if (!(en is Enemy))
                    continue;

                if (en.HasConditionEffect(ConditionEffectIndex.Invincible) ||
                    en.HasConditionEffect(ConditionEffectIndex.Stasis))
                    continue;

                if (exclude.Contains(en))
                    continue;

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }

        public static Vector2 RandomPointWithin(Vector2 point, float radius)
        {
            var dir1 = MathUtils.PlusMinus();
            var dir2 = MathUtils.PlusMinus();
            return point + new Vector2(MathUtils.NextFloat(0, radius) * dir1, MathUtils.NextFloat(0, radius) * dir2);
        }

        public static Entity GetNearestPlayer(this Entity entity, float radius, Func<Entity, bool> pred = null)
        {
#if DEBUG
            if (entity == null || entity.Parent == null)
                throw new Exception();
#endif
            if (entity == null) return null;
            if (radius <= 0) return null;
            Entity nearest = null;
            var dist = float.MaxValue;
            foreach (var en in entity.Parent?.PlayerChunks?.HitTest(entity.Position, radius))
            {
                if (en == null) continue;
                if (en.HasConditionEffect(ConditionEffectIndex.Invisible))
                    continue;

                if(pred != null && !pred(en))
                {
                    continue;
                }

                float d;
                if ((d = entity.Position.Distance(en.Position)) < dist)
                {
                    nearest = en;
                    dist = d;
                }
            }
            return nearest;
        }
        public static void ShowEffectRange(Player sender, World Parent, Vector2 Position, float range, byte[] bytes)
        {
            foreach (var j in Parent.PlayerChunks.HitTest(Position, range))
            {
                if (j is Player k)
                {
                    if (k.Client.Account.Effects || k.Equals(sender))
                        k.Client.Send(bytes);
                }
            }
        }
    }

}
