using System;
using System.Collections.Generic;
using System.IO;
using RotMG.Common;
using RotMG.Networking;
using System.Linq;
using System.Text.RegularExpressions;
using RotMG.Game.SetPieces;
using RotMG.Game.Worlds;
using RotMG.Utils;
using RotMG.Game.Logic.Mechanics;

namespace RotMG.Game.Entities
{
    public partial class Player
    {

        //Define a dictionary of float, string
        private static List<(float, string)> FishingLoot  = new List<(float, string)>()
        {
            (10f, "Health Potion"),
            (10f, "Magic Potion"),
            (3f, "Rice Wine"),
            (0.1f, "Potion of Dexterity"),
            (0.1f, "Potion of Attack"),
            (0.1f, "Potion of Defense"),
            (0.1f, "Potion of Speed"),
            (0.1f, "Potion of Vitality"),
            (0.1f, "Potion of Wisdom"),
        };

        public static WeightedElements<string> FishingLootGenerator;

        static Player() {
            FishingLootGenerator = new WeightedElements<string>(FishingLoot);
        }
    }
}