using RotMG.Common;
using RotMG.Game.Entities;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class WhiteDragonInAnOrb : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player p)
        {
            Enemy e = new Enemy(Resources.Id2Object["White Drake"].Type);
            e.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, 10000);
            p.Parent.AddEntity(e, position);
            e.PlayerOwner = p;
        }
    }
}
