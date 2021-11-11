using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public class WoodlandLabyrinth : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Woodland Weakness Turret",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(25, index: 0, count: 8, cooldown: 3000, cooldownOffset: 4000)
            );
            db.Init("Woodland Silence Turret",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(25, index: 0, count: 8, cooldown: 3000, cooldownOffset: 4000)
            );
            db.Init("Woodland Paralyze Turret",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(25, index: 0, count: 8, cooldown: 3000, cooldownOffset: 4000)
            );
            db.Init("Wooland Armor Squirrel",
                new Prioritize(
                    new Follow(0.52f, 8, 2, cooldown: 500),
                    new StayBack(0.7f, 4)
                ),
                new Shoot(25, index: 0, count: 3, shootAngle: 30, cooldown: 700, cooldownOffset: 1000)
            );
            db.Init("Woodland Ultimate Squirrel",
                new Prioritize(
                    new Follow(0.3f, 8, 1),
                    new Wander(0.3f)
                ),
                new Shoot(25, index: 0, count: 3, shootAngle: 10, cooldown: 900, cooldownOffset: 1000),
                new Shoot(25, index: 0, count: 3, shootAngle: 35, cooldown: 900, cooldownOffset: 1000),
                new Shoot(25, index: 0, count: 1, shootAngle: 35, cooldown: 1100, cooldownOffset: 21000)
            );
            db.Init("Woodland Goblin Mage",
                new Prioritize(
                    new Follow(0.3f, 8, 2, cooldown: 500),
                    new StayBack(0.7f, 4)
                ),
                new Shoot(55, index: 0, count: 2, shootAngle: 10, cooldown: 900, cooldownOffset: 1000)
            );
            db.Init("Woodland Goblin",
                new Follow(0.46f, 8, 1),
                new Shoot(25, index: 0, count: 1, shootAngle: 20, cooldown: 900, cooldownOffset: 1000)
            );
            db.Init("Woodland Mini Megamoth",
                new Prioritize(
                    new Protect(0.5f, "Epic Mama Megamoth", protectionRange: 15, reprotectRange: 3),
                    new Wander(0.35f)
                ),
                new Shoot(25, index: 0, count: 3, shootAngle: 20, cooldown: 700, cooldownOffset: 1000),
                new Threshold(0.5f,
                    new ItemLoot("Magic Potion", 0.1f),
                    new ItemLoot("Magic Potion", 0.1f)
                )
            );
            db.Init("Mini Larva",
                new Prioritize(
                    new Protect(0.5f, "Murderous Megamoth", protectionRange: 15, reprotectRange: 3),
                    new Wander(0.35f)
                ),
                new Shoot(25, index: 0, count: 6, cooldown: 3500, cooldownOffset: 1200),
                new Threshold(0.5f,
                    new ItemLoot("Health Potion", 0.01f),
                    new ItemLoot("Magic Potion", 0.01f)
                )
            );
            db.Init("Epic Larva",
                new State("norm",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Follow(0.08f, 8, 1),
                    new Shoot(8.4f, 1, fixedAngle: 50, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 140, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 240, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 325, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 0, cooldown: 1750),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 0, cooldown: 1750),
                    //    new TossObject("Blood Ground Effector", angle: null, cooldown: 3750),
                    new HealthTransition(0.5f, "tran")
                ),
                new State("tran",
                    new Flash(0xFF0000, 2, 2),
                    new HealthTransition(0.3f, "home")
                ),
                new State("home",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TransformOnDeath("Epic Mama Megamoth"),
                    new Flash(0xFF0000, 2, 2),
                    new Suicide()
                )
            );
            db.Init("Epic Mama Megamoth",
                new State("norm",
                    new Spawn("Woodland Mini Megamoth", 1, 10, 90000),
                    new Spawn("Woodland Mini Megamoth", 1, 2, 5500),
                    new Reproduce("Woodland Mini Megamoth", 2, 4, 3000),
                    new Prioritize(
                        new Charge(1, 4),
                        new Wander(0.36f)
                    ),
                    new Shoot(8.4f, 1, fixedAngle: 60, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 150, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 255, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 335, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 50, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 140, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 240, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 325, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 0, cooldown: 2000),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 0, cooldown: 2000),
                    new HealthTransition(0.5f, "tran")
                ),
                new State("tran",
                    new Flash(0xFF0000, 2, 2),
                    new HealthTransition(0.3f, "home")
                ),
                new State("home",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TransformOnDeath("Murderous Megamoth"),
                    new Flash(0xFF0000, 2, 2),
                    new Suicide()
                )
            );
            db.Init("Murderous Megamoth",
                new DropPortalOnDeath("Realm Portal"),
                new Spawn("Mini Larva", 1, 14, 90000),
                new Spawn("Mini Larva", 1, 2, 5500),
                new Prioritize(
                    new Charge(1.25f, 4),
                    new Follow(0.4f, 8, 1)
                ),
                new Shoot(25, index: 1, count: 2, cooldown: 700, cooldownOffset: 1000),
                new Shoot(8.4f, 1, fixedAngle: 45, index: 0, cooldown: 2000,
                    cooldownOffset: 3000),
                new Shoot(8.4f, 1, fixedAngle: 135, index: 0, cooldown: 2000,
                    cooldownOffset: 3000),
                new Shoot(8.4f, 1, fixedAngle: 235, index: 0, cooldown: 2000,
                    cooldownOffset: 3000),
                new Shoot(8.4f, 1, fixedAngle: 315, index: 0, cooldown: 2000,
                    cooldownOffset: 3000),
                //new Threshold(0.02f,
                //    new ItemLoot("Leaf Bow", 0.05f)
                //),
                new Threshold(0.03f,
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.1f),
                    new TierLoot(12, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.05f),
                    new TierLoot(13, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(12, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(5, TierLoot.LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Vitality", 1)
                    //new ItemLoot("Campaign Point x25", 1)
                )
            );
        }
    }
}