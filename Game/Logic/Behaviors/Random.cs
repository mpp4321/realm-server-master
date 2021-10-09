using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{

    class RandomBehavior : Behavior
    {

        private readonly Behavior[] choices;

        public RandomBehavior(params Behavior[] choices) : base()
        {
            this.choices = choices;
        }

        public override void Enter(Entity host)
        {
            foreach(var c in choices)
            {
                c.Enter(host);
            }
        }

        public override void Exit(Entity host)
        {
            foreach(var c in choices)
            {
                c.Exit(host);
            }
        }

        public override bool Tick(Entity host)
        {
            return choices[MathUtils.Next(choices.Length)].Tick(host);
        }

        public override void Death(Entity host)
        {
            foreach(var c in choices)
            {
                c.Death(host);
            }
        }

    }
}
