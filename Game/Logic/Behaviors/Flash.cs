using RotMG.Game.Entities;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Behaviors 
{
    class Flash : Behavior
    {
        uint color;
        float flashPeriod;
        int flashRepeats;

        /*
         * Makes enemies flash periodically, 
         * color is a hex number representing the color 0x(AA)(RR)(GG)(BB)
         * flashPeriod is the time to flash once
         * flashRepeats is the amount to flash
         */
        public Flash(uint color, double flashPeriod, int flashRepeats)
        {
            this.color = color;
            this.flashPeriod = (float)flashPeriod;
            this.flashRepeats = flashRepeats;
        }

        public override void Enter(Entity host)
        {
            var flashPacket = GameServer.ShowEffect(Common.ShowEffectIndex.Flash, host.Id, color, new Common.Vector2(flashPeriod, flashRepeats));
            var players = host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius)
                .Where(e => e is Player j && j.Entities.Contains(host)).ToArray();

            foreach (var en in players)
                (en as Player).Client.Send(flashPacket);
        }
    }
}
