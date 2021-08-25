namespace RotMG.Game.Logic.Loots
{
    public class ItemLoot : MobDrop
    {
        public ItemLoot(string item, float chance = 1, float threshold = 0, int min = 0)
        {
            LootDefs.Add(new LootDef(
                item, chance, threshold, min));
        }
    }
}
