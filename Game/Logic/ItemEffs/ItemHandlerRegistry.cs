using RotMG.Game.Logic.ItemEffs.ItemDB;
using RotMG.Game.Logic.Mechanics.Database;
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
            //Item effs
            Registry.Add("PoisonDagger", new PoisonousDagger());
            Registry.Add("CuriousEyeball", new CuriousEyeball());
            Registry.Add("SpectralCloth", new SpectralCloth());
            Registry.Add("Crumbling", new Crumbling());
            Registry.Add("MedusaGarment", new OutOfCombatSpeed());
            Registry.Add("BrainOrb", new BrainOrb());
            Registry.Add("PoisonFire", new PoisonFire());
            Registry.Add("Electricity", new Electricity());
            Registry.Add("BloodSuck", new BloodSuckingAmulet());
            Registry.Add("ElectricSnake", new ElectricSnake());
            Registry.Add("SnakePitArmor", new SnakePitArmor());
            Registry.Add("OphGem", new OphGem());

            //components
            Registry.Add("FireRune", new FireRune());
        }

    }
}
