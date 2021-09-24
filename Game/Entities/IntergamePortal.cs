using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Entities
{
    class IntergamePortal : Portal
    {

        public IntergamePortal(ushort type) : base(type)
        {
        }

        public override World GetWorldInstance(Client connectingClient)
        {
            if(PortalParent != null)
            {
                return PortalParent;
            }
            return base.GetWorldInstance(connectingClient);
        }

    }
}
