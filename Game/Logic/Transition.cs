using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Logic
{
    public abstract class Transition : IBehavior
    {
        public readonly int Id;
        public virtual int SubIndex { get; set; } = 1;

        public Transition(params string[] targetStates)
        {
            CurrentState = targetStates[0].ToLower();
            TargetStates = targetStates.Select(a => a.ToLower()).ToDictionary(a => a, a => -1);
            Id = ++BehaviorDb.NextId;
        }

        public string CurrentState = "";
        public Dictionary<string, int> TargetStates = new Dictionary<string, int>();

        public virtual void Enter(Entity host) { }
        public virtual bool Tick(Entity host) => false;
        public virtual void Exit(Entity host) { }
    }
}
