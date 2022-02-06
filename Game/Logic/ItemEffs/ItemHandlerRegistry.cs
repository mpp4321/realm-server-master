using RotMG.Game.Logic.ItemEffs.ItemDB;
using RotMG.Game.Logic.ItemEffs.RuneEffects;
using RotMG.Game.Logic.ItemEffs.SetEffects;
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
            Registry.Add("UniversalPower", new UniversalPower());
            Registry.Add("AOBL", new AmuletOfBackwardsLuck());
            Registry.Add("SappingShot", new SappingShot());
            Registry.Add("Lifeless", new Lifeless());
            Registry.Add("WillOfDen", new WillOfDen());
            Registry.Add("Conflict", new OrbOfConflict());
            Registry.Add("ArachnidGarment", new ArachGar());
            Registry.Add("CryptRing", new CryptRing());
            Registry.Add("MiasmaPoison", new MiasmaPoison());
            Registry.Add("SpriteRing", new SpriteRing());
            Registry.Add("GLizard", new TheGreatLizard());
            Registry.Add("RulerDominion", new RulerDominion());

            //Set effs
            Registry.Add("Phylac", new Phylac());

            //components
            Registry.Add("FireRune", new FireRune());

            //Runes
            Registry.Add("Percise", new Percise());
            Registry.Add("Vampirism", new Vampirism());
            Registry.Add("Brute", new Brute());
            Registry.Add("Mage", new Mage());
            Registry.Add("Elven", new Elven());
            Registry.Add("Juggernaut", new Juggernaut());
        }

    }
}
