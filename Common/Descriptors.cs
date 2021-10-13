using RotMG.Game;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using static RotMG.Game.Logic.LootDef;
using System.Globalization;

namespace RotMG.Common
{
    public enum Currency
    {
        Gold,
        Fame,
        GuildFame
    }
    
    public enum ItemData : ulong
    {
        //Tiers
        T0 = 1 << 0,
        T1 = 1 << 1,
        T2 = 1 << 2,
        T3 = 1 << 3,
        T4 = 1 << 4,
        T5 = 1 << 5,
        T6 = 1 << 6,
        T7 = 1 << 7,

        //Bonuses
        MaxHP = 1 << 8,
        MaxMP = 1 << 9,
        Attack = 1 << 10,
        Defense = 1 << 11,
        Speed = 1 << 12,
        Dexterity = 1 << 13,
        Vitality = 1 << 14,
        Wisdom = 1 << 15,
        RateOfFire = 1 << 16,
        Damage = 1 << 17,
        Cooldown = 1 << 18,
        FameBonus = 1 << 19
    }

    [Flags]
    public enum ConditionEffects : ulong
    {
        Nothing = 1 << 0,
        Quiet = 1 << 1,
        Weak = 1 << 2,
        Slowed = 1 << 3,
        Sick = 1 << 4,
        Dazed = 1 << 5,
        Stunned = 1 << 6,
        Blind = 1 << 7,
        Hallucinating = 1 << 8,
        Drunk = 1 << 9,
        Confused = 1 << 10,
        StunImmume = 1 << 11,
        Invisible = 1 << 12,
        Paralyzed = 1 << 13,
        Speedy = 1 << 14,
        Bleeding = 1 << 15,
        Healing = 1 << 16,
        Damaging = 1 << 17,
        Berserk = 1 << 18,
        Stasis = 1 << 19,
        StasisImmune = 1 << 20,
        Invincible = 1 << 21,
        Invulnerable = 1 << 23,
        Armored = 1 << 24,
        ArmorBroken = 1 << 25,
        Hexed = 1 << 26,
        NinjaSpeedy = 1 << 27,
    }

    public enum ConditionEffectIndex
    {
        Nothing = 0,
        Quiet = 1,
        Weak = 2,
        Slowed = 3,
        Sick = 4,
        Dazed = 5,
        Stunned = 6,
        Blind = 7,
        Hallucinating = 8,
        Drunk = 9,
        Confused = 10,
        StunImmune = 11,
        Invisible = 12,
        Paralyzed = 13,
        Speedy = 14,
        Bleeding = 15,
        Healing = 16,
        Damaging = 17,
        Berserk = 18,
        Stasis = 19,
        StasisImmune = 20,
        Invincible = 21,
        Invulnerable = 22,
        Armored = 23,
        ArmorBroken = 24,
        Hexed = 25,
        Cursed = 26,
    }

    public enum ActivateEffectIndex
    {
        Create,
        Dye,
        Shoot,
        IncrementStat,
        Heal,
        Magic,
        HealNova,
        StatBoostSelf,
        StatBoostAura,
        BulletNova,
        ConditionEffectSelf,
        ConditionEffectAura,
        Teleport,
        PoisonGrenade,
        VampireBlast,
        Trap,
        StasisBlast,
        Pet,
        Decoy,
        Lightning,
        UnlockPortal,
        MagicNova,
        ClearConditionEffectAura,
        RemoveNegativeConditions,
        ClearConditionEffectSelf,
        ClearConditionsEffectSelf,
        RemoveNegativeConditionsSelf,
        Shuriken,
        DazeBlast,
        Backpack,
        PermaPet,
        ItemDataModifier,
        ConditionEffectBlast,
        RuneConsume,
        UnlockSkin,
        RemoveFromBag
    }

    public enum ShowEffectIndex
    {
        Unknown = 0,
        Heal = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        Nova = 5,
        Poison = 6,
        Line = 7,
        Burst = 8,
        Flow = 9,
        Ring = 10,
        Lightning = 11,
        Collapse = 12,
        Coneblast = 13,
        Jitter = 14,
        Flash = 15,
        ThrowProjectile = 16
    }
    
