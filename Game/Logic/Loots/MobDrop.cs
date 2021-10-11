using System.Collections.Generic;
using System.Linq;
using RotMG.Common;

namespace RotMG.Game.Logic.Loots
{
    public class MobDrop : IBehavior
    {
        protected readonly List<LootDef> LootDefs = new List<LootDef>();

        public MobDrop(params LootDef[] defs)
        {
            LootDefs = defs.ToList();
        }

        public virtual void Populate(IList<LootDef> lootDefs, LootDef overrides = null)
        {
            if (overrides == null)
            {
                foreach (var lootDef in LootDefs)
                    lootDefs.Add(lootDef);
                return;
            }

            foreach (var lootDef in LootDefs)
            {
                lootDefs.Add(new LootDef(
                    Resources.Type2Item[lootDef.Item].Id, 
                    overrides.Chance >= 0 ? overrides.Chance : lootDef.Chance,
                    overrides.Threshold >= 0 ? overrides.Threshold : lootDef.Threshold,
                    overrides.Min >= 0 ? overrides.Min : lootDef.Min, overrides.RareData.RarityMod >= 0 ? overrides.RareData : lootDef.RareData,
                    overrides.MaxTop != 999 ? overrides.MaxTop : lootDef.MaxTop,
                    overrides.overrideData != null ? overrides.overrideData : lootDef.overrideData));
            }
        }
    }
}