using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Protect : Behavior
    {
        //State storage: protect state
        enum ProtectState
        {
            DontKnowWhere,
            Protecting,
            Protected,
        }

        float speed;
        ushort protectee;
        float acquireRange;
        float protectionRange;
        float reprotectRange;
        public Protect(float speed, string protectee, double acquireRange = 10, double protectionRange = 2, double reprotectRange = 1)
        {
            this.speed = speed;
            this.protectee = Resources.Id2Object[protectee].Type;
            this.acquireRange = (float) acquireRange;
            this.protectionRange = (float) protectionRange;
            this.reprotectRange = (float) reprotectRange;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }


        public override bool Tick(Entity host)
        {
            var state = host.StateObject[Id];
            ProtectState s;
            if (state == null) s = ProtectState.DontKnowWhere;
            else s = (ProtectState)state;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var entity = GameUtils.GetNearestEntity(host, acquireRange, protectee);
            Vector2 vect;
            switch (s)
            {
                case ProtectState.DontKnowWhere:
                    if (entity != null)
                    {
                        s = ProtectState.Protecting;
                        goto case ProtectState.Protecting;
                    }
                    break;
                case ProtectState.Protecting:
                    if (entity == null)
                    {
                        s = ProtectState.DontKnowWhere;
                        break;
                    }
                    vect = entity.Position - host.Position;
                    if (vect.Length() > reprotectRange)
                    {
                        vect.Normalize();
                        float dist = host.GetSpeed(speed) * (Settings.MillisecondsPerTick / 1000f);
                        host.ValidateAndMove(vect * dist + host.Position);
                    }
                    else
                    {
                        s = ProtectState.Protected;
                    }
                    break;
                case ProtectState.Protected:
                    if (entity == null)
                    {
                        s = ProtectState.DontKnowWhere;
                        break;
                    }
                    vect = entity.Position - host.Position;
                    if (vect.Length() > protectionRange)
                    {
                        s = ProtectState.Protecting;
                        goto case ProtectState.Protecting;
                    }
                    break;

            }

            host.StateObject[Id] = s;
            return true;
        }
    }
}
