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
            (3f, "Rice Wine"),
            (3f, "Muscat"),
            (3f, "Sauvignon Blanc"),
            (3f, "Shiraz"),
            (0.3f, "Potion of Dexterity"),
            (0.3f, "Potion of Attack"),
            (0.3f, "Potion of Defense"),
            (0.3f, "Potion of Speed"),
            (0.3f, "Potion of Vitality"),
            (0.3f, "Potion of Wisdom"),
            (0.05f, "Realm Equipment Crystal"),
            (0.01f, "Blue Snail Generator")
        };

        public static WeightedElements<string> FishingLootGenerator;

        static Player() {
            FishingLootGenerator = new WeightedElements<string>(FishingLoot);
        }
    }
}