using static RotMG.Game.Logic.LootDef;

namespace RotMG.Game.Logic.Loots
{
    public class ItemLoot : MobDrop
    {
        public ItemLoot(string item, float chance = 1, float threshold = 0, int min = 0, RarityModifiedData r=null)
        {
            LootDefs.Add(new LootDef(
                item, chance, threshold, min, r));
        }
    }
}
