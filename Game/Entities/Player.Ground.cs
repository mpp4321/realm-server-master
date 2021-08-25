using System;
using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System.Collections.Generic;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        private const float MoveSpeedThreshold = 1.1f;
        private const int TeleportCooldown = 10000;
        private const int SpeedHistoryCount = 3; //in world ticks (10 = 1 sec history), the lower the count, the stricter the detection

        public float MoveMultiplier = 1f;
        public int MoveTime;
        public int AwaitingMoves;
        public Queue<int> AwaitingGoto;
        public List<float> SpeedHistory;
        public float PushX;
        public float PushY;
        public int NextTeleportTime;

        public void PushSpeedToHistory(float speed)
        {
            SpeedHistory.Add(speed);
            if (SpeedHistory.Count > SpeedHistoryCount)
                SpeedHistory.RemoveAt(0); //Remove oldest entry
        }

        public float GetHighestSpeedHistory()
        {
            float ret = 0f;
            for (int i = 0; i < SpeedHistoryCount; i++)
            {
                if (SpeedHistory[i] > ret)
                    ret = SpeedHistory[i];
            }
            return ret;
        }

        public bool ValidMove(int time, Vector2 pos)
        {
            var diff = time - MoveTime;
            var movementSpeed = MathF.Max(GetMovementSpeed(), GetHighestSpeedHistory());
            var distanceTraveled = (movementSpeed * diff) * MoveSpeedThreshold;
            var pushedServer = new Vector2(Position.X - diff * PushX, Position.Y - diff * PushY);
            if (pos.Distance(pushedServer) > distanceTraveled && pos.Distance(Position) > distanceTraveled)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Move stuffs... DIST/SPD = " + pos.Distance(pushedServer) + " : " + distanceTraveled);
#endif
                return false;
            }
            return true;
        }

        public void TryMove(int time, Vector2 pos)
        {
            if (!ValidTime(time))
            {
                Client.Disconnect();
                return;
            }

            if (AwaitingGoto.Count > 0)
            {
                foreach (var gt in AwaitingGoto)
                {
                    if (gt + TimeUntilAckTimeout < time)
                    {
                        Program.Print(PrintType.Error, "Goto ack timed out");
                        Client.Disconnect();
                        return;
                    }
                }
#if DEBUG
                Program.Print(PrintType.Error, "Waiting for goto ack...");
#endif
                return;
            }

            if (!ValidMove(time, pos))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid move");
#endif
                Client.Disconnect();
                return;
            }

            if (TileFullOccupied(pos.X, pos.Y))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Tile occupied");
#endif
                Client.Disconnect();
                return;
            }

            AwaitingMoves--;
            if (AwaitingMoves < 0)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Too many move packets");
#endif
                Client.Disconnect();
                return;
            }

            var tile = Parent.Tiles[(int) pos.X, (int) pos.Y];
            var desc = Resources.Type2Tile[tile.Type];
            if (desc.Damage > 0 && !HasConditionEffect(ConditionEffectIndex.Invincible))
            {
                if (!(tile.StaticObject?.Desc.ProtectFromGroundDamage ?? false) &&
                    Damage(desc.Id, desc.Damage, new ConditionEffectDesc[0], true))
                    return;
            }

            Parent.MoveEntity(this, pos);
            if (CheckProjectiles(time))
                return;

            if (desc.Push)
            {
                PushX = desc.DX;
                PushY = desc.DY;
            }
            else
            {
                PushX = 0;
                PushY = 0;
            }

            MoveMultiplier = GetMoveMultiplier();
            MoveTime = time;
            
            PushSpeedToHistory(GetMovementSpeed()); //Add a new entry
        }

        public void TryGotoAck(int time)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "GotoAck invalid time");
#endif
                Client.Disconnect();
                return;
            }

            if (!AwaitingGoto.TryDequeue(out var t))
            {
#if DEBUG
                Program.Print(PrintType.Error, "No GotoAck to ack");
#endif
                Client.Disconnect();
                return;
            }
        }

        public bool EntityTeleport(int time, int objectId, bool force = false)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid time Teleport");
#endif
                return false;
            }

            if (Manager.TotalTime < NextTeleportTime)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Too early to teleport");
#endif
                return false;
            }

            var en = Parent.GetEntity(objectId);
            if (en == null)
            {
                SendError("Target does not exist");
                return false;
            }
            
            if (force)
            {
                Teleport(time, en.Position);
                return true;
            }

            if (objectId == Id)
                return false;

            if (!Parent.AllowTeleport)
                return false;

            if (!(en is Player))
                return false;

            if (en.HasConditionEffect(ConditionEffectIndex.Invisible))
                return false;

            Teleport(time, en.Position);
            NextTeleportTime = Manager.TotalTime + TeleportCooldown;
            return true;
        }

        public bool Teleport(int time, Vector2 pos, bool itemTp = false)
        {
            if (!RegionUnblocked(pos.X, pos.Y))
                return false;

            if (itemTp)
            {
                var tile = Parent.GetTileF(pos.X, pos.Y);
                if (tile == null || TileUpdates[(int) pos.X, (int) pos.Y] != tile.UpdateCount)
                    return false;
            }

            Parent.MoveEntity(this, pos);
            AwaitingGoto.Enqueue(time);

            var eff = GameServer.ShowEffect(ShowEffectIndex.Teleport, Id, 0xFFFFFFFF, pos);
            var go = GameServer.Goto(Id, pos);

            foreach (var player in Parent.Players.Values)
            {
                if (player.Client.Account.Effects)
                    player.Client.Send(eff);
                player.Client.Send(go);
            }

            return true;
        }
    }
}