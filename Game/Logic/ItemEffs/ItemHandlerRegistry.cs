using RotMG.Common;
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
        public static Dictionary<string, int> RuneFameCosts = new Dictionary<string, int>();
        public static Dictionary<string, int> RuneHandlerToItemId = new Dictionary<string, int>();

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
            Registry.Add("JackpotItem", new JackpotItem());
            Registry.Add("ShaitTome", new ShaitTome());
            Registry.Add("GoldenTome", new GoldenTome());
            Registry.Add("Immunity", new Immunity());
            Registry.Add("MagmaQuiver", new MagmaQuiver());
            Registry.Add("DragonRage", new DragonRage());
            Registry.Add("Thunderbolts", new Thunderbolts());
            Registry.Add("OutOfCombatRegen", new OutOfCombatRegen());
            Registry.Add("GhostlyAura", new GhostlyAura());
            Registry.Add("Bloodlust", new Bloodlust());
            Registry.Add("SorReactor", new SorReactor());
            Registry.Add("ObsidianPlatemail", new ObsidianPlatemail());
            Registry.Add("PaladinRecovery", new PaladinRecovery());
            Registry.Add("CosmicCloak", new CosmicCloak());
            Registry.Add("PredatorNecklace", new PredatorNecklace());
            Registry.Add("WhiteDragonInAnOrb", new WhiteDragonInAnOrb());
            Registry.Add("LifeSteal", new Lifesteal());
            Registry.Add("ManaSteal", new Manasteal());
            Registry.Add("MasterSword", new MasterSword());
            Registry.Add("CritBomb", new CritBomb());

            //components

            //Set effs
            Registry.Add("Phylac", new Phylac());

            //components
            Registry.Add("FireRune", new FireRune());

            //Runes
            Registry.Add("Percise", new Percise());
            RuneFameCosts.Add("Percise", 500);
            RuneHandlerToItemId.Add("Percise", Resources.Id2Item["Rune of the Brute"].Type);
            Registry.Add("Vampirism", new Vampirism());
            RuneFameCosts.Add("Vampirism", 500);
            RuneHandlerToItemId.Add("Vampirism", Resources.Id2Item["Rune of Vampirism"].Type);
            Registry.Add("Brute", new Brute());
            RuneFameCosts.Add("Brute", 250);
            RuneHandlerToItemId.Add("Brute", Resources.Id2Item["Rune of the Brute"].Type);
            Registry.Add("Mage", new Mage());
            RuneFameCosts.Add("Mage", 500);
            RuneHandlerToItemId.Add("Mage", Resources.Id2Item["Rune of the Mage"].Type);
            Registry.Add("Elven", new Elven());
            RuneFameCosts.Add("Elven", 500);
            RuneHandlerToItemId.Add("Elven", Resources.Id2Item["Rune of Elven Magic"].Type);
            Registry.Add("Juggernaut", new Juggernaut());
            RuneFameCosts.Add("Juggernaut", 250);
            RuneHandlerToItemId.Add("Juggernaut", Resources.Id2Item["Rune of the Juggernaut"].Type);
            Registry.Add("Ephemeral", new EphemeralRune());
            RuneFameCosts.Add("Ephemeral", 250);
            RuneHandlerToItemId.Add("Ephemeral", Resources.Id2Item["Rune of the Sprites"].Type);
        }

    }
}
