namespace RotMG.Game.Logic
{
    public abstract class Behavior : IBehavior
    {
        public readonly int Id;

        public Behavior()
        {
            Id = ++BehaviorDb.NextId;
        }

        public virtual void Enter(Entity host) { }
        /// <returns>true if behavior complete</returns>
        public virtual bool Tick(Entity host) => true;
        public virtual void Exit(Entity host) { }
        public virtual void Death(Entity host) { }
    }
}
