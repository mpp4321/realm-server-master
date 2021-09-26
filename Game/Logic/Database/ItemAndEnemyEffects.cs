using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class ItemAndEnemyEffects : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Thunder Cloud", 
                new State("Base", 
                        new TimedTransition("Die", 3000)
                    ),
                new State("Die", 
                    new OnDeath( (h) =>
                    {
                        var foundPlayer = h.GetNearbyPlayers(3).Where(a => a == h.PlayerOwner).FirstOrDefault();
                        var aoe = GameServer.ShowEffect(ShowEffectIndex.Nova, h.Id, 0xffffff00, new Vector2(3f, 0));
                        if(foundPlayer != null)
                        {
                            (foundPlayer as Player).Damage(h.Desc.DisplayId, 300, new ConditionEffectDesc[] { }, true);
                        }
                        h.PlayerOwner.Client.Send(aoe);
                    }),
                    new Suicide()
                )
                );

            db.Init("Mini Flying Brain",
                new State("Base", 
                        new Wander(0.4f),
                        new Shoot(12, count: 5, shootAngle: 72, cooldown: 500, playerOwner: e => e.PlayerOwner),
                        new TimedTransition("Die", 5000)
                    ),
                    new State("Die", new Suicide())
                );

            db.Init("Poison Fire", new State("Base", 
                new PulseFire((h) =>
                {
                    var entities = GameUtils.GetNearbyEntities(h, 4).OfType<Enemy>().Where(a => a.Type != h.Type);
                    foreach(var e in entities)
                    {
                        if(e != h)
                            e?.Damage(h.PlayerOwner, Player.StatScaling(h.PlayerOwner.GetStatTotal(7), 500, 0, 5), new ConditionEffectDesc[] { }, false, true);
                    }
                    var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, h.Id, 0xffff0000, new Vector2(4, 0));
                    foreach (var j in h.Parent.PlayerChunks.HitTest(h.Position, Math.Max(4, Player.SightRadius)))
                    {
                        if(j is Player p)
                            p.Client.Send(nova);
                    }
                    return true;
                }, 1000),
                new TimedTransition("Die", 5000)
                ), new State("Die", new Suicide()));

        }
    }
}
