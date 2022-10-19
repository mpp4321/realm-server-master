using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Linq;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class PaladinRecovery : IItemHandler
    {
        static int t = 0;
        public void OnTick(Player p)
        {
            t++;
            if (t % 30 != 0) return;
            var entities = GameUtils.GetNearbyPlayers(p, 5);

            var vitTotal = p.GetStatTotal(6);
            foreach (var e in entities)
            {
                if (e == null) continue;
                if (e is Player pl)
                {
                    pl.Heal(vitTotal * 2, false, true);
                }
            }
            var nova_blue = GameServer.ShowEffect(ShowEffectIndex.Nova, p.Id, 0xffffffff, new Vector2(5, 0));
            foreach (var j in p.Parent.PlayerChunks.HitTest(p.Position, Math.Max(4, Player.SightRadius)))
            {
                if (j is Player pl && pl.Client.Account.Effects)
                {
                    pl.Client?.Send(nova_blue);
                }
            }
        }
    }
}
