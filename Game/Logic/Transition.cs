namespace RotMG.Game.Logic
{
    public abstract class Transition : IBehavior
    {
        public readonly int Id;
        public virtual int SubIndex { get; set; } = 1;

        public Transition(string targetState)
        {
            StringTargetState = targetState.ToLower();
            Id = ++BehaviorDb.NextId;
        }

        public string StringTargetState; //Only used for parsing.
        public virtual int TargetState { get; set; } = -1;

        public virtual void Enter(Entity host) { }
        public virtual bool Tick(Entity host) => false;
        public virtual void Exit(Entity host) { }
    }
}
