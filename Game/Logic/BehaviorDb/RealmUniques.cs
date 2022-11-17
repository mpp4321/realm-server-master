using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Database
{
    class RealmUniques : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Sheep Statue", new Shoot(10f, 4, 90, 0, rotateAngle: 45, cooldown: 1000));

            db.Init("Sheep God",
                new HPScale(0.5f),
                new State("init",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                    new ConditionalEffect(ConditionEffectIndex.StasisImmune, true),
                    new Taunt(cooldown: 0, "Bah. Fools."),
                    new TimedTransition("go", 3000)
                ),
                new State("go",
                    new Wander(0.8f),
                    new QueuedBehav(
                        new HealSelf(cooldown: 1000, 20000),
                        new CooldownBehav(10000, null),
                        new Grenade(12, 200, 4, cooldown: 10000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 5000, color: 0xFFFF00)
                    ),
                    new Shoot(99, 3, 30, cooldown: 300),
                    new Shoot(99, 5, 30, cooldown: 300, cooldownOffset: 300),
                    new HealthTransition(0.5f, "spin_pre")
                ),
                new State("spin_pre",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt(cooldown: 0, "now spin."),
                    new QueuedBehav(
                        new TossObject("Sheep Statue", 8f, 0f),
                        new TossObject("Sheep Statue", 8f, 90f),
                        new TossObject("Sheep Statue", 8f, 180f),
                        new TossObject("Sheep Statue", 8f, 270f)
                    ),
                    new TimedTransition("spin", 3000)
                ),
                new State("spin",
                    new Shoot(30, 5, 360 / 5, 1, 0, -16, cooldown: 200),
                    new Shoot(99, 3, 30, cooldown: 600),
                    new TransitionFrom("spin", "invuln"),
                    new QueuedBehav(
                        new HealEntity(12f, "Sheep", 20000, cooldown: 1000),
                        new CooldownBehav(10000, null),
                        new Taunt(cooldown: 0, "quak"),
                        new Grenade(12, 200, 4, cooldown: 10000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 300, color: 0xFFFF00)
                    ),
                    new State("invuln", 
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(10f, "vuln", "Sheep Statue")
                    ),
                    new State("vuln")
                ),
                new Threshold(0.001f,
                    LootTemplates.CrystalsHardRegular()
                ),
                new Threshold(0.001f,
                    new ItemLoot("Fiery Equipment Crystal", 0.5f),
                    new ItemLoot("Ghastly Equipment Crystal", 0.5f),
                    new ItemLoot("Oryx Equipment Crystal", 0.5f),
                    new ItemLoot("Potion of Grand Life", 0.1f),
                    new ItemLoot("Potion of Grand Mana", 0.1f),
                    new ItemLoot("Potion of Grand Attack", 0.1f),
                    new ItemLoot("Potion of Grand Defense", 0.1f),
                    new ItemLoot("Potion of Grand Speed", 0.1f),
                    new ItemLoot("Potion of Grand Dexterity", 0.1f),
                    new ItemLoot("Potion of Grand Wisdom", 0.1f),
                    new ItemLoot("Potion of Grand Vitality", 0.1f)
                ),
                new Threshold(0.01f,
                    new MobDrop(new LootDef.Builder()
                                .Item("Fame Consumable")
                                .OverrideJson( new ItemDataJson() { MiscIntOne = 100 })
                                .Chance(0.1f)
                                .Build()
                    ),
                    new MobDrop(new LootDef.Builder()
                                .Item("Skin Unlocker")
                                .OverrideJson( new ItemDataJson() { SkinId = "Sheep_Warrior" })
                                .Chance(0.1f)
                                .Build()
                    ),
                    new MobDrop(new LootDef.Builder()
                                .Item("Skin Unlocker")
                                .OverrideJson( new ItemDataJson() { SkinId = "Sheep_Rogue" })
                                .Chance(0.1f)
                                .Build()
                    ),
                    new MobDrop(new LootDef.Builder()
                                .Item("Skin Unlocker")
                                .OverrideJson( new ItemDataJson() { SkinId = "Sheep_Wizard" })
                                .Chance(0.1f)
                                .Build()
                    ),
                    new MobDrop(new LootDef.Builder()
                                .Item("Skin Unlocker")
                                .OverrideJson( new ItemDataJson() { SkinId = "Sheep_Archer" })
                                .Chance(0.1f)
                                .Build()
                    ),
                    new MobDrop(new LootDef.Builder()
                                .Item("Skin Unlocker")
                                .OverrideJson( new ItemDataJson() { SkinId = "Sheep_Priest" })
                                .Chance(0.1f)
                                .Build()
                    )
                )
            );
        }
    }
}
