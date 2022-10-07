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
    public class MagmaQuiver : IItemHandler
    {

        public void OnProjectileShoot(Player shotFrom, ref Projectile projectile) 
        {
            if (shotFrom.AwaitingGoto.Count > 0) return;
            if (MathUtils.Chance(0.92f)) return;
            var count = 1;
            var projs = new List<Projectile>(count);
            var startId = shotFrom.NextAEProjectileId;
            var desc = Resources.Id2Item["Quiver of Flowing Magma"];
            shotFrom.NextAEProjectileId += count;
            for (var i = 0; i < count; i++)
            {
                var d = shotFrom.GetNextDamage(projectile.Damage, projectile.Damage * 2, new Common.ItemDataJson { });
                var p = new Projectile(shotFrom, desc.Projectile[0], startId + i, shotFrom.GetLastClientTime(), projectile.Angle, shotFrom.Position, d);
                projs.Add(p);
            }

            shotFrom.AwaitProjectiles(projs);

            var nova = GameServer.ServerPlayerShoot(startId, shotFrom.Id, desc.Type, shotFrom.Position, projectile.Angle, 0, projs);

            foreach (var j in shotFrom.Parent.PlayerChunks.HitTest(shotFrom.Position, Player.SightRadius))
            {
                if (j is Player k)
                {
                    if (k.Client.Account.AllyShots || k.Equals(shotFrom))
                        k.Client.Send(nova);
                }
            }

        }

    }
}
