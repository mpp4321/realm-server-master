using RotMG.Utils;
using System.Collections.Generic;
using Functional.Maybe;
using System.Linq;
using System.Xml.Linq;
using RotMG.Game;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace RotMG.Common
{
    public interface IDatabaseInfo
    {
        void Export(IData data);
        XElement ExportApp();
    }

    public interface IData
    {
        public IData Child(string childName);
        public string Path();
        public string GetKey(string subKey);
        public string GetKeyOr(string subKey, string defaultValue);
        public int GetInt(string subKey, int def=0);
        public bool GetBool(string subKey, bool def=false);
        public Maybe<T> GetKeyParsed<T>(string subKey); 
        public void SetKey(string subKey, object value);
        public void SetKey(string subKey, string value);
        public void DelKey(string subKey);
        public IData Element(string subKey);
        public List<IData> Elements(string subKey);
    }

    public class RedisObject : IData
    {
        private string parentPath;

        public RedisObject(string parentPath) 
        {
            this.parentPath = parentPath;
        }

        public string Path()
        {
            return this.parentPath;
        }

        private string CombineSubPath(string subKey)
        {
            return $"{parentPath}.{subKey}";
        }

        public void DelKey(string subKey)
        {
            Database.DeleteKey(CombineSubPath(subKey), false);
        }

        public string GetKey(string subKey)
        {
            return Database.GetKey(CombineSubPath(subKey), false);
        }
        public int GetInt(string subKey, int def=0)
        {
            return int.Parse(Database.GetKeyOr(CombineSubPath(subKey), def.ToString(), false));
        }
        public bool GetBool(string subKey, bool def=false)
        {
            return bool.Parse(Database.GetKeyOr(CombineSubPath(subKey), def.ToString(), false));
        }

        public void SetKey(string subKey, object value)
        {
            try
            {
                Database.SetKey(CombineSubPath(subKey), JsonConvert.SerializeObject(value), false);
            } catch
            {
                Program.Print(PrintType.Error, $"Failed to serialize object ${CombineSubPath(subKey)}");
            }
        }

        public void SetKey(string subKey, string value)
        {
            Database.SetKey(CombineSubPath(subKey), value, false);
        }

        public Maybe<T> GetKeyParsed<T>(string subKey)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(GetKey(subKey)).ToMaybe();
            } catch
            {

                Program.Print(PrintType.Error, $"Failed to deserialize object ${CombineSubPath(subKey)}");
                return Maybe<T>.Nothing;
            }
        }

        public IData Element(string subKey)
        {
            return new RedisObject(CombineSubPath(subKey));
        }

        public string GetKeyOr(string subKey, string defaultValue)
        {
            if(Database.KeyExists(CombineSubPath(subKey))) {
                return GetKey(subKey);
            }
            return defaultValue;
        }

        public List<IData> Elements(string subKey)
        {
            return Database.GetList(subKey).Select(v => (IData) new RedisObject(CombineSubPath(v))).ToList();
        }

        public IData Child(string childName)
        {
            return new RedisObject($"{this.Path()}.{childName}");
        }
    }

    public abstract class DatabaseModel : IDatabaseInfo
    {
        public IData Data = null;
        public readonly string Path;
        public DatabaseModel(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                Path = Database.CombineKeyPath(key);
                Reload();
            }
        }

        public void Reload()
        {
            try
            {
                Data = new RedisObject(Path);
                Load();
            } catch(Exception e)
            {
                Console.WriteLine("Error:" + e);
                Console.WriteLine("failed to load " + Path);
            }
        }

        public virtual void Save()
        {
            Export(Data);
        }

        protected abstract void Load();

        // This is now nullable XElement for the sake of the app exports.
        public abstract void Export(IData Data);

        // This is now nullable XElement for the sake of the app exports.
        public abstract XElement? ExportApp();
    }

    public class VaultChestModel : DatabaseModel
    {
        public readonly int Id;
        public int[] Inventory;
        public ItemDataJson[] ItemDatas;
        public VaultChestModel(int accountId, int key) : base($"vault.{accountId}.{key}")
        {
            Id = key;
        }
        private VaultChestModel() : base(null) { }
        public static VaultChestModel CreateFrom(int Id, int[] inventory, ItemDataJson[] itemDatas)
        {
            var model = new VaultChestModel();
            model.Inventory = inventory;
            model.ItemDatas = itemDatas;
            return model;
        }

        protected override void Load()
        {
            Inventory = Data.GetKeyParsed<int[]>("Inventory").OrElse(new int[8]);
            ItemDatas = Data.GetKeyParsed<ItemDataJson[]>("ItemDatas").OrElse(Enumerable.Repeat(new ItemDataJson(), 20).ToArray()).ToArray();
        }

        public override void Export(IData Data)
        {
            Data.SetKey("Inventory", Inventory);
            Data.SetKey("ItemDatas", ItemDatas);
        }

        public override XElement ExportApp()
        {
            throw new NotImplementedException();
        }
    }

    public class CharacterModel : DatabaseModel
    {
        public readonly int Id;

        public int Experience;
        public int Level;
        public int ClassType;
        public int HP;
        public int MP;
        public int[] Stats;
        public int[] Inventory;

        public string ItemDataModifier = "Classical";

        public string[] SelectedRunes = new string[] { };

        public ItemDataJson[] ItemDataJsons = Enumerable.Repeat(new ItemDataJson() { Meta = -1 }, 20).ToArray();

        public int Fame;
        public int Tex1;
        public int Tex2;
        public int SkinType;
        public int HealthPotions;
        public int MagicPotions;
        public int CreationTime;
        public bool Deleted;
        public bool Dead;
        public int DeathFame;
        public int DeathTime;
        public bool HasBackpack;
        public FameStatsInfo FameStats;
        public int PetId;

        public int GlowColor = 0; 
        public int Size = 0;

        public CharacterModel(int accountId, int key) : base($"char.{accountId}.{key}") 
        {
            Id = key;
        }

        protected override void Load()
        {
            Level = Data.GetInt("Level");
            Experience = Data.GetInt("Experience");
            ClassType = Data.GetInt("ClassType");
            HP = Data.GetInt("HP");
            MP = Data.GetInt("MP");
            Stats = Data.GetKeyParsed<int[]>("Stats").OrElse(new int[10]);
            // There are 9 stats including new protection, this is to ensure backwards compatibility with old stat blocks
            // now 10 stats
            Stats = Stats.Concat(Enumerable.Range(Stats.Length, 10 - Stats.Length).Select(a => Resources.Type2Player[(ushort) ClassType].Stats[a].StartingValue)).ToArray();
            Inventory = Data.GetKeyParsed<int[]>("Equipment").OrElse(Enumerable.Repeat(-1, 20).ToArray()).Select(v => v != 0 ? v : -1).ToArray();
            ItemDataJsons = Data.GetKeyParsed<ItemDataJson[]>("ItemDatas").OrElse<ItemDataJson[]>(Enumerable.Repeat(new ItemDataJson(), 20).ToArray());
            Fame = Data.GetInt("Fame");
            Tex1 = Data.GetInt("Tex1");
            Tex2 = Data.GetInt("Tex2");
            SkinType = Data.GetInt("SkinType");
            HealthPotions = Data.GetInt("HealthPotions");
            MagicPotions = Data.GetInt("MagicPotions");
            CreationTime = Data.GetInt("CreationTime");
            Deleted = Data.GetBool("Deleted");
            Dead = Data.GetBool("Dead");
            DeathFame = Data.GetInt("DeathFame");
            DeathTime = Data.GetInt("DeathTime");
            HasBackpack = Data.GetBool("HasBackpack");
            FameStats = new FameStatsInfo(Data.Element("FameStats"));
            PetId = Data.GetInt("PetId");
            ItemDataModifier = Data.GetKey("ItemDataModifier");
            if(ItemDataModifier == "")
            {
                ItemDataModifier = "Classical";
            }
            SelectedRunes = Data.GetKeyParsed<string[]>("Runes").OrElse(new string[0]);

            GlowColor = Data.GetInt("GlowColor");
            Size = Data.GetInt("Size");
        }

        public XElement ExportFame()
        {
            var data = new XElement("Char");
            data.Add(new XElement("ObjectType", ClassType));
            data.Add(new XElement("Level", Level));
            data.Add(new XElement("Exp", Experience));
            data.Add(new XElement("CurrentFame", Fame));
            data.Add(new XElement("Equipment", string.Join(",", Inventory)));
            data.Add(new XElement("ItemDatas", ItemDataJsons));
            data.Add(new XElement("MaxHitPoints", Stats[0]));
            data.Add(new XElement("HitPoints", HP));
            data.Add(new XElement("MaxMagicPoints", Stats[1]));
            data.Add(new XElement("MagicPoints", MP));
            data.Add(new XElement("Attack", Stats[2]));
            data.Add(new XElement("Defense", Stats[3]));
            data.Add(new XElement("Speed", Stats[4]));
            data.Add(new XElement("Dexterity", Stats[5]));
            data.Add(new XElement("HpRegen", Stats[6]));
            data.Add(new XElement("MpRegen", Stats[7]));
            data.Add(new XElement("Tex1", Tex1));
            data.Add(new XElement("Tex2", Tex2));
            data.Add(new XElement("Texture", SkinType));
            return data;
        }
        public override XElement ExportApp()
        {
            var data = new XElement("Char");
            data.Add(new XElement("ObjectType", ClassType));
            data.Add(new XElement("Level", Level));
            data.Add(new XElement("Exp", Experience));
            data.Add(new XElement("CurrentFame", Fame));
            data.Add(new XElement("Equipment", string.Join(",", Inventory)));
            data.Add(new XElement("ItemDatas", string.Join(",", ItemDataJsons.Select(a => ItemDesc.ExportItemDataJson(a)).ToArray())));
            data.Add(new XElement("MaxHitPoints", Stats[0]));
            data.Add(new XElement("HitPoints", HP));
            data.Add(new XElement("MaxMagicPoints", Stats[1]));
            data.Add(new XElement("MagicPoints", MP));
            data.Add(new XElement("Attack", Stats[2]));
            data.Add(new XElement("Defense", Stats[3]));
            data.Add(new XElement("Speed", Stats[4]));
            data.Add(new XElement("Dexterity", Stats[5]));
            data.Add(new XElement("HpRegen", Stats[6]));
            data.Add(new XElement("MpRegen", Stats[7]));
            data.Add(new XElement("Protection", Stats[8]));
            data.Add(new XElement("CritChance", Stats[9]));
            data.Add(new XElement("Tex1", Tex1));
            data.Add(new XElement("Tex2", Tex2));
            data.Add(new XElement("Texture", SkinType));
            return data;
        }

        public override void Export(IData Data)
        {
            Data.SetKey("Level", Level);
            Data.SetKey("Experience", Experience.ToString());
            Data.SetKey("ClassType", ClassType.ToString());
            Data.SetKey("HP", HP.ToString());
            Data.SetKey("MP", MP.ToString());
            Data.SetKey("Stats", Stats);
            Data.SetKey("Equipment", Inventory);
            Data.SetKey("ItemDatas", JsonConvert.SerializeObject(ItemDataJsons));
            Data.SetKey("Fame", Fame.ToString());
            Data.SetKey("Tex1", Tex1.ToString());
            Data.SetKey("Tex2", Tex2.ToString());
            Data.SetKey("SkinType", SkinType.ToString());
            Data.SetKey("HealthPotions", HealthPotions.ToString());
            Data.SetKey("MagicPotions", MagicPotions.ToString());
            Data.SetKey("CreationTime", CreationTime.ToString());
            Data.SetKey("Deleted", Deleted.ToString());
            Data.SetKey("Dead", Dead.ToString());
            Data.SetKey("DeathFame", DeathFame.ToString());
            Data.SetKey("DeathTime", DeathTime.ToString());
            Data.SetKey("HasBackpack", HasBackpack.ToString());
            FameStats.Export(Data);
            Data.SetKey("PetId", PetId.ToString());
            Data.SetKey("ItemDataModifier", ItemDataModifier.ToString());
            Data.SetKey("Runes", SelectedRunes);

            Data.SetKey("GlowColor", GlowColor.ToString());
            Data.SetKey("Size", Size.ToString());
        }

        public ItemDesc GetDescription(int k)
        {
            int itemInSlot = Inventory[k];
            return Resources.Type2Item[(ushort) itemInSlot];
        }
    }

    // A player account's market data
    public class MarketModel : DatabaseModel
    {
        public readonly int AccountId;
        public List<MarketPost> Posts = new List<MarketPost>();
        public int NextPostId = 0;

        public MarketModel(int accountId) : base($"market.{accountId}")
        {
            AccountId = accountId;
        }


        public override void Export(IData Data)
        {
            Data.SetKey("CurrentListings", Posts);
            Data.SetKey("NextPostId", NextPostId.ToString());
        }

        public override XElement ExportApp()
        {
            throw new NotImplementedException();
        }

        protected override void Load()
        {
            Posts = Data.GetKeyParsed<List<MarketPost>>("CurrentListings").OrElse(new List<MarketPost>());
            NextPostId = Data.GetInt("NextPostId");
        }
    }

    public class MarketPost : IDatabaseInfo
    {
        public ItemDataJson Json { get; set; } = new ItemDataJson();
        public int Item { get; set; } = -1;
        public int Price { get; set; } = 0;
        public int Id { get; set; } = 0;
        public int AccountId = 0;

        public MarketPost()
        {
        }

        public MarketPost(XElement elem)
        {
            Item = elem.ParseInt("Item");
            Price = elem.ParseInt("Price");
            Json = ItemDesc.ParseItemDataJson(elem.ParseString("ItemData"));
            Id = elem.ParseInt("Id");
            AccountId = elem.ParseInt("AccountId");
        }

        public XElement ExportApp()
        {
            XElement elem = new XElement("MarketPost");
            elem.Add(new XElement("Item", Item));
            elem.Add(new XElement("Price", Price));
            elem.Add(new XElement("ItemData", ItemDesc.ExportItemDataJson(Json)));
            elem.Add(new XElement("Id", Id));
            elem.Add(new XElement("AccountId", AccountId));
            return elem;
        }

        public void Export(IData parent)
        {
            throw new NotImplementedException();
        }
    }

    public class FameStatsInfo : IDatabaseInfo
    {
        private IData Data;

        public int Shots;
        public int ShotsThatDamage;
        public int TilesUncovered;
        public int QuestsCompleted;
        public int Escapes;
        public int NearDeathEscapes;
        public int MinutesActive;

        public int LevelUpAssists;
        public int PotionsDrank;
        public int Teleports;
        public int AbilitiesUsed;

        public int DamageTaken;
        public int DamageDealt;

        public int MonsterKills;
        public int MonsterAssists;
        public int GodKills;
        public int GodAssists;
        public int OryxKills;
        public int OryxAssists;
        public int CubeKills;
        public int CubeAssists;
        public int BlueBags;
        public int CyanBags;
        public int WhiteBags;

        public int PirateCavesCompleted;
        public int UndeadLairsCompleted;
        public int AbyssOfDemonsCompleted;
        public int SnakePitsCompleted;
        public int SpiderDensCompleted;
        public int SpriteWorldsCompleted;
        public int TombsCompleted;

        public FameStatsInfo() { }
        public FameStatsInfo(IData data)
        {
            this.Data = data;

            Shots = data.GetInt("Shots");
            ShotsThatDamage = data.GetInt("ShotsThatDamage");
            TilesUncovered = data.GetInt("TilesUncovered");
            QuestsCompleted = data.GetInt("QuestsCompleted");
            Escapes = data.GetInt("Escapes");
            NearDeathEscapes = data.GetInt("NearDeathEscapes");
            MinutesActive = data.GetInt("MinutesActive");

            LevelUpAssists = data.GetInt("LevelUpAssists");
            PotionsDrank = data.GetInt("PotionsDrank");
            Teleports = data.GetInt("Teleports");
            AbilitiesUsed = data.GetInt("AbilitiesUsed");

            DamageTaken = data.GetInt("DamageTaken");
            DamageDealt = data.GetInt("DamageDealt");

            MonsterKills = data.GetInt("MonsterKills");
            MonsterAssists = data.GetInt("MonsterAssists");
            GodKills = data.GetInt("GodKills");
            GodAssists = data.GetInt("GodAssists");
            OryxKills = data.GetInt("OryxKills");
            OryxAssists = data.GetInt("OryxAssists");
            CubeKills = data.GetInt("CubeKills");
            CubeAssists = data.GetInt("CubeAssists");
            CyanBags = data.GetInt("CyanBags");
            BlueBags = data.GetInt("BlueBags");
            WhiteBags = data.GetInt("WhiteBags");

            PirateCavesCompleted = data.GetInt("PirateCavesCompleted");
            UndeadLairsCompleted = data.GetInt("UndeadLairsCompleted");
            AbyssOfDemonsCompleted = data.GetInt("AbyssOfDemonsCompleted");
            SnakePitsCompleted = data.GetInt("SnakePitsCompleted");
            SpiderDensCompleted = data.GetInt("SpiderDensCompleted");
            SpriteWorldsCompleted = data.GetInt("SpriteWorldsCompleted");
            TombsCompleted = data.GetInt("TombsCompleted");
        }

        public XElement Export(IData data)
        {
            if (data == null)
                return null;

            data.SetKey("Shots", Shots.ToString());
            data.SetKey("ShotsThatDamage", ShotsThatDamage.ToString());
            data.SetKey("TilesUncovered", TilesUncovered.ToString());
            data.SetKey("QuestsCompleted", QuestsCompleted.ToString());
            data.SetKey("PirateCavesCompleted", PirateCavesCompleted.ToString());
            data.SetKey("UndeadLairsCompleted", UndeadLairsCompleted.ToString());
            data.SetKey("AbyssOfDemonsCompleted", AbyssOfDemonsCompleted.ToString());
            data.SetKey("SnakePitsCompleted", SnakePitsCompleted.ToString());
            data.SetKey("SpiderDensCompleted", SpiderDensCompleted.ToString());
            data.SetKey("SpriteWorldsCompleted", SpriteWorldsCompleted.ToString());
            data.SetKey("Escapes", Escapes.ToString());
            data.SetKey("NearDeathEscapes", NearDeathEscapes.ToString());
            data.SetKey("LevelUpAssists", LevelUpAssists.ToString());
            data.SetKey("DamageTaken", DamageTaken.ToString());
            data.SetKey("DamageDealt", DamageDealt.ToString());
            data.SetKey("Teleports", Teleports.ToString());
            data.SetKey("PotionsDrank", PotionsDrank.ToString());
            data.SetKey("MonsterKills", MonsterKills.ToString());
            data.SetKey("MonsterAssists", MonsterAssists.ToString());
            data.SetKey("GodKills", GodKills.ToString());
            data.SetKey("GodAssists", GodAssists.ToString());
            data.SetKey("OryxKills", OryxKills.ToString());
            data.SetKey("OryxAssists", OryxAssists.ToString());
            data.SetKey("CubeKills", CubeKills.ToString());
            data.SetKey("CubeAssists", CubeAssists.ToString());
            data.SetKey("CyanBags", CyanBags.ToString());
            data.SetKey("BlueBags", BlueBags.ToString());
            data.SetKey("WhiteBags", WhiteBags.ToString());
            data.SetKey("MinutesActive", MinutesActive.ToString());
            data.SetKey("AbilitiesUsed", AbilitiesUsed.ToString());
            return null;
        }

        public void ExportTo(XElement e)
        {
            e.Add(new XElement("Shots", Shots));
            e.Add(new XElement("ShotsThatDamage", ShotsThatDamage));
            e.Add(new XElement("TilesUncovered", TilesUncovered));
            e.Add(new XElement("QuestsCompleted", QuestsCompleted));
            e.Add(new XElement("PirateCavesCompleted", PirateCavesCompleted));
            e.Add(new XElement("UndeadLairsCompleted", UndeadLairsCompleted));
            e.Add(new XElement("AbyssOfDemonsCompleted", AbyssOfDemonsCompleted));
            e.Add(new XElement("SnakePitsCompleted", SnakePitsCompleted));
            e.Add(new XElement("SpiderDensCompleted", SpiderDensCompleted));
            e.Add(new XElement("SpriteWorldsCompleted", SpriteWorldsCompleted));
            e.Add(new XElement("Escapes", Escapes));
            e.Add(new XElement("NearDeathEscapes", NearDeathEscapes));
            e.Add(new XElement("LevelUpAssists", LevelUpAssists));
            e.Add(new XElement("DamageTaken", DamageTaken));
            e.Add(new XElement("DamageDealt", DamageDealt));
            e.Add(new XElement("Teleports", Teleports));
            e.Add(new XElement("PotionsDrank", PotionsDrank));
            e.Add(new XElement("MonsterKills", MonsterKills));
            e.Add(new XElement("MonsterAssists", MonsterAssists));
            e.Add(new XElement("GodKills", GodKills));
            e.Add(new XElement("GodAssists", GodAssists));
            e.Add(new XElement("OryxKills", OryxKills));
            e.Add(new XElement("OryxAssists", OryxAssists));
            e.Add(new XElement("CubeKills", CubeKills));
            e.Add(new XElement("CubeAssists", CubeAssists));
            e.Add(new XElement("CyanBags", CyanBags));
            e.Add(new XElement("BlueBags", BlueBags));
            e.Add(new XElement("WhiteBags", WhiteBags));
            e.Add(new XElement("MinutesActive", MinutesActive));
            e.Add(new XElement("AbilitiesUsed", AbilitiesUsed));
        }

        void IDatabaseInfo.Export(IData parent)
        {
            throw new NotImplementedException();
        }

        public XElement ExportApp()
        {
            XElement elem = new XElement("Fame");
            ExportTo(elem);
            return elem;
        }
    }

    public class GuildModel : DatabaseModel
    {
        public readonly string Name;
        
        public int Level;
        public int Fame;
        public int TotalFame;
        public List<int> Members;
        public string BoardMessage;

        public GuildModel(string name) : base($"guild.{name}")
        {
            Name = name;
        }

        protected override void Load()
        {
            Level = Data.GetInt("Level");
            Fame = Data.GetInt("Fame");
            TotalFame = Data.GetInt("TotalFame");
            Members = Data.GetKeyParsed<List<int>>("Members").OrElse(new List<int>());
            BoardMessage = Data.GetKey("BoardMessage");
        }

        public override XElement ExportApp()
        {
            var data = new XElement("Guild");
            data.Add(new XElement("TotalFame", TotalFame));
            data.Add(new XAttribute("name", Name));
            data.Add(new XElement("CurrentFame", Fame));
            data.Add(new XElement("HallType", "Guild Hall " + Level));
            foreach (var member in from i in Members
                    select new AccountModel(i) into acc
                    orderby acc.GuildRank descending, 
                            acc.GuildFame descending, 
                            acc.Name
                    select acc)
            {
                data.Add(new XElement("Member",
                    new XElement("Name", member.Name),
                    new XElement("Rank", member.GuildRank),
                    new XElement("Fame", member.GuildFame),
                    new XElement("LastSeen", member.LastSeen)));
            }
            return data;
        }

        public override void Export(IData Data)
        {
            Data.SetKey("TotalFame", TotalFame.ToString());
            Data.SetKey("Level", Level.ToString());
            Data.SetKey("Fame", Fame.ToString());
            Data.SetKey("TotalFame", TotalFame.ToString());
            Data.SetKey("Members", Members);
            Data.SetKey("BoardMessage", BoardMessage);
        }
    }

    public class AccountModel : DatabaseModel
    {
        public const int MaxDeadCharsStored = 20;

        public readonly int Id; //Taken from database.
        public readonly string Name; //Taken from database.

        public int NextCharId;
        public int MaxNumChars;
        public int VaultCount;
        public List<int> AliveChars;
        public List<int> DeadChars;
        public List<int> OwnedSkins;
        public bool Ranked;

        public int Donator;

        public bool Muted;
        public bool Banned;
        public string GuildName;
        public int GuildRank;
        public int GuildFame;
        public StatsInfo Stats;
        public bool Connected;
        public int RegisterTime;
        public int LastSeen;
        public List<int> LockedIds;
        public List<int> IgnoredIds;
        public bool AllyShots;
        public bool AllyDamage;
        public bool Effects;
        public bool Sounds;
        public bool Notifications;
        public List<int> Gifts;
        public List<ItemDataJson> GiftData;
        public int EntitySpawnCooldown;

        public AccountModel() : base(null) { }
        public AccountModel(int key) : base($"account.{key}")
        {
            Id = key;
            Name = Database.UsernameFromId(key);
        }

        public override void Save()
        {
            //might be slow, but this overrides account changes while they're online
            var client = Manager.GetClient(Id);
            if (client != null)
                client.Account = this;
            base.Save();
        }

        protected override void Load()
        {
            NextCharId = Data.GetInt("NextCharId");
            MaxNumChars = Data.GetInt("MaxNumChars", 4);
            VaultCount = Data.GetInt("VaultCount", 1);
            AliveChars = Data.GetKeyParsed<List<int>>("AliveChars").OrElse(new List<int>());
            DeadChars = Data.GetKeyParsed<List<int>>("DeadChars").OrElse(new List<int>());
            OwnedSkins = Data.GetKeyParsed<List<int>>("OwnedSkins").OrElse(new List<int>());
            Ranked = bool.Parse(Data.GetKeyOr("Ranked", "false"));
            Muted = bool.Parse(Data.GetKeyOr("Muted", "false"));
            Banned = bool.Parse(Data.GetKeyOr("Banned", "false"));
            GuildName = Data.GetKey("GuildName");
            GuildRank = Data.GetInt("GuildRank");
            GuildFame = Data.GetInt("GuildFame");
            Connected = bool.Parse(Data.GetKeyOr("Connected", "false"));
            RegisterTime = Data.GetInt("RegisterTime");
            LastSeen = Data.GetInt("LastSeen");
            LockedIds = Data.GetKeyParsed<List<int>>("LockedIds").OrElse(new List<int>());
            IgnoredIds = Data.GetKeyParsed<List<int>>("IgnoredIds").OrElse(new List<int>());
            AllyShots = Data.GetBool("AllyShots");
            AllyDamage = Data.GetBool("AllyDamage", true);
            Effects = Data.GetBool("Effects", true);
            Sounds = Data.GetBool("Sounds", true);
            Notifications = bool.Parse(Data.GetKeyOr("Notifications", "true"));
            Gifts = Data.GetKeyParsed<List<int>>("Gifts").OrElse(new List<int>());
            var giftDataList = Data.GetKeyParsed<List<ItemDataJson>>("GiftData").OrElse(new List<ItemDataJson>());
            while (giftDataList.Count < Gifts.Count)
            {
                giftDataList.Add(new ItemDataJson());
            }
            GiftData = giftDataList;
            EntitySpawnCooldown = Data.GetInt("EntitySpawnCooldown", 0);

            Donator = Data.GetInt("Donator");

            Stats = new StatsInfo
            {
                BestCharFame = Data.Element("Stats").GetInt("BestCharFame"),
                TotalFame = Data.Element("Stats").GetInt("TotalFame"),
                Fame = Data.Element("Stats").GetInt("Fame"),
                TotalCredits = Data.Element("Stats").GetInt("TotalCredits"),
                Credits = Data.Element("Stats").GetInt("Credits")
            };

            var classStats = new List<ClassStatsInfo>();
            foreach (var e in Data.Element("Stats").Elements("ClassStats"))
            {
                classStats.Add(new ClassStatsInfo
                {
                    ObjectType = e.GetInt("@objectType"),
                    BestFame = e.GetInt("BestFame"),
                    BestLevel = e.GetInt("BestLevel")
                });
            }
            Stats.ClassStats = classStats.ToArray();
        }

        public override XElement ExportApp()
        {
            var data = new XElement("Account");
            data.Add(new XElement("AccountId", Id));
            data.Add(new XElement("Name", Name));
            data.Add(new XElement("Guild", 
                new XElement("Name", GuildName), 
                new XElement("Rank", GuildRank)));
            data.Add(Stats.ExportApp());
            return data;
        }

        public override void Export(IData Data)
        {
            Data.SetKey("AccountId", Id);
            Data.SetKey("AliveChars", AliveChars);
            Data.SetKey("DeadChars", DeadChars);
            Data.SetKey("OwnedSkins", OwnedSkins);
            Data.SetKey("Ranked", Ranked.ToString());
            Data.SetKey("Banned", Banned.ToString());
            Data.SetKey("Connected", Connected.ToString());
            Data.SetKey("NextCharId", NextCharId.ToString());
            Data.SetKey("MaxNumChars", MaxNumChars.ToString());
            Data.SetKey("VaultCount", VaultCount.ToString());
            Data.SetKey("GuildName", GuildName);
            Data.SetKey("GuildRank", GuildRank.ToString());
            Data.SetKey("RegisterTime", RegisterTime.ToString());
            Data.SetKey("LastSeen", LastSeen.ToString());
            Data.SetKey("LockedIds", LockedIds);
            Data.SetKey("IgnoredIds", IgnoredIds);
            Data.SetKey("AllyShots", AllyShots.ToString());
            Data.SetKey("AllyDamage", AllyDamage.ToString());
            Data.SetKey("Effects", Effects.ToString());
            Data.SetKey("Sounds", Sounds.ToString());
            Data.SetKey("Notifications", Notifications.ToString());
            Data.SetKey("Gifts", Gifts);
            Data.SetKey("GiftData", GiftData);
            Data.SetKey("Donator", Donator);
            Data.SetKey("EntitySpawnCooldown", EntitySpawnCooldown.ToString());

            Stats.Export(Data.Element("Stats"));
        }
    }

    public class ClassStatsInfo : IDatabaseInfo
    {
        public int ObjectType;
        public int BestLevel;
        public int BestFame;

        public void Export(IData parent)
        {
            var child = parent.Child($"ClassStats.{ObjectType.ToString()}");

            child.SetKey("objectType", ObjectType.ToString());
            child.SetKey("BestLevel", BestLevel.ToString());
            child.SetKey("BestFame", BestFame.ToString());
        }

        public XElement ExportApp()
        {
            var data = new XElement("ClassStats");
            data.Add(new XAttribute("objectType", ObjectType));
            data.Add(new XElement("BestLevel", BestLevel));
            data.Add(new XElement("BestFame", BestFame));
            return data;
        }
    }

    public class StatsInfo : IDatabaseInfo
    {
        public int BestCharFame;
        public int TotalFame;
        public int Fame;
        public int Credits;
        public int TotalCredits;
        public ClassStatsInfo[] ClassStats;

        public void Export(IData parent)
        {
            parent.SetKey("BestCharFame", BestCharFame.ToString());
            parent.SetKey("TotalFame", TotalFame.ToString());
            parent.SetKey("Fame", Fame.ToString());
            parent.SetKey("TotalCredits", TotalCredits.ToString());
            parent.SetKey("Credits", Credits.ToString());
        }

        public XElement ExportApp()
        {
            var data = new XElement("Stats");
            data.Add(new XElement("BestCharFame", BestCharFame));
            data.Add(new XElement("TotalFame", TotalFame));
            data.Add(new XElement("Fame", Fame));
            data.Add(new XElement("TotalCredits", TotalCredits));
            data.Add(new XElement("Credits", Credits));
            foreach (var k in ClassStats)
                data.Add(k.ExportApp());
            return data;
        }

        public ClassStatsInfo GetClassStats(int type)
        {
            foreach (var s in ClassStats)
                if (s.ObjectType == type)
                    return s;
            return null;
        }
    }
}