    public enum ItemType : byte
    {
        All,
        Sword,
        Dagger,
        Bow,
        Tome,
        Shield,
        Leather,
        Plate,
        Wand,
        Ring,
        Potion,
        Spell,
        Seal,
        Cloak,
        Robe,
        Quiver,
        Helm,
        Staff,
        Poison,
        Skull,
        Trap,
        Orb,
        Prism,
        Scepter,
        Katana,
        Shuriken,
    }

    public class ObjectDesc
    {
        public readonly string Id;
        public readonly string Group;
        public readonly ushort Type;

        public readonly string DisplayId;

        public readonly bool Static;
        public readonly bool Friendly;
        public readonly string Class;
        public readonly bool BlocksSight;

        public readonly bool OccupySquare;
        public readonly bool FullOccupy;
        public readonly bool EnemyOccupySquare;

        public readonly bool ProtectFromGroundDamage;
        public readonly bool ProtectFromSink;

        public readonly bool Player;
        public readonly bool Enemy;

        public readonly bool God;
        public readonly bool Cube;
        public readonly bool Quest;
        public readonly bool Hero;
        public readonly int Level;
        public readonly bool Oryx;
        public readonly float XpMult;

        public readonly int Size;
        public readonly int MinSize;
        public readonly int MaxSize;

        public readonly int MaxHp;
        public readonly int Defense;

        public readonly string DungeonName;
        public readonly bool LeavePortal;

        public readonly SpawnData SpawnData;
        public readonly int PerRealmMax;

        public readonly Dictionary<int, ProjectileDesc> Projectiles;

        public ObjectDesc(XElement e, string id, ushort type)
        {
            Id = id;
            Type = type;

            DisplayId = e.ParseString("DisplayId", Id);
            Group = e.ParseString("Group");

            Static = e.ParseBool("Static");
            Friendly = e.ParseBool("Friendly");
            Class = e.ParseString("Class");
            BlocksSight = e.ParseBool("BlocksSight");

            OccupySquare = e.ParseBool("OccupySquare");
            LeavePortal = e.ParseBool("LeavePortal");
            FullOccupy = e.ParseBool("FullOccupy");
            EnemyOccupySquare = e.ParseBool("EnemyOccupySquare");

            ProtectFromGroundDamage = e.ParseBool("ProtectFromGroundDamage");
            ProtectFromSink = e.ParseBool("ProtectFromSink");

            Enemy = e.ParseBool("Enemy");
            Player = e.ParseBool("Player");

            God = e.ParseBool("God");
            Cube = e.ParseBool("Cube");
            Quest = e.ParseBool("Quest");
            Hero = e.ParseBool("Hero");
            Level = e.ParseInt("Level", -1);
            Oryx = e.ParseBool("Oryx");
            XpMult = e.ParseFloat("XpMult", 1);

            Size = e.ParseInt("Size", 100);
            MinSize = e.ParseInt("MinSize", Size);
            MaxSize = e.ParseInt("MaxSize", Size);

            MaxHp = e.ParseInt("MaxHitPoints");
            Defense = e.ParseInt("Defense");

            DungeonName = e.ParseString("DungeonName");

            if (e.Element("Spawn") != null)
                SpawnData = new SpawnData(e.Element("Spawn"));
            PerRealmMax = e.ParseInt("PerRealmMax");

            Projectiles = new Dictionary<int, ProjectileDesc>();
            foreach (var k in e.Elements("Projectile"))
            {
                var desc = new ProjectileDesc(k, Type);
#if DEBUG
                if (Projectiles.ContainsKey(desc.BulletType))
                    throw new Exception("Duplicate bullet type");
#endif
                Projectiles[desc.BulletType] = desc;
            }
        }
    }
    
    public class SpawnData
    {
        public readonly int Mean;
        public readonly int StdDev;
        public readonly int Min;
        public readonly int Max;

        public SpawnData(XElement e)
        {
            Mean = e.ParseInt("Mean");
            StdDev = e.ParseInt("StdDev");
            Min = e.ParseInt("Min");
            Max = e.ParseInt("Max");
        }
    }

    public class PlayerDesc : ObjectDesc
    {
        public readonly ItemType[] SlotTypes;
        public readonly int[] Equipment;
        public readonly string[] ItemDatas;
        public readonly StatDesc[] Stats;
        public readonly int[] StartingValues;

