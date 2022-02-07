using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs
{
    public interface IItemHandler
    {

        public virtual void OnItemEquip(Player p, int slot) { }

        public virtual void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) { }

        public virtual void OnProjectileShoot(Player shotFrom, ref Projectile projectile) { }

        public virtual void OnHitByEnemy(Player hit, Entity hitBy, Projectile by) { }

        public virtual void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player) { }

        public virtual void OnTick(Player p) { }

        public virtual void ModifyDrop(Player p, LootDef def, ref Dictionary<Player, int> thresholds, ref float dropMod)
        {
        }

        public virtual void ModifyDroppedItemData(Player p, ref ItemDataJson json) { }

    }
}
