using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class PredatorNecklace : IItemHandler
    {
        private int counter = 0;
        public void OnTick(Player p) 
        {
            counter = (counter + 1) % 32;
            if(counter == 0)
            {
                var enemies = p.GetNearbyEntities(5f).Where((e) => e is Enemy && !e.HasConditionEffect(Common.ConditionEffectIndex.Invincible) && !e.HasConditionEffect(Common.ConditionEffectIndex.Invulnerable));
                var packets = new List<byte[]>();
                var count = 0;
                foreach (var e in enemies)
                {
                    if (count == 5) break;
                    var enemy = e as Enemy;
                    enemy.Damage(p, 50, null, true, false);
                    packets.Add(GameServer.ShowEffect(ShowEffectIndex.Flow, p.Id, 0xFFFFFFFF, enemy.Position));
                    count++;
                }
                p.Heal(count * 50, false, true);
                foreach (var packet in packets)
                {
                    p.Parent.BroadcastPacketNearby(packet, p.Position, (p) => p.Client.HasEffectsEnabled());
                }
            }
        }
    }
}