        public PlayerDesc(XElement e, string id, ushort type) : base(e, id, type)
        {
            SlotTypes = e.ParseIntArray("SlotTypes", ",").Select(x => (ItemType)x).ToArray();

            var equipment = e.ParseUshortArray("Equipment", ",").Select(k => k == 0xffff ? -1 : k).ToArray();
            Equipment = new int[20];
            for (var k = 0; k < 20; k++)
                Equipment[k] = k >= equipment.Length ? -1 : equipment[k];

            ItemDatas = new string[20];
            for (var k = 0; k < 20; k++)
                ItemDatas[k] = "{}";

            Stats = new StatDesc[8];
            for (var i = 0; i < Stats.Length; i++)
                Stats[i] = new StatDesc(i, e);
            Stats = Stats.OrderBy(k => k.Index).ToArray();

            StartingValues = Stats.Select(k => k.StartingValue).ToArray();
        }
    }

    public class StatDesc
    {
        public readonly string Type;
        public readonly int Index;
        public readonly int MaxValue;
        public readonly int StartingValue;
        public readonly int MinIncrease;
        public readonly int MaxIncrease;

        public StatDesc(int index, XElement e)
        {
            Index = index;
            Type = StatIndexToName(index);

            StartingValue = e.ParseInt(Type);
            MaxValue = e.Element(Type).ParseInt("@max");

            foreach (var stat in e.Elements("LevelIncrease"))
            {
                if (stat.Value == Type)
                {
                    MinIncrease = stat.ParseInt("@min");
                    MaxIncrease = stat.ParseInt("@max");
                    break;
                }
            }
        }

        public static string StatIndexToName(int index)
        {
            switch (index)
            {
                case 0: return "MaxHitPoints";
                case 1: return "MaxMagicPoints";
                case 2: return "Attack";
                case 3: return "Defense";
                case 4: return "Speed";
                case 5: return "Dexterity";
                case 6: return "HpRegen";
                case 7: return "MpRegen";
            }
            return null;
        }

        public static int StatNameToIndex(string name)
        {
            switch (name)
            {
                case "MaxHitPoints": return 0;
                case "MaxMagicPoints": return 1;
                case "Attack": return 2;
                case "Defense": return 3;
                case "Speed": return 4;
                case "Dexterity": return 5;
                case "HpRegen": return 6;
                case "MpRegen": return 7;
            }
            return -1;
        }
    }

    public class SkinDesc
    {
        public readonly string Id;
        public readonly ushort Type;

        public readonly ushort PlayerClassType;

        public SkinDesc(XElement e, string id, ushort type)
        {
            Id = id;
            Type = type;
            PlayerClassType = e.ParseUshort("PlayerClassType");
        }
    }

    public class ActivateEffectDesc
    {
        public readonly ActivateEffectIndex Index;
        public readonly ConditionEffectDesc[] Effects;
        public readonly ConditionEffectIndex Effect;
        public readonly string Id;
        //Map name valid for use here
        public readonly string Map;
        public readonly int DurationMS;
        public readonly int ThrowtimeMS;
        public readonly float Range;
        public readonly int Amount;
        public readonly int TotalDamage;
        public readonly float Radius;
        public readonly uint? Color;
        public readonly int MaxTargets;
        public readonly int Stat;

        public readonly bool UseWisMod = false;

        public readonly string StatForScale;

        public readonly float StatScale;
        public readonly float StatDurationScale;
        public readonly float StatRangeScale;

        //Position to teleport to for cracked prisms
        public readonly Vector2 Position;

        public readonly bool TargetCursor;

        public readonly int StatMin;
        
        public ActivateEffectDesc(XElement e)
        {
            Index = (ActivateEffectIndex)Enum.Parse(typeof(ActivateEffectIndex), e.Value.Replace(" ", ""));
            Id = e.ParseString("@id");
            Effect = e.ParseConditionEffect("@effect");
            DurationMS = (int)(e.ParseFloat("@duration") * 1000);
            //Needs multiple of 100
            ThrowtimeMS = (int)(e.ParseFloat("@throwTime", .8f) * 1000);
            Range = e.ParseFloat("@range");
            Amount = e.ParseInt("@amount");
            TotalDamage = e.ParseInt("@totalDamage");
            Radius = e.ParseFloat("@radius");
            MaxTargets = e.ParseInt("@maxTargets");
            Stat = e.ParseInt("@stat", -1);

            StatMin = e.ParseInt("@statMin", 0);
            StatScale = e.ParseFloat("@statScale", 0.0f);
            StatDurationScale = e.ParseFloat("@statDurationScale", 0.0f);
            StatRangeScale = e.ParseFloat("@statRangeScale", 0.0f);
            UseWisMod = e.ParseBool("useWisMod", false);
            TargetCursor = e.ParseBool("@targetCursor", false);
            StatForScale = e.ParseString("@statForScale", "Wisdom");

            Position = new Vector2(e.ParseInt("@posx", 0), e.ParseInt("@posy", 0));
            Map = e.ParseString("@map", null);

            Effects = new ConditionEffectDesc[1]
            {
                new ConditionEffectDesc(Effect, DurationMS)
            };

            if (e.Attribute("color") != null)
                Color = e.ParseUInt("@color");
        }
    }
    
