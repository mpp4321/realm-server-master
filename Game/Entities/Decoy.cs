﻿using RotMG.Common;
using RotMG.Networking;
using System;

namespace RotMG.Game.Entities
{
    public class Decoy : Entity
    {
        private const int DecoyMoveTime = 1600;

        public int Duration;
        public float Speed = 1.0f;
        public Vector2 Direction;

        public Decoy(Player player, float angle, int duration, ushort entityType=0x0715) : base(entityType, duration)
        {
#if DEBUG
            if (duration < DecoyMoveTime)
            {
                throw new Exception("Nope.");
            }
#endif
            PlayerOwner = player;
            Duration = duration;
            Direction = new Vector2(
                MathF.Cos(angle),
                MathF.Sin(angle)) / Settings.TicksPerSecond * 5;

            if(entityType == 0x0715)
            {
                if (player.Tex1 != 0)
                    SetSV(StatType.Tex1, player.Tex1);
                if (player.Tex2 != 0)
                    SetSV(StatType.Tex2, player.Tex2);
            }
        }

        public override void Tick()
        {
            var elapsed = Duration - Lifetime.Value;
            if (elapsed <= DecoyMoveTime)
                ValidateAndMove(Position + Speed * Direction);

            base.Tick();
        }

        public override void OnLifeEnd()
        {
            var nova = GameServer.ShowEffect(
                ShowEffectIndex.Nova,
                Id,
                0xffff0000,
                new Vector2(1, 0));

            foreach (var player in Parent.Players.Values)
            {
                if (player.Client.Account.Effects)
                    player.Client.Send(nova);
            }
        }
    }
}
