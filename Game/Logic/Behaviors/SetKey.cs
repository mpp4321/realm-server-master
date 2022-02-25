using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class SetKey : Behavior
    {
        private string Key { get; set; }
        private object Value { get; set; }
        
        public SetKey(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override void Enter(Entity host)
        {
        }

        public override bool Tick(Entity host)
        {
            host.StateKeys[Key] = Value;
            return true;
        }
    }
}
