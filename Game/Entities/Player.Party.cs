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

        public struct Party
        {
            public int Leader;
            public HashSet<int> Members;
            public HashSet<int> Invited;
        }

        public static List<Party> Parties = new List<Party>();

        public Party? CurrentParty = null;

        public void InviteToParty(Player p)
        {
            if(!CurrentParty.HasValue)
            {
                SendError("You are currently not in a party!");
                return;
            }

            if(CurrentParty.Value.Leader != this.AccountId)
            {
                SendInfo("You must be the leader of this party to invite other players!");
                return;
            }

            if(CurrentParty.Value.Members.Contains(p.AccountId))
            {
                SendError("That player is in your party already!");
                return;
            }

            SendInfo($"Invited {p.Name} to your party!");
            CurrentParty.Value.Invited.Add(p.AccountId);
            return;
        }

        public void Disband()
        {

            if(CurrentParty.HasValue)
            {
                if(AccountId == CurrentParty.Value.Leader)
                {
                    Parties.Remove(CurrentParty.Value);
                    foreach(int i in CurrentParty.Value.Members)
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
            } else
            {
                foreach (Party p in Parties)
                {
                }
            }


        }

        public void AcceptInvite()
        {
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

    }
}