    public class ItemDataJson
    {

        public int Meta { get; set; } = -1;
        public Dictionary<ulong, int> ExtraStatBonuses = new Dictionary<ulong, int>();
        public string ItemComponent = null;
        public string SkinId = null;

        public List<int> StoredItems = null;
        public List<int> AllowedItems = null;
        
        public int GetStatBonus(ItemData k)
        {
            if (ExtraStatBonuses == null) ExtraStatBonuses = new Dictionary<ulong, int>();
            return ExtraStatBonuses.ContainsKey((ulong)k) 
                ? ExtraStatBonuses[(ulong)k] 
                : 0;
        }

    }

    public enum ItemDataModType {
        Classical, Strength, Wisdom, Agile,
        Insanity
    }

    public interface IItemDataModifier
    {
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta);
    }

    struct ClassicalDataGenerator : IItemDataModifier
    {
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta)
        {
            return new Dictionary<ItemData, int>();
        }
    }

    struct StrengthDataGenerator : IItemDataModifier
    {
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta)
        {
            //atleast 1
            var rank = ItemDesc.GetRank(meta.Meta);
            return new Dictionary<ItemData, int>()
            {
                [ItemData.Attack] = MathUtils.NextInt(rank, 3 * rank),
                [ItemData.MaxHP] = MathUtils.NextInt(2),
                [ItemData.Dexterity] = -1 * MathUtils.NextInt(rank, 3*rank)
            };
        }
    }

    struct WisdomDataGenerator : IItemDataModifier
    {
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta)
        {
            var rank = ItemDesc.GetRank(meta.Meta);
            return new Dictionary<ItemData, int>()
            {
                [ItemData.Wisdom] = MathUtils.NextInt(rank, 3 + (int)(1.25f * rank)),
                [ItemData.MaxHP] = -1 * MathUtils.NextInt(rank * 3)
            };
        }
    }

    struct AgileDataGenerator : IItemDataModifier
    {
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta)
        {
            var rank = ItemDesc.GetRank(meta.Meta);
            return new Dictionary<ItemData, int>()
            {
                [ItemData.Attack] = -1 * MathUtils.NextInt(rank, 3 + rank),
                [ItemData.Dexterity] = MathUtils.NextInt(rank, 3 + (int)(1.25f * rank)),
                [ItemData.Speed] = MathUtils.NextInt(rank, 3 * rank),
                [ItemData.RateOfFire] = MathUtils.NextInt(rank),
            };
        }
    }

    struct InsanityDataGenerator : IItemDataModifier
    {
        static ItemData[] RandomChoices = new ItemData[]
        {

            ItemData.Attack, ItemData.Defense, ItemData.Speed, ItemData.Dexterity,
            ItemData.Vitality, ItemData.Wisdom

        };
        public Dictionary<ItemData, int> GenerateStats(ref ItemDataJson meta)
        {
            var rank = ItemDesc.GetRank(meta.Meta);
            var d = new Dictionary<ItemData, int>();
            var points = 0;
            HashSet<ItemData> chosen = new HashSet<ItemData>();
            for(int i = 0; i < MathUtils.NextInt(2, 4); i++)
            {
                ItemData idc = ItemData.T0;
                while(idc == ItemData.T0 || chosen.Contains(idc))
                {
                    idc = RandomChoices[MathUtils.NextInt(RandomChoices.Length)];
                }
                d[idc] = MathUtils.NextInt(MathUtils.PlusMinus() * 2 * rank + points);
                points -= d[idc];
                chosen.Add(idc);
            }
            Console.WriteLine(d);
            return d;
        }
    }

    public class ItemDataModifiers
    {
        public static Dictionary<ItemDataModType, IItemDataModifier> Registry = new Dictionary<ItemDataModType, IItemDataModifier>();
        static ItemDataModifiers()
        {
            Registry[ItemDataModType.Classical] = new ClassicalDataGenerator();
            Registry[ItemDataModType.Strength] = new StrengthDataGenerator();
            Registry[ItemDataModType.Wisdom] = new WisdomDataGenerator();
            Registry[ItemDataModType.Agile] = new AgileDataGenerator();
            Registry[ItemDataModType.Insanity] = new InsanityDataGenerator();
        }
    }

    public class SetpieceActivator
    {
        public int stat;
        public int amount;
        ActivateEffectIndex index;

        public SetpieceActivator(XElement xml)
        {
            stat = xml.ParseInt("@stat");
            amount = xml.ParseInt("@amount");
            index = (ActivateEffectIndex)Enum.Parse(typeof(ActivateEffectIndex), xml.Value.Replace(" ", ""));
        }

    }

    public class EquipmentSet
    {
        public HashSet<int> Setpieces = new HashSet<int>();
        public List<SetpieceActivator> ActivationEffects;

        public EquipmentSet(XElement xml)
        {
            ActivationEffects = new List<SetpieceActivator>();
            foreach(var spiece in xml.Elements("ActivateOnEquipAll"))
            {
                ActivationEffects.Add(new SetpieceActivator(spiece));
            }

            Setpieces = new HashSet<int>();
            foreach(var spiece in xml.Elements("Setpiece"))
            {
                Setpieces.Add(Int32.Parse(spiece.ParseString("@itemtype").Split("x")[1], NumberStyles.HexNumber));
            }
        }

    }

    public class ItemDesc
    {
        static ItemDesc() {

            JsonConvert.DefaultSettings = () => { 
                var settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
                
                return settings; 
            };

        }

        public static ItemDataJson ParseOrDefault(string p)
        {
            try
            {
                var parsed = ParseItemDataJson(p);
                return parsed;
            } catch
            {
                return new ItemDataJson() { Meta = -1 };
            }
        }

        public static ItemDataJson ParseItemDataJson(string p)
        {
            if (p == null) return new ItemDataJson() { Meta = -1 };
            return JsonConvert.DeserializeObject<ItemDataJson>(p);
        }

        public static string ExportItemDataJson(ItemDataJson j)
        {
            if (j == null) return "{\"Meta\": -1}";
            return JsonConvert.SerializeObject(j);
        }


        public const float RateOfFireMultiplier = 0.05f;
        public const float DamageMultiplier = 0.05f;
        public const float CooldownMultiplier = 0.05f;

        static ItemData[] GlobalModifiers =
        {
            ItemData.MaxHP, 
            ItemData.MaxMP, 
            ItemData.Attack, 
            ItemData.Defense, 
            ItemData.Speed, 
            ItemData.Dexterity, 
            ItemData.Vitality, 
            ItemData.Wisdom
        };

        static ItemData[] AbilityModifiers = GlobalModifiers.Concat(
            new []
            {
                ItemData.Cooldown, 
                ItemData.Damage, 
            }).ToArray();

        static ItemData[] WeaponModifiers = GlobalModifiers.Concat(
            new []
            {
                ItemData.RateOfFire, 
                ItemData.Damage, 
            }).ToArray();

        public static ItemType[] WeaponTypes =
        {
            ItemType.Sword,
            ItemType.Dagger,
            ItemType.Staff,
            ItemType.Wand,
            ItemType.Katana,
            ItemType.Bow
        };

        public static ItemType[] ArmorTypes =
        {
            ItemType.Robe,
            ItemType.Plate,
            ItemType.Leather
        };

        public static ItemType[] RingTypes =
        {
            ItemType.Ring,
        };

        public static ItemType[] AbilityTypes =
        {
            ItemType.Cloak,
            ItemType.Spell,
            ItemType.Tome,
            ItemType.Helm,
            ItemType.Quiver,
            ItemType.Seal,
            ItemType.Poison,
            ItemType.Skull,
            ItemType.Shield,
            ItemType.Trap,
            ItemType.Orb,
            ItemType.Shuriken,
            ItemType.Prism,
            ItemType.Scepter
        };

        static ItemType[] ModifiableTypes = WeaponTypes.Concat(ArmorTypes).Concat(RingTypes).Concat(AbilityTypes).ToArray();

        public static float GetStat(ItemDataJson data, ItemData i, float multiplier)
        {
            var rank = GetRank(data.Meta);
            if (rank == -1)
                return 0;
            var value = 0;
            if (HasStat(data.Meta, i))
            {
                value += rank;
            }
            return (value * multiplier) + (multiplier * data.GetStatBonus(i));
        }

        public static int GetRank(int data)
        {
            if (data == -1)
                return -1;
            if (HasStat(data, ItemData.T0))
                return 1;
            if (HasStat(data, ItemData.T1))
                return 2;
            if (HasStat(data, ItemData.T2))
                return 3;
            if (HasStat(data, ItemData.T3))
                return 4;
            if (HasStat(data, ItemData.T4))
                return 5;
            if (HasStat(data, ItemData.T5))
                return 6;
            if (HasStat(data, ItemData.T6))
                return 7;
            if (HasStat(data, ItemData.T7))
                return 8;
            return -1;
        }

        public static bool HasStat(int data, ItemData i)
        {
            if (data == -1)
                return false;
            return ((ItemData)data & i) != 0;
        }

        public ItemDataJson FinalizeItemData(ItemDataModType? smod, ItemData data)
        {
            ItemDataJson j = new ItemDataJson() { Meta = (int)data };
            if (smod.HasValue)
            {
                var d = ItemDataModifiers.Registry[smod.Value].GenerateStats(ref j);
                j.ExtraStatBonuses = new Dictionary<ulong, int>(d.Select(a => 
                    new KeyValuePair<ulong, int>((ulong)a.Key, a.Value)
                ).ToList());
            }
            return j;
        }

        public Tuple<bool, ItemDataJson> Roll(RarityModifiedData r=null, ItemDataModType? smod=ItemDataModType.Classical)
        {
            r = r ?? new RarityModifiedData();
            ItemData data = 0;
            if (!ModifiableTypes.Contains(SlotType))
                return Tuple.Create(false, FinalizeItemData(smod, data));

            if (!MathUtils.Chance(.5f) && !r.AlwaysRare)
                return Tuple.Create(false, FinalizeItemData(smod, data));

            var rank = -1;
            var chance = .5f * r.RarityMod;
            for (var i = 0; i < 8 - r.RarityShift; i++)
            {
                if (MathUtils.Chance(chance) && rank < 8)
                    rank++;
                else break;
            }
            if (rank == -1 && !r.AlwaysRare) 
                return Tuple.Create(false, FinalizeItemData(smod, data));
            //Considering the -1 rank
            rank = Math.Min(7, rank + r.RarityShift + (r.AlwaysRare ? 1 : 0));

            data |= (ItemData)((ulong)1 << rank);

            var modifiers = GlobalModifiers;
            if (WeaponTypes.Contains(SlotType))
                modifiers = WeaponModifiers;
            else if (AbilityTypes.Contains(SlotType))
                modifiers = AbilityModifiers;

            var bonuses = MathUtils.NextInt(2, 3);
            if ((data & ItemData.T7) != 0) //T7s can have 4 bonuses
                if (MathUtils.Chance(0.5f))
                    bonuses++;

            var s = new List<ItemData>();
            while (s.Count < bonuses)
            {
                var k = modifiers[MathUtils.Next(modifiers.Length)];
                if (s.Contains(k))
                    continue;
                if (k == ItemData.Damage 
                    && !HasProjectile)
                    continue;
                s.Add(k);
                data |= k;
            }

            return Tuple.Create(true, FinalizeItemData(smod, data));
        }

        public readonly string Id;
        public readonly ushort Type;

        public readonly ItemType SlotType;
        public readonly int Tier;
        public readonly string Description;
        public readonly float RateOfFire;
        public readonly bool Usable;
        public readonly int BagType;
        public readonly int MpCost;
        public readonly int FameBonus;
        public readonly int NumProjectiles;
        public readonly float ArcGap;
        public readonly bool Consumable;
        public readonly bool Potion;
        public readonly string DisplayId;
        public readonly string SuccessorId;
        public readonly bool Soulbound;
        public readonly int CooldownMS;
        public readonly bool Resurrects;
        public readonly int Tex1;
        public readonly int Tex2;
        public readonly int Doses;
        //The string identifier for the unique effect, look at ItemHandlerRegistry
        public readonly string UniqueEffect;
        //The component identifier for mixing items, look at ComponentFactory, this is for recipes
        public readonly string Component;
        //This XML tag indicates that this item is used in the item storage itemdata mechanic
        // And specifies how many items it can store
        public readonly int StorageMax;

        public readonly KeyValuePair<int, int>[] StatBoosts;
        public readonly ActivateEffectDesc[] ActivateEffects;

        public readonly ProjectileDesc[] Projectile;
        public bool HasProjectile { get { return Projectile.Length > 0; } }
        public ProjectileDesc FirstProjectile { get { return HasProjectile ? Projectile[0] : null; } }

        public readonly bool DoBurst;

        public readonly int BurstCount;
        public readonly int BurstDelay;

        public ProjectileDesc NextProjectile(int id)
        {
            if (!HasProjectile) return null;
            var m = Projectile.Length;
            var r = id % m;
            var slot = r < 0 ? r + m : r;
            return Projectile[slot];
        }

        public ItemDesc(XElement e, string id, ushort type)
        {
            Id = id;
            Type = type;

            SlotType = (ItemType)e.ParseInt("SlotType");
            Tier = e.ParseInt("Tier", -1);
            Description = e.ParseString("Description");
            RateOfFire = e.ParseFloat("RateOfFire", 1);
            Usable = e.ParseBool("Usable");
            StorageMax = e.ParseInt("StorageMax", -1);
            BagType = e.ParseInt("BagType");
            MpCost = e.ParseInt("MpCost");
            FameBonus = e.ParseInt("FameBonus");
            NumProjectiles = e.ParseInt("NumProjectiles", 1);
            ArcGap = e.ParseFloat("ArcGap", 11.25f);
            Consumable = e.ParseBool("Consumable");
            Potion = e.ParseBool("Potion");
            DisplayId = e.ParseString("DisplayId", Id);
            Doses = e.ParseInt("Doses");
            SuccessorId = e.ParseString("SuccessorId");
            Soulbound = e.ParseBool("Soulbound");
            CooldownMS = (int)(e.ParseFloat("Cooldown", .2f) * 1000);
            Resurrects = e.ParseBool("Resurrects");
            UniqueEffect = e.ParseString("UniqueEffect", null);
            Component = e.ParseString("Component", null);
            Tex1 = (int)e.ParseUInt("Tex1");
            Tex2 = (int)e.ParseUInt("Tex2");

            var stats = new List<KeyValuePair<int, int>>();
            foreach (var s in e.Elements("ActivateOnEquip"))
                stats.Add(new KeyValuePair<int, int>(
                    s.ParseInt("@stat"),
                    s.ParseInt("@amount")));
            StatBoosts = stats.ToArray();

            var activate = new List<ActivateEffectDesc>();
            foreach (var i in e.Elements("Activate"))
                activate.Add(new ActivateEffectDesc(i));
            ActivateEffects = activate.ToArray();

            var pXMLs = e.Elements("Projectile");
            if (pXMLs.Count() > 0) 
            {
                Projectile = pXMLs.Select(a => new ProjectileDesc(a, Type)).ToArray();
            } else
            {
                Projectile = new ProjectileDesc[] { };
            }

            BurstCount = e.ParseInt("Burst");
            DoBurst = BurstCount > 0;
            BurstDelay = e.ParseInt("BurstCooldown");
        }
    }

    public class TileDesc
    {
        public readonly string Id;
        public readonly ushort Type;
        public readonly bool NoWalk;
        public readonly int Damage;
        public readonly float Speed;
        public readonly bool Sinking;
        public readonly bool Push;
        public readonly float DX;
        public readonly float DY;

        public TileDesc(XElement e, string id, ushort type)
        {
            Id = id;
            Type = type;
            NoWalk = e.ParseBool("NoWalk");
            Damage = e.ParseInt("Damage");
            Speed = e.ParseFloat("Speed", 1.0f);
            Sinking = e.ParseBool("Sinking");
            if (Push = e.ParseBool("Push"))
            {
                DX = e.Element("Animate").ParseFloat("@dx") / 1000f;
                DY = e.Element("Animate").ParseFloat("@dy") / 1000f;
            }
        }
    }

    public class ProjectileDesc
    {
        public readonly byte BulletType;
        public readonly string ObjectId;
        public readonly int LifetimeMS;
        public readonly float Speed;

        public readonly int Damage;
        public readonly int MinDamage; //Only for players
        public readonly int MaxDamage;

        public readonly ConditionEffectDesc[] Effects;

        public readonly bool MultiHit;
        public readonly bool PassesCover;
        public readonly bool ArmorPiercing;
        public readonly bool Wavy;
        public readonly bool Parametric;
        public readonly bool Boomerang;

        public readonly float Amplitude;
        public readonly float Frequency;
        public readonly float Magnitude;

        public readonly bool DoAccelerate;
        public readonly float Accelerate;
        public readonly float AccelerateDelay;
        public readonly float SpeedClamp;

        public readonly ushort ContainerType;

        public ProjectileDesc(XElement e, ushort containerType)
        {
            ContainerType = containerType;
            BulletType = (byte)e.ParseInt("@id");
            ObjectId = e.ParseString("ObjectId");
            LifetimeMS = e.ParseInt("LifetimeMS");
            Speed = e.ParseFloat("Speed");
            Damage = e.ParseInt("Damage");
            MinDamage = e.ParseInt("MinDamage", Damage);
            MaxDamage = e.ParseInt("MaxDamage", Damage);

            var effects = new List<ConditionEffectDesc>();
            foreach (var k in e.Elements("ConditionEffect"))
                effects.Add(new ConditionEffectDesc(k));
            Effects = effects.ToArray();

            MultiHit = e.ParseBool("MultiHit");
            PassesCover = e.ParseBool("PassesCover");
            ArmorPiercing = e.ParseBool("ArmorPiercing");
            Wavy = e.ParseBool("Wavy");
            Parametric = e.ParseBool("Parametric");
            Boomerang = e.ParseBool("Boomerang");

            Amplitude = e.ParseFloat("Amplitude");
            Frequency = e.ParseFloat("Frequency", 1);
            Magnitude = e.ParseFloat("Magnitude", 3);

            Accelerate = e.ParseFloat("Accelerate");
            DoAccelerate = Accelerate > 0.0f;
            AccelerateDelay = e.ParseFloat("AccelerateDelay");
            SpeedClamp = e.ParseFloat("SpeedClamp");

        }
    }

    public class ConditionEffectDesc
    {
        public readonly ConditionEffectIndex Effect;
        public readonly int DurationMS;

        public ConditionEffectDesc(ConditionEffectIndex effect, int durationMs)
        {
            Effect = effect;
            DurationMS = durationMs;
        }

        public ConditionEffectDesc(XElement e)
        {
            Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), e.Value.Replace(" ", ""));
            DurationMS = (int)(e.ParseFloat("@duration") * 1000);
        }
    }

    public class QuestDesc
    {
        public readonly int Level;
        public readonly int Priority;

        public QuestDesc(int level, int priority)
        {
            Level = level;
            Priority = priority;
        }
    }

    public class WorldDesc
    {
        public readonly string Name;
        public readonly string DisplayName;
        public readonly string Music;
        public readonly int Id;
        public readonly int Background;
        public readonly bool ShowDisplays;
        public readonly bool AllowTeleport;
        public readonly int BlockSight;
        public readonly bool Persist;
        public readonly bool IsTemplate;
        public readonly ushort[] Portals;
        public readonly Map[] Maps;

        public WorldDesc(XElement e)
        {
            Name = e.ParseString("@name");
            DisplayName = e.ParseString("@display", Name);
            Music = e.ParseString("Music", "");
            Id = e.ParseInt("@id");
            Background = e.ParseInt("Background");
            ShowDisplays = e.ParseBool("ShowDisplays");
            AllowTeleport = e.ParseBool("AllowTeleport");
            BlockSight = e.ParseInt("BlockSight");
            Persist = e.ParseBool("Persist");
            IsTemplate = e.ParseBool("IsTemplate");
            Portals = e.ParseUshortArray("Portals", ";", new ushort[0]);

            var maps = e.ParseStringArray("Maps", ";", new string[0]);
            Maps = new Map[maps.Length];
            for (var i = 0; i < maps.Length; i++)
            {
                if (maps[0].EndsWith("wmap"))
                    Maps[i] = new WMap(File.ReadAllBytes(Resources.CombineResourcePath($"Worlds/{maps[i]}")));
                else
                    Maps[i] = new JSMap(File.ReadAllText(Resources.CombineResourcePath($"Worlds/{maps[i]}")));
            }
        }
    }
}
