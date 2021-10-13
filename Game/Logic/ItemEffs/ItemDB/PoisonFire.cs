using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class PoisonFire : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player p)
        {
            var inRange = p.Position.Distance(position) <= Player.MaxAbilityDist && p.Parent.GetTileF(position.X, position.Y) != null;
            if (inRange)
            {
                var placeholder = new Placeholder();
                p.Parent.AddEntity(placeholder, position);

                var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, p.Id, 0xffddff00, position, speed: 1500);

                foreach (var j in p.Parent.PlayerChunks.HitTest(p.Position, Player.SightRadius))
                    if (j is Player k && (k.Client.Account.Effects || k.Equals(p)))
                        k.Client.Send(@throw);

                Manager.AddTimedAction(1500, () =>
                {
                    if (placeholder.Parent != null)
                    {
                        if (p.Parent != null)
                        {
                            var fire = new Enemy(Resources.Id2Object["Poison Fire"].Type);
                            fire.PlayerOwner = p;
                            placeholder.Parent.AddEntity(fire, placeholder.Position);
                        }
                        placeholder.Parent.RemoveEntity(placeholder);
                    }
                });
            }
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
