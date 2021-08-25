using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;

namespace RotMG.Game.Logic.Database
{
    public class Mountains : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Medusa", 
                new IfConditionEffect(ConditionEffectIndex.Slowed, 
                    new Shoot(32, 16)),
                new Shoot(7, 1, cooldown: 5000), 
                new Wander(.4f), 
                new Grenade(radius: 2, damage: 20, cooldown: 1500, color: 0xffFFFF00, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000),
                new ItemLoot("Conducting Wand", 1, .1f));

            db.Init("Big Green Slime",
                    new StayAbove(0.4, 50),
                    new Shoot(9, cooldown: 100),
                    new Wander(0.4f),
                    new Reproduce(densityMax: 5, densityRadius: 10),
                    new TransformOnDeath("Little Green Slime", 4, 8, 1.0)
            );

            db.Init("Little Green Slime",
                    new Shoot(9, cooldown: 100),
                    new Wander(0.4f),
                    new TransformOnDeath("Little Green Slime", 4, 8, .1)
            );

            db.Init("Beholder",
                new Prioritize(
                    new Wander(1),
                    new Shoot(16, 3, cooldown: 1000)),
                new ItemLoot("Doom Bow", .001f, .001f));

            db.Init("Beer God",
            new Wander(2),
            new ItemLoot("Potion of Speed", min: 3));

            db.Init("Guardian of the Lost Lands",
                new Prioritize(
                    new Wander(0.2f)),
                new Shoot(10, count: 8, fixedAngle: 360 / 8, cooldown: 3000, index: 1),
                new Shoot(10, count: 5, shootAngle: 10, cooldown: 1500));

            db.Init("Knight of the Lost Lands",
                new Prioritize(
                    new Wander(0.3f)
                ),
                new Shoot(13, 1, cooldown: 700));
        }
    }
}
