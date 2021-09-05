using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic
{
    static class Utils
    {
        public static int CountEntity(this Entity entity, float dist, ushort? objType)
        {
            if (entity.Parent == null) return 0;
            int ret = 0;
            if (objType == null)
                foreach (var i in entity.Parent.PlayerChunks.HitTest(entity.Position, dist).Where(e => e is Player))
                {
                    var d = MathUtils.Distance(i.Position, entity.Position);
                    if (d < dist)
                        ret++;
                }
            else
                foreach (var i in entity.Parent.EntityChunks.HitTest(entity.Position, dist))
                {
                    if (i.GetObjectDefinition().ObjectType != objType.Value) continue;
                    var d = MathUtils.Distance(i.Position, entity.Position);
                    if (d < dist)
                        ret++;
                }
            return ret;
        }

        public static int CountEntity(this Entity entity, float dist, string group)
        {
            if (entity.Parent == null) return 0;
            int ret = 0;
            foreach (var i in entity.Parent.EntityChunks.HitTest(entity.Position, dist))
            {
                if (!i.Desc.Group.Equals(group)) continue;
                var d = MathUtils.Distance(i.Position, entity.Position);
                if (d < dist)
                    ret++;
            }
            return ret;
        }

    }
}
