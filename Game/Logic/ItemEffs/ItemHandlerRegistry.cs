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
        }

    }
}
