namespace RotMG.Game.Logic.Loots
{
    public class Threshold : MobDrop
    {
        public Threshold(float threshold, params MobDrop[] children)
        {
            foreach (var child in children)
                child.Populate(LootDefs, new LootDef(null, -1, threshold, -1));
        }
    }
}