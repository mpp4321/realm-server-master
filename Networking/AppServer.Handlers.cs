using RotMG.Common;
using RotMG.Game;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Xml.Linq;

namespace RotMG.Networking
{
    public static partial class AppServer
    {
        private static byte[] CharList(HttpListenerContext context, NameValueCollection query)
        {
            var data = new XElement("Chars");

            var accountInUse = false;
            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (!(accountInUse = Database.IsAccountInUse(acc)))
                {
                    data.Add(new XAttribute("nextCharId", acc?.NextCharId ?? 0));
                    data.Add(new XAttribute("maxNumChars", acc?.MaxNumChars ?? 0));
                    data.Add(acc?.ExportApp() ?? new XElement("Account"));
                    data.Add(new XElement("OwnedSkins", string.Join(",", acc?.OwnedSkins ?? new List<int>())));
                    if(acc != null)
                    {
                        data.Add(Database.GetNews(acc));
                        foreach (var charId in acc?.AliveChars)
                        {
                            var character = Database.LoadCharacter(acc, charId);
                            if (character == null || character.Data == null) continue;
                            var export = character.ExportApp();
                            export.Add(new XAttribute("id", charId));
                            data.Add(export);
                        }
                    }
                }
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return accountInUse ? WriteError("Account in use!") : Write(data.ToString());
        }

        private static byte[] Verify(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;

            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null || acc.Data == null)
                    data = WriteError("Invalid account.");
                else if (Database.IsAccountInUse(acc))
                {
                    data = WriteError("Account in use!");
                    Manager.GetClient(acc.Id)?.Disconnect();
                }
                else
                    data = WriteSuccess();
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] Register(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            var newUsername = query["newUsername"];
            var newPassword = query["newPassword"];

            if (!Database.IsValidUsername(newUsername))
                return WriteError("Invalid username.");

            if (!Database.IsValidPassword(newPassword))
                return WriteError("Invalid password.");

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var status = Database.RegisterAccount(newUsername, newPassword, GetIPFromContext(context));
                if (status == RegisterStatus.Success)
                    data = WriteSuccess();
                else data = WriteError(status.ToString());
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] FameList(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            _listenEvent.Reset(); 
            Program.PushWork(() =>
            {
                data = Write(Database.GetLegends(query["timespan"]).ToString());
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();
            return data;
        }

        private static byte[] CharFame(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            var accId = int.Parse(query["accountId"]);
            var charId = int.Parse(query["charId"]);
            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var legend = Database.GetLegend(accId, charId);
                data = string.IsNullOrWhiteSpace(legend) ? WriteError("Invalid character") : Write(legend);
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();
            return data;
        }

        private static byte[] CharDelete(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;

            var username = query["username"];
            var password = query["password"];
            var charId = int.Parse(query["charId"]);

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account.");
                else if (Database.IsAccountInUse(acc))
                    data = WriteError("Account in use!");
                else
                    data = Database.DeleteCharacter(acc, charId) ? WriteSuccess() : WriteError("Issue deleting character");
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] AccountPurchaseCharSlot(HttpListenerContext context, NameValueCollection query)
        {

            byte[] data = null;

            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account.");
                else if (Database.IsAccountInUse(acc))
                    data = WriteError("Account in use!");
                else
                    data = Database.BuyCharSlot(acc) ? WriteSuccess() : WriteError("Not enough fame");
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] AccountPurchaseSkin(HttpListenerContext context, NameValueCollection query)
        {

            byte[] data = null;

            var username = query["username"];
            var password = query["password"];
            var skinType = int.Parse(query["skinType"]);

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account.");
                else if (Database.IsAccountInUse(acc))
                    data = WriteError("Account in use!");
                else
                    data = Database.BuySkin(acc, skinType) ? WriteSuccess() : WriteError("Could not buy skin");
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] AccountChangePassword(HttpListenerContext context, NameValueCollection query)
        {

            byte[] data = null;

            var username = query["username"];
            var password = query["password"];
            var newPassword = query["newPassword"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account.");
                else if (Database.IsAccountInUse(acc))
                    data = WriteError("Account in use!");
                else
                    data = Database.ChangePassword(acc, newPassword) ? WriteSuccess() : WriteError("Could not change password");
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] GuildListMembers(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            
            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account");
                else if (string.IsNullOrEmpty(acc.GuildName))
                    data = WriteError("Not in a guild");
                else
                {
                    data = Write(Database.GetGuild(acc.GuildName).ExportApp().ToString());
                }
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] GuildGetBoard(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            
            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account");
                else if (string.IsNullOrEmpty(acc.GuildName))
                    data = WriteError("Not in a guild");
                else
                {
                    data = Write(Database.GetGuild(acc.GuildName).BoardMessage);
                }
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }

        private static byte[] GuildSetBoard(HttpListenerContext context, NameValueCollection query)
        {
            byte[] data = null;
            
            var username = query["username"];
            var password = query["password"];

            _listenEvent.Reset();
            Program.PushWork(() =>
            {
                var acc = Database.Verify(username, password, GetIPFromContext(context));
                if (acc == null)
                    data = WriteError("Invalid account");
                else if (string.IsNullOrEmpty(acc.GuildName))
                    data = WriteError("Not in a guild");
                else if (acc.GuildRank < 20)
                    data = WriteError("No permission");
                else
                {
                    data = Database.SetGuildBoard(Database.GetGuild(acc.GuildName), query["board"])
                        ? Write(query["board"])
                        : WriteError("Failed to set board");
                }
            }, () => _listenEvent.Set());
            _listenEvent.WaitOne();

            return data;
        }
    }
}
