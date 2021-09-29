using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Loots
{
    class TopDamagersOnly : MobDrop
    {
        public TopDamagersOnly(int topDamage, params MobDrop[] children)
        {
            foreach (var child in children)
                child.Populate(
                        LootDefs, new LootDef.Builder()
                            .Item(null)
                            .Chance(-1)
                            .Threshold(-1f)
                            .Min(-1)
                            .RareMod(new LootDef.RarityModifiedData(-1f))
                            .MaxTop(topDamage)
                            .Build()
                    );
        }

        

    }
}
