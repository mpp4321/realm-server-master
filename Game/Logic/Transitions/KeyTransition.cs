using RotMG.Game.Logic.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Transitions
{
    public class KeyTransition : Transition
    {

        private string Key { get; set; }
        private object Value { get; set; }

        public KeyTransition(string key, object value, string toTransition) : base(toTransition)
        {
            Key = key;
            Value = value;
        }

        public override bool Tick(Entity host)
        {
            if (host.StateKeys[Key] == null) return false;
            return host.StateKeys[Key].Equals(Value);
        }

    }
}
