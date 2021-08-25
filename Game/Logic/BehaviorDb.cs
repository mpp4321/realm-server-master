using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RotMG.Game.Logic.Loots;

namespace RotMG.Game.Logic
{
    public interface IBehavior { }

    public interface IBehaviorDatabase
    {
        public void Init(BehaviorDb db);
    }

    public class BehaviorModel 
    {
        public Dictionary<int, State> States;
        public List<Behavior> Behaviors;
        public Loot Loot;

        public BehaviorModel(params IBehavior[] behaviors)
        {
            States = new Dictionary<int, State>();
            Behaviors = new List<Behavior>();
            var loots = new List<MobDrop>();
            foreach (var bh in behaviors)
            {
                if (bh is MobDrop) loots.Add(bh as MobDrop);
                if (bh is Behavior) Behaviors.Add(bh as Behavior);
                if (bh is State)
                {
                    var state = bh as State;
                    States.Add(state.Id, state);
                }
            }
            Loot = new Loot(loots.ToArray());

            foreach (var s1 in States.Values)
                foreach (var t in s1.Transitions)
                    foreach (var s2 in States.Values)
                        if (s2.StringId == t.StringTargetState)
                            t.TargetState = s2.Id;

            foreach (var s1 in States.Values)
                foreach (var s2 in s1.States.Values)
                    s2.FindStateTransitions();
        }
    }

    public class BehaviorDb
    {
        public static int NextId;
        public Dictionary<int, BehaviorModel> Models;

        public BehaviorDb()
        {
            Models = new Dictionary<int, BehaviorModel>();
            var results = from type in Assembly.GetCallingAssembly().GetTypes()
                          where typeof(IBehaviorDatabase).IsAssignableFrom(type) && !type.IsInterface
                          select type;

            foreach (var k in results)
            {
#if DEBUG
                Program.Print(PrintType.Debug, $"Initializing Behavior <{k}>");
#endif
                var bd = (IBehaviorDatabase)Activator.CreateInstance(k);
                bd?.Init(this);
            }
        }

        public void Init(string id, params IBehavior[] behaviors)
        {
            int type = Resources.Id2Object[id].Type;
#if DEBUG
            if (Models.ContainsKey(type))
                throw new Exception("Behavior already resolved for this entity.");
#endif

            Models[type] = new BehaviorModel(behaviors);
        }

        public BehaviorModel Resolve(ushort type)
        {
            if (Models.TryGetValue(type, out var model))
                return model;
            return null;
        }
    }
}
