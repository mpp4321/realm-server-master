using RotMG.Game.Logic;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game
{
    class RandomState : State
    {

        private List<Behavior> _behaviors;
        private readonly int _num;

        public override List<Behavior> Behaviors { 
            get
            {
                List<Behavior> getting = new List<Behavior>();
                for(int i = 0; i < _num; i++)
                {
                    getting.Add(
                        _behaviors[MathUtils.Next(_behaviors.Count)]
                    );
                }
                return _behaviors;
            } 
            set
            {
                _behaviors = value;
            }
        }

        public RandomState(string id, int num, params IBehavior[] behaviors) : base(id, behaviors)
        {
            this._num = num;
        }

    }
}
