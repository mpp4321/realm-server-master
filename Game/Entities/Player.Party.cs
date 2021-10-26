using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Entities
{
    public partial class Player
    {

        /**
         * party stuff here
         */

        public struct WorldLocation
        {
            public int Type;

            public void SetType(int type)
            {
                Type = type;
            }
        }

        public class Party
        {
            public int Leader;

            public WorldLocation SummonRequest;

            public HashSet<int> Members;
            public HashSet<int> Invited;
        }

        public static List<Party> Parties = new List<Party>();

        public Party CurrentParty = null;

        public void PartyReconnect()
        {
            foreach(Party p in Parties)
            {
                if(p.Members.Contains(AccountId))
                {
                    CurrentParty = p;
                    return;
                }
            }
        }

        public void PartyMessage(string message)
        {
            if(CurrentParty != null)
            {
                foreach(int m in CurrentParty.Members)
                {
                    var recipient = Manager.GetPlayer(m);
                    if (recipient == null) continue;
                    var tell = GameServer.Text(Name, -1, NumStars, 5, recipient.Name, message);
                    recipient.Client.Send(tell);
                }
                return;
            }
            SendError("You are not in a party!");
        }

        public void CreateParty()
        {
            if(CurrentParty != null)
            {
                SendError("You are in a party!");
                return;
            }
            CurrentParty = new Party()
            {
                Leader = AccountId,
                Members = new HashSet<int>() { AccountId },
                Invited = new HashSet<int>(),
                SummonRequest = new WorldLocation() { Type = -1 }
            };
            Parties.Add(CurrentParty);
            SendInfo("Party created!");
            return;
        }

        public void InviteToParty(Player p)
        {
            if(CurrentParty == null)
            {
                SendError("You are currently not in a party!");
                return;
            }

            if(CurrentParty.Leader != this.AccountId)
            {
                SendInfo("You must be the leader of this party to invite other players!");
                return;
            }

            if(CurrentParty.Members.Contains(p.AccountId))
            {
                SendError("That player is in your party already!");
                return;
            }

            SendInfo($"Invited {p.Name} to your party!");
            p.SendInfo($"You have been invited to a party by {Name}!");
            CurrentParty.Invited.Add(p.AccountId);
            return;
        }

        public void Disband()
        {
            if(CurrentParty != null)
            {
                if(AccountId == CurrentParty.Leader)
                {
                    Parties.Remove(CurrentParty);
                    foreach(int i in CurrentParty.Members)
                    {
                        var player = Manager.GetPlayer(i);
                        if (player == null)
                        {
                            continue;
                        }
                        player.CurrentParty = null;
                        player.SendInfo("Your party was disbanded!");
                    }
                    CurrentParty = null;
                }
            }
        }

        public void QuakeSingle(int id)
        {
            Client.Send(GameServer.ShowEffect(ShowEffectIndex.Jitter, 0, 0));

            Manager.AddTimedAction(3000, () =>
            {
                Client.Send(GameServer.Reconnect(id));
            });
        }

        public void AcceptInvite()
        {
            if(CurrentParty != null)
            {
                if(CurrentParty.SummonRequest.Type == -1)
                {
                    SendInfo("There is no summon requests to be accepted at this time.");
                    return;
                } else
                {
                    SendInfo("Accepting summon request...");
                    QuakeSingle(CurrentParty.SummonRequest.Type);
                    return;
                }
            }
            foreach(Party p in Parties)
            {
                if(p.Invited.Contains(AccountId))
                {
                    p.Members.Add(AccountId);
                    CurrentParty = p;
                    SendInfo("Joined!");
                    return;
                }
            }
            SendInfo("You have no invites to accept!");
        }

        public void SummonParty()
        {
            if (CurrentParty == null) {
                SendError("You are not in a party!");
                return;
            }
            if(CurrentParty.Leader != AccountId)
            {
                SendError("You are not the leader of this party!");
                return;
            }
            if(CurrentParty.SummonRequest.Type != -1)
            {
                SendError("There is a invite already existing");
                return;
            }

            PartyMessage($"Hello guys I am inviting you to a dungeon, {Parent.DisplayName}!");
            PartyMessage($"Type /paccept to join me in the {Parent.DisplayName}!");

            CurrentParty.SummonRequest.SetType(Parent.Id);
            Manager.AddTimedAction(15000, () =>
            {
                if(CurrentParty != null)
                {
                    CurrentParty.SummonRequest.SetType(-1);
                    PartyMessage("My party invite has just expired! Whoops!");
                }
            });
        }

        public void LeaveParty()
        {
            if(CurrentParty == null)
            {
                SendError("You're not in a party!");
                return;
            }
            if(CurrentParty.Leader == AccountId)
            {
                Disband();
                return;
            }
            CurrentParty.Members.Remove(AccountId);
            CurrentParty = null;
            SendInfo("You've left your party!");
        }

    }
}
