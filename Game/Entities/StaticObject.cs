using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;

namespace RotMG.Game.Entities
{
    public class StaticObject : Entity
    {
        public StaticObject(ushort type) : base(type)
        {

        }

        public override bool HitByProjectile(Projectile projectile)
        {
#if DEBUG
            if (projectile.Owner == null || !(projectile.Owner is Player))
                throw new Exception("Projectile owner is undefined");
#endif

            if (Desc.Enemy)
            {
                var damageWithDefense = this.GetDefenseDamage(projectile.Damage, Desc.Defense, projectile.Desc.ArmorPiercing);
                Hp -= damageWithDefense;

                var owner = projectile.Owner as Player;
                owner.FameStats.DamageDealt += damageWithDefense;
                owner.FameStats.ShotsThatDamage++; 
                
                var packet = GameServer.Damage(Id, new ConditionEffectIndex[0], damageWithDefense);
                foreach (var en in Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                    if (en is Player player && player.Client.Account.AllyDamage && !player.Equals(owner))
                        player.Client.Send(packet);

                if (Hp <= 0)
                {
                    Dead = true;
                    Parent.RemoveStatic((int)Position.X, (int)Position.Y);
                    return true;
                }
            }
            return false;
        }

        public override ObjectDefinition GetObjectDefinition()
        {
            var tile = Parent.GetTileF(Position.X, Position.Y);
            AddSVs(tile.Key);
            return base.GetObjectDefinition();
        }

        private void AddSVs(string objKey)
        {
            if (string.IsNullOrEmpty(objKey))
                return;

            foreach (var item in objKey.Split(';'))
            {
                var config = item.Split(':');
                var value = config[1];
                switch (config[0])
                {
                    case "hp":
                        var hp = Convert.ToInt32(value);
                        TrySetSV(StatType.Hp, hp);
                        TrySetSV(StatType.MaxHp, hp);
                        continue;
                    case "name":
                        TrySetSV(StatType.Name, value);
                        continue;
                    case "size":
                        TrySetSV(StatType.Size, Convert.ToInt32(value));
                        continue;
                }
            }
        }
    }
}
