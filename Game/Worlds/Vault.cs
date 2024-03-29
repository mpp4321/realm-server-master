using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public sealed class Vault : World
    {
        private readonly Client _client;
        
        public Vault(Map map, WorldDesc desc, Client client) : base(map, desc)
        {
            if (client == null)
                return;
            
            _client = client;

            // Get regions
            var spawn = GetRegion(Region.Spawn);
            var vaultChestLocations = new List<IntPoint>(GetAllRegion(Region.VaultChest));
            vaultChestLocations.Sort((a, b) => Comparer<int>.Default.Compare(
                (a.X - spawn.X) * (a.X - spawn.X) + (a.Y - spawn.Y) * (a.Y - spawn.Y),
                (b.X - spawn.X) * (b.X - spawn.X) + (b.Y - spawn.Y) * (b.Y - spawn.Y)));

            var giftChestLocations = new List<IntPoint>(GetAllRegion(Region.GiftChest));
            giftChestLocations.Sort((a, b) => Comparer<int>.Default.Compare(
                (a.X - spawn.X) * (a.X - spawn.X) + (a.Y - spawn.Y) * (a.Y - spawn.Y),
                (b.X - spawn.X) * (b.X - spawn.X) + (b.Y - spawn.Y) * (b.Y - spawn.Y)));

            // Clear any abnormal issues with saved vault files; because file databases suck

            // Spawn normal chests
            for (var i = 0; i < client.Account.VaultCount && vaultChestLocations.Count > 0; i++)
            {
                var chestModel = new VaultChestModel(client.Account.Id, i);
                if(chestModel.Data != null) 
                {
                    var chest = new VaultChest(chestModel);
                    AddEntity(chest, vaultChestLocations[0].ToVector2() + .5f);
                    vaultChestLocations.RemoveAt(0);
                } else
                {
                    chestModel.Inventory = Enumerable.Repeat(-1, 8).ToArray();
                    chestModel.ItemDatas = Enumerable.Repeat(new ItemDataJson(), 8).ToArray();
                    chestModel.Save();

                    var chest = new VaultChest(chestModel);
                    
                    AddEntity(chest, vaultChestLocations[0].ToVector2() + .5f);
                    vaultChestLocations.RemoveAt(0);
                }
            }
            
            foreach (var point in vaultChestLocations)
            {
                var chest = new ClosedVaultChest();
                AddEntity(chest, point.ToVector2() + .5f);
            }

            // Spawn gift chests
            var gifts = new List<int>(client.Account.Gifts);
            var giftdata = new List<ItemDataJson>(client.Account.GiftData);
            while (gifts.Count > 0 && giftChestLocations.Count > 0)
            {
                var count = Math.Min(8, gifts.Count);
                var countData = Math.Min(8, giftdata.Count);
                var items = gifts.GetRange(0, count);
                var datas = giftdata.GetRange(0, countData);
                gifts.RemoveRange(0, count);
                giftdata.RemoveRange(0, countData);
                var chest = new GiftChest(items, datas, client.Account);
                AddEntity(chest, giftChestLocations[0].ToVector2() + .5f);
                giftChestLocations.RemoveAt(0);
            }
            
            foreach (var point in giftChestLocations)
            {
                var chest = new Entity(0x0743);
                AddEntity(chest, point.ToVector2() + .5f);
            }
        }

        public override bool AllowedAccess(Client client)
        {
            return client.Account.Id == _client.Account.Id && base.AllowedAccess(client);
        }
    }
}