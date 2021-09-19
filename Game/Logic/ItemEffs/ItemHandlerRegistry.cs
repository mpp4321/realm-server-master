using RotMG.Game.Logic.ItemEffs.ItemDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs
{
    class ItemHandlerRegistry
    {

        public static Dictionary<string, IItemHandler> Registry = new Dictionary<string, IItemHandler>();

        static ItemHandlerRegistry()
        {
            Registry.Add("PoisonDagger", new PoisonousDagger());
            Registry.Add("CuriousEyeball", new CuriousEyeball());
            Registry.Add("SpectralCloth", new SpectralCloth());
            Registry.Add("Crumbling", new Crumbling());
            Registry.Add("MedusaGarment", new OutOfCombatSpeed());
            Registry.Add("BrainOrb", new BrainOrb());
            Registry.Add("PoisonFire", new PoisonFire());
        }

    }
}
