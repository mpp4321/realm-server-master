using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class Immunity : IItemHandler
    {

        public void OnTick(Player p)
        {
            p.RemoveNegativeEffects();
        }
    }
}
