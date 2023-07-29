using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    public class GhostlyAura : IItemHandler
    {

        private int T = 0; 
        public virtual void OnTick(Player p)
        {
            if (p.AwaitingGoto.Count > 0) return;
            var count = 2;
            var projs = new List<Projectile>(count);
            var startId = p.NextAEProjectileId;
            var desc = Resources.Id2Item["Dummy Item for Res Armor"];
            p.NextAEProjectileId += count;
            var angle = MathUtils.NextFloat() * 2 * MathF.PI;
            for (var i = 0; i < count; i++)
            {
                var damagelower = p.GetStatTotal(7) * 15;
                var d = p.GetNextDamage(damagelower, (int)(damagelower * 1.3), new Common.ItemDataJson { });
                var pr = new Projectile(p, desc.Projectile[0], startId + i, p.GetLastClientTime(), angle + MathF.PI * i, p.Position, 0f, 0f, d);
                projs.Add(pr);
            }

            p.AwaitProjectiles(projs);

            var nova = GameServer.ServerPlayerShoot(startId, p.Id, desc.Type, p.Position, angle, MathF.PI, projs);

            foreach (var j in p.Parent.PlayerChunks.HitTest(p.Position, Player.SightRadius))
            {
                if (j is Player k)
                {
                    if (k.Client.Account.AllyShots || k.Equals(this))
                        k.Client.Send(nova);
                }
            }

        }

    }
}
