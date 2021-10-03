using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Taunt : Behavior
    {
        //State storage: time

        float probability = 1;
        bool broadcast = false;
        Cooldown cooldown = new Cooldown(1000, 500);
        string[] text;
        int? ordered;

        public Taunt(params string[] text)
        {
            this.text = text;
        }

        public Taunt(double probability, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
        }
        public Taunt(bool broadcast, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
        }
        public Taunt(Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.broadcast = broadcast;
        }

        public Taunt(double probability, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.cooldown = cooldown;
        }

        public Taunt(bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }

        private static Random _Random = new Random();

        public override bool Tick(Entity host)
        {
            var state = host.StateObject[Id];
            if (state != null && cooldown.CoolDown == 0) return false;    //cooldown = 0 -> once per state entry

            int c;
            if (state == null) c = cooldown.Next(_Random);
            else c = (int)state;

            c -= Settings.MillisecondsPerTick;
            state = c;
            if (c > 0)
            {
                host.StateObject[Id] = state;
                return false;
            }

            c = cooldown.Next(_Random);
            state = c;

            if (_Random.NextDouble() >= probability) return false;

            string taunt;
            if (ordered != null)
            {
                taunt = text[ordered.Value];
                ordered = (ordered.Value + 1) % text.Length;
            }
            else
                taunt = text[_Random.Next(text.Length)];
            
            if (taunt.Contains("{PLAYER}"))
            {
                Entity player = GameUtils.GetNearestPlayer(host, 10);
                if (player == null) return false;
                taunt = taunt.Replace("{PLAYER}", player.Name);
            }

            taunt = taunt.Replace("{HP}", (host as Enemy).Hp.ToString());
            taunt = $"<{host.Desc.DisplayId}>" + " " + taunt;

            
            var packet = GameServer.Text
            (
                "#" + host.Name,
                host.GetObjectDefinition().ObjectType,
                -1,
                 3,
                "",
                taunt
            );
            if (broadcast)
            {
                foreach (var player in host.Parent.Players.Values)
                    player.Client.Send(packet);
            }
            else
                foreach (var i in host.Parent.PlayerChunks.HitTest(host.Position, 15f).Where(e => e is Player))
                {
                    if (i is Player)
                        (i as Player).Client.Send(packet);
                }

            host.StateObject[Id] = state;

            return true;
        }
    }
}
