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
                    new OnDeath((h) =>
                   {
                       var foundPlayer = h.GetNearbyPlayers(3).Where(a => a == h.PlayerOwner).FirstOrDefault();
                       var foundEnemies = h.GetNearbyEntities(3).Where(a => a is Enemy);
                       var aoe = GameServer.ShowEffect(ShowEffectIndex.Nova, h.Id, 0xffffff00, new Vector2(3f, 0));
                       if (foundPlayer != null)
                       {
                           (foundPlayer as Player).Damage(h.Desc.DisplayId, 100, new ConditionEffectDesc[] { }, true);
                       }
                       h.PlayerOwner.Client.Send(aoe);

                       foreach (var en in foundEnemies)
                       {
                           (en as Enemy).Damage(h.PlayerOwner, 100, new ConditionEffectDesc[] { }, false, true);
                       }
                   }),
                    new Suicide()
                )
                );

            db.Init("Mini Flying Brain",
                new State("Base",
                        new Wander(0.4f),
                        new Shoot(12, count: 5, shootAngle: 72, predictive: 1, cooldown: 500, playerOwner: e => e.PlayerOwner),
                        new TimedTransition("Die", 5000)
                    ),
                    new State("Die", new Suicide())

                );


            db.Init("Electric Snake Ally",
                new State("Base",
                    new Shoot(12, count: 3, shootAngle: 10, cooldown: 500, playerOwner: e => e.PlayerOwner),
                    new TimedTransition("Die", 5000)
                ),
                new State("Die", new Suicide())
            );

            db.Init("Poison Fire", new State("Base",
                new PulseFire((h) =>
                {
                    var entities = GameUtils.GetNearbyEntities(h, 4).OfType<Enemy>().Where(a => a.Type != h.Type);
                    foreach (var e in entities)
                    {
                        if (e != h)
                            e?.Damage(h.PlayerOwner, Player.StatScaling(h.PlayerOwner.GetStatTotal(7), 500, 0, 5), new ConditionEffectDesc[] { }, false, true);
                    }
                    var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, h.Id, 0xffff0000, new Vector2(4, 0));
                    foreach (var j in h.Parent.PlayerChunks.HitTest(h.Position, Math.Max(4, Player.SightRadius)))
                    {
                        if (j is Player p)
                            p.Client?.Send(nova);
                    }
                    return true;
                }, 1000),
                new TimedTransition("Die", 5000)
                ), new State("Die", new Suicide()));

            db.Init("Spider Decoy",
                    new State("Base",
                        new Wander(0.4f),
                        new TimedTransition("Die", 2000)
                    ),
                    new State("Die",
                        new Spawn("CryptSpiderAlly", 3, 1.0, probability: 0.3f),
                        new Decay(0)
                    )
                );

            db.Init("CryptSpiderAlly",
                new State("base",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300, playerOwner: e => e.PlayerOwner),
                    new Prioritize(
                        new StayCloseToSpawn(3f, 2),
                        new Wander(0.2f)
                    ),
                    new PlayerWithinTransition(6, "chase")
                ),
                new State("chase",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300, playerOwner: e => e.PlayerOwner),
                    new Charge(1.5f, 20, 500, targetPlayers: false),
                    new TimedTransition("chase0", 1500)
                ),
                new State("chase0",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300, playerOwner: e => e.PlayerOwner),
                    new Charge(2f, 20, 250, targetPlayers: false),
                    new TimedTransition("chase1", 1000)
                ),
                new State("chase1",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300, playerOwner: e => e.PlayerOwner),
                    new Charge(2.5f, 20, 100, targetPlayers: false),
                    new TimedTransition("return", 400)
                ),
                new State("return",
                    new Decay(0)
                )
                );

            db.Init("White Orb Ally",
                new State("Base",
                        new PulseFire(h =>
                        {
                            var entities = GameUtils.GetNearbyEntities(h, 4).OfType<Enemy>().Where(a => a.Type != h.Type);
                            foreach (var e in entities)
                            {
                                if (e != h)
                                    e?.Damage(h.PlayerOwner, 175, new ConditionEffectDesc[] { new ConditionEffectDesc(ConditionEffectIndex.Bleeding, 5000) }, false, true);
                            }
                            var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, h.Id, 0xffff0000, new Vector2(4, 0));
                            foreach (var j in h.Parent.PlayerChunks.HitTest(h.Position, Math.Max(4, Player.SightRadius)))
                            {
                                if (j is Player p)
                                    p.Client?.Send(nova);
                            }
                            return true;
                        }, cooldown: 1000),
                        new OrbitSpawn(1.0f, 3f, 1f),
                        new TimedTransition("Die", 5000)
                    ),
                    new State("Die", new Suicide())
                );

            db.Init("Black Orb Ally",
                new State("Base",
                        new Shoot(12, count: 3, shootAngle: 20, cooldown: 1000, playerOwner: e => e.PlayerOwner),
                        new OrbitSpawn(1.0f, 3f, 1f, orbitClockwise: true),
                        new TimedTransition("Die", 5000)
                    ),
                    new State("Die", new Suicide())
                );

            db.Init("The Red Guardian",
                new State("Base",
                        new PulseFire(h =>
                        {
                            var entities = GameUtils.GetNearbyEntities(h, 4).OfType<Enemy>().Where(a => a.Type != h.Type);
                            foreach (var e in entities)
                            {
                                if (e != h && !e.Desc.Friendly)
                                {
                                    e.ApplyConditionEffect(ConditionEffectIndex.Stunned, 1000);
                                }
                            }
                            var nova = GameServer.ShowEffect(ShowEffectIndex.Ring, h.Id, 0xff0000ff, new Vector2(4, 0));
                            GameUtils.ShowEffectRange(h.PlayerOwner, h.Parent, h.Position, 10.0f, nova);
                            return true;
                        }, cooldown: 500),
                        new TimedTransition("Die", 5000),
                        new SetAltTexture(0, 1, cooldown: 300, loop: true) 
                    ),
                    new State("Die", new Decay(0))
                );

        }
    }
}
