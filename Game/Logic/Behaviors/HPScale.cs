﻿using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    public class HPScale : Behavior
    {

        public static Func<HPScale> BOSS_HP_SCALE_DEFAULT = () =>
        {
            return new HPScale(0.3f);
        };

        private float Factor { get; set; }
        private Cooldown cooldown { get; } = new Cooldown(1000, 0);
        public HPScale(float factor)
        {
            Factor = factor;
        }
        private struct HPScaleState
        {
            public int TimeLeft { get; set; }
            public HashSet<int> Players { get; set; }
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new HPScaleState()
            {
                /* Here we set to 200 to make sure that it doesn't die within 1 second */
                TimeLeft = 200,
                Players = new HashSet<int>()
            };
        }

        public override bool Tick(Entity host)
        {
            bool hasState = host.StateObject.TryGetValue(Id, out var objstate);
            HPScaleState state = (HPScaleState) objstate;
            if (!hasState)
                return false;
            if(host is Enemy en && state.TimeLeft <= 0)
            {
                var setOfPlayers = en.DamageStorage.Keys.Select(a => a.AccountId).ToHashSet();
                var setCount = setOfPlayers.Count();
                if(setCount > 1)
                {
                    state.Players = setOfPlayers;
                    var oldMaxHp = en.MaxHp;
                    en.MaxHp = (int)(en.Desc.MaxHp * (1.0f + (Factor * (setCount - 1)) ));
                    en.Hp += (en.MaxHp - oldMaxHp);
                    state.TimeLeft = cooldown.Next(MathUtils.GetStaticRandom());
                }
            } else
            {
                state.TimeLeft -= Settings.MillisecondsPerTick;
            }
            host.StateObject[Id] = state;
            return false;
        }

    }
}