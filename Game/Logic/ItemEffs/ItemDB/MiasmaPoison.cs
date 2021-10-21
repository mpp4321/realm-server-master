using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class MiasmaPoison : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(by.Id % 10 == 0)
            {
                if (!(hit is Enemy en) || !(by.Owner is Player pl))
                    return;

                    var placeholder = new Placeholder();
                    pl.Parent.AddEntity(placeholder, en.Position);

                    var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, pl.Id, 0xffddff00, pos1: en.Position, speed: 700);
                    var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, placeholder.Id, 0xffddff00, new Vector2(4, 0));

                    foreach (var j in pl.Parent.PlayerChunks.HitTest(pl.Position, Player.SightRadius))
                        if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                            k.Client.Send(@throw);

                    Manager.AddTimedAction(700, () =>
                    {
                        if (placeholder.Parent != null)
                        {
                            if (pl.Parent != null)
                            {
                                foreach (var j in pl.Parent.PlayerChunks.HitTest(pl.Position, Player.SightRadius))
                                    if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                        k.Client.Send(nova);
                                foreach (var j in pl.Parent.EntityChunks.HitTest(placeholder.Position, 4))
                                    if (j is Enemy e)
                                    {
                                        var dmg = Player.StatScaling(pl.GetStatTotal(7), 400f, 75, 5f);
                                        e.ApplyPoison(pl, new ConditionEffectDesc[0], (int)(dmg
                                            / 4), dmg);
                                    }
                            }
                            placeholder.Parent.RemoveEntity(placeholder);
                        }
                    });
            }
        }

    }
}
