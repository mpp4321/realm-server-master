﻿using RotMG.Common;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Entities
{
    public partial class Player : Entity, IContainer
    {
        private const int MaxLatencyMS = 4000;
        public const int MaxPotions = 6;

        public String PrioritizeQuest = null;

        public float LootBoost = 0.0f;

        public static int[] Stars = 
        {
            20,
            150,
            400,
            800,
            2000
        };

        public Client Client;
        public string GuildInvite;

        private int _accountId;
        public int AccountId
        {
            get => _accountId;
            set => TrySetSV(StatType.AccountId, _accountId = value);
        }

        private int _exp;
        public int EXP
        {
            get => _exp;
            set => TrySetSV(StatType.Exp, _exp = value);
        }

        private int _nextLevelExp;
        public int NextLevelEXP
        {
            get => _nextLevelExp;
            set => SetPrivateSV(StatType.NextLevelExp, _nextLevelExp = value);
        }

        private int _level;
        public int Level
        {
            get => _level;
            set => TrySetSV(StatType.Level, _level = value);
        }

        private int _charFame;
        public int CharFame
        {
            get => _charFame;
            set => TrySetSV(StatType.CharFame, _charFame = value);
        }

        private int _fame;
        public int Fame
        {
            get => _fame;
            set => SetPrivateSV(StatType.Fame, _fame = value);
        }

        private int _nextClassQuestFame;
        public int NextClassQuestFame
        {
            get => _nextClassQuestFame;
            set => SetPrivateSV(StatType.NextClassQuestFame, _nextClassQuestFame = value);
        }

        private int _numStars;
        public int NumStars
        {
            get => _numStars;
            set => TrySetSV(StatType.NumStars, _numStars = value);
        }

        private string _guildName;
        public string GuildName
        {
            get => _guildName;
            set => TrySetSV(StatType.GuildName, _guildName = value);
        }

        private int _guildRank;
        public int GuildRank
        {
            get => _guildRank;
            set => TrySetSV(StatType.GuildRank, _guildRank = value);
        }

        private int _credits;
        public int Credits
        {
            get => _credits;
            set => SetPrivateSV(StatType.Credits, _credits = value);
        }

        private int _tex1;
        public int Tex1
        {
            get => _tex1;
            set => TrySetSV(StatType.Tex1, _tex1 = value);
        }

        private int _tex2;
        public int Tex2
        {
            get => _tex2;
            set => TrySetSV(StatType.Tex2, _tex2 = value);
        }

        private int _skinType;
        public int SkinType
        {
            get => _skinType;
            set => TrySetSV(StatType.Texture, _skinType = value);
        }

        private bool _hasBackpack;
        public bool HasBackpack
        {
            get => _hasBackpack;
            set => SetPrivateSV(StatType.HasBackpack, (_hasBackpack = value).GetHashCode());
        }

        private int _mp;
        public int MP
        {
            get => _mp;
            set => TrySetSV(StatType.Mp, _mp = value);
        }

        private int _maxMp;
        public int MaxMP
        {
            get => _maxMp;
            set => TrySetSV(StatType.MaxMp, _maxMp = value);
        }

        private int _oxygen;
        public int Oxygen
        {
            get => _oxygen;
            set => SetPrivateSV(StatType.Breath, _oxygen = value);
        }

        private int _healthPotions;
        public int HealthPotions
        {
            get => _healthPotions;
            set => SetPrivateSV(StatType.HealthPotionStack, _healthPotions = value);
        }

        private int _magicPotions;
        public int MagicPotions
        {
            get => _magicPotions;
            set => SetPrivateSV(StatType.MagicPotionStack, _magicPotions = value);
        }

        private int _sinkLevel;
        public int SinkLevel
        {
            get => _sinkLevel;
            set => TrySetSV(StatType.SinkLevel, _sinkLevel = value);
        }

        public FameStatsInfo FameStats;

        public Player(Client client) : base((ushort)client.Character.ClassType)
        {
            PrivateSVs = new Dictionary<StatType, object>();

            Client = client;
            Hp = client.Character.HP;
            MP = client.Character.MP;
            AccountId = client.Account.Id;
            Name = client.Account.Name;
            Level = client.Character.Level;
            Size = client.Character.Size > 0 ? client.Character.Size : 100;
            Glow = client.Character.GlowColor;
            LootBoost = Settings.LootBoost;

            if (client.Character.HealthPotions != 0) HealthPotions = client.Character.HealthPotions;
            if (client.Character.MagicPotions != 0) MagicPotions = client.Character.MagicPotions;
            if (client.Character.HasBackpack) HasBackpack = client.Character.HasBackpack;
            if (client.Character.SkinType != 0) SkinType = client.Character.SkinType;
            if (client.Character.Tex1 != 0) Tex1 = client.Character.Tex1;
            if (client.Character.Tex2 != 0) Tex2 = client.Character.Tex2;
            if (client.Account.Stats.Credits != 0) Credits = client.Account.Stats.Credits;
            if (client.Account.Stats.Fame != 0) Fame = client.Account.Stats.Fame;

            if (!string.IsNullOrWhiteSpace(client.Account.GuildName))
            {
                GuildName = client.Account.GuildName;
                GuildRank = client.Account.GuildRank;
            }

            var stars = Database.GetStars(client.Account);
            if (stars != 0) NumStars = stars;

            if (Database.IsLegend(client.Account.Id))
                SetSV(StatType.LegendaryRank, 0);

            FameStats = client.Character.FameStats;

            InitInventory(client.Character);
            InitStats(client.Character);
            InitLevel(client.Character);

            RecalculateEquipBonuses();

            PartyReconnect();

            UpdateRunes();

            InvOnLoad();
        }

        public void InvOnLoad()
        {
            for(var i = 0; i < 4; i++)
            {
                var eff = Resources.Type2Item.GetValueOrDefault((ushort)Inventory[i])?.UniqueEffect;
                if(eff != null)
                    ItemHandlerRegistry.Registry[eff].OnLoad(this);
            }
        }

        public void SaveToCharacter()
        {
            Client.Character.HP = Hp;
            Client.Character.MP = MP;
            Client.Character.Level = Level;
            Client.Character.HealthPotions = HealthPotions;
            Client.Character.MagicPotions = MagicPotions;
            Client.Character.HasBackpack = HasBackpack;
            Client.Character.SkinType = SkinType;
            Client.Character.Tex1 = Tex1;
            Client.Character.Tex2 = Tex2;
            Client.Character.Experience = EXP;
            Client.Character.Fame = CharFame;
            Client.Character.Inventory = Inventory.ToArray();
            Client.Character.ItemDataJsons = ItemDatas.ToArray();
            Client.Character.Stats = Stats.ToArray();
        }

        public override void Init()
        {
            base.Init();
            TileUpdates = new int[Parent.Width, Parent.Height];
            EntityUpdates = new Dictionary<int, int>();
            Entities = new HashSet<Entity>();
            CalculatedSightCircle = new HashSet<IntPoint>();
            AwaitingProjectiles = new Queue<List<Projectile>>();
            AckedProjectiles = new Dictionary<int, ProjectileAck>();
            ShotProjectiles = new Dictionary<int, Projectile>();
            AckedExplosions = new Dictionary<int, ExplosionAck>();
            AwaitingAoes = new Queue<AoeAck>();
            ShootAEs = new Queue<ushort>();
            AwaitingGoto = new Queue<int>();
            TradedWith = new HashSet<int>();
            PendingTrades = new HashSet<int>();
            
            SpeedHistory = new List<float>(SpeedHistoryCount);
            for (var i = 0; i < SpeedHistoryCount; i++) //Just make some temporary history when player is first initialized
                PushSpeedToHistory(GetMovementSpeed() * 1.5f);

            Client.Send(GameServer.AccountList(0, Client.Account.LockedIds));
            Client.Send(GameServer.AccountList(1, Client.Account.IgnoredIds));

            ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
            ApplyConditionEffect(ConditionEffectIndex.Invisible, 3000);
            GetNextQuest(true);
        }

        public void Heal(int amount, bool magic, bool notify=true)
        {
            var heal = 0;
            if (magic)
            {
                if (HasConditionEffect(ConditionEffectIndex.Quiet))
                    return;
                
                var mp = MP;
                MP = Math.Max(1, Math.Min(GetStat(0), MP + amount));
                heal = MP - mp;
            }
            else
            {
                if (HasConditionEffect(ConditionEffectIndex.Sick))
                    return;
                
                var hp = Hp;
                Hp = Math.Max(0, Math.Min(GetStat(0), Hp + amount));
                heal = Hp - hp;
            }

            if (heal <= 0) 
                return;

            if (!notify) return;

            var notification = GameServer.Notification(Id, $"+{heal}", magic ? 0xff6084e0 : 0xff00ff00);
            foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
            {
                if (en is Player player && 
                    (player.Client.Account.Notifications || player.Equals(this)))
                {
                    player.Client.Send(notification);
                }
            }
        }

        public void Death(string killer)
        {
#if DEBUG
            if (Parent.Name.Equals("Dreamland"))
                return;
#endif
            if (Dead) 
                return;

            if (Parent.Name.Equals("Nexus") || Parent.Name.Equals("Dreamland")) 
            {
                Client.Disconnect();
                return;
            }

            Client.Active = false;
            Dead = true;

            SaveToCharacter();
            Database.Death(killer, Client.Account, Client.Character);

            var death = GameServer.Death(Client.Account.Id, Client.Character.Id, killer);
            Client.Send(death);

            var text = GameServer.Text("", 0, -1, 0, "", $"{Name} died at level {Level} killed by {killer}!");
            var sound = GameServer.PlaySound("quack");

            foreach (var p in Parent.Players.Values)
            {
                p.Client.Send(text);
                if (p.Client.Account.Sounds)
                    p.Client.Send(sound);
            }

            ushort type;
            int time;
            switch (GetMaxedStats())
            {
                case 8: type = 0x0735; time = 600000; break;
                case 7: type = 0x0734; time = 600000; break;
                case 6: type = 0x072b; time = 600000; break;
                case 5: type = 0x072a; time = 600000; break;
                case 4: type = 0x0729; time = 600000; break;
                case 3: type = 0x0728; time = 600000; break;
                case 2: type = 0x0727; time = 600000; break;
                case 1: type = 0x0726; time = 600000; break;
                default:
                    type = 0x0725; time = 300000;
                    if (Level < 20) { type = 0x0724; time = 60000; }
                    if (Level <= 1) { type = 0x0723; time = 30000; }
                    break;
            }

            var grave = new Entity(type, time);
            grave.TrySetSV(StatType.Name, Name);
            Parent.AddEntity(grave, Position);
            Parent.RemoveEntity(this);

            Manager.AddTimedAction(1500, () => 
            {
                Client.Disconnect();
            });
        }

        public bool Damage(string hitter, int damage, ConditionEffectDesc[] effects, bool pierces, bool showSelf = false)
        {
#if DEBUG
            /*if (HasConditionEffect(ConditionEffectIndex.Invincible))
                throw new Exception("Entity should not be damaged if invincible");
            if (effects == null)
                throw new Exception("Null effects");
            if (string.IsNullOrWhiteSpace(hitter))
                throw new Exception("Undefined hitter");*/
#else
            if (HasConditionEffect(ConditionEffectIndex.Invincible))
                return true;
#endif

            //Projectiles never have null effects. But other sources of damage might.
            foreach (var eff in effects)
            {
              ApplyConditionEffect(eff.Effect, eff.DurationMS, eff.Probability);
            }

            //Force pierce if armor broken
            if (HasConditionEffect(ConditionEffectIndex.ArmorBroken))
                pierces = true;

            //Calculate damage with defense
            var damageWithDefense = this.GetDefenseDamage(damage, GetStatTotal(3), pierces, GetStatTotal(8));

            //Nullify damage if invulnerable
            if (HasConditionEffect(ConditionEffectIndex.Invulnerable))
                damageWithDefense = 0;

            Hp -= damageWithDefense;
            FameStats.DamageTaken += damageWithDefense;
            if (Hp <= 0)
            {
                Death(hitter);
                return true;
            }

            var packet = GameServer.Damage(Id, effects.Select(k => k.Effect).ToArray(), damageWithDefense);
            foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
                if (en is Player player && player.Client.Account.AllyDamage && (showSelf || !player.Equals(this)))
                    player.Client.Send(packet);
            return false;
        }

        public override void Tick()
        {
            if (TooLongSinceLastValidation())
            {
                Client.Disconnect();
                return;
            }

            if (Manager.TotalTime % 60000 == 0)
                FameStats.MinutesActive++;
            
            if (Manager.TotalTime % 5000 == 0)
                GetNextQuest(false);

            TickRegens();
            TickBoosts();
            TickProjectiles();

            if(Client?.Account != null)
                Client.Account.EntitySpawnCooldown -= Settings.MillisecondsPerTick;

            //Iterate equipment and tick unique effs, maybe once every couple ticks for performance?
            for(int i = 0; i < 4; i++)
            {
                var eff = Resources.Type2Item.GetValueOrDefault((ushort) Inventory[i])?.UniqueEffect;
                if(eff != null)
                    ItemHandlerRegistry.Registry[eff].OnTick(this);
            }

            foreach(var rune in this.Client?.Character?.SelectedRunes ?? new string[] { })
            {
                if (ItemHandlerRegistry.Registry.TryGetValue(rune, out var eff))
                {
                    eff.OnTick(this);
                }
            }

            base.Tick();
        }

        private int _serverStartTime = -1;
        private int _serverTime = -1;
        private int _clientStartTime = -1;
        private int _clientTime = -1;

        public int GetLastClientTime()
        {
            return _clientTime;
        }
        public bool ValidTime(int clientTime)
        {
            var serverTime = Manager.TotalTimeUnsynced;
            if (_serverTime == -1)
            {
                _clientTime = clientTime;
                _clientStartTime = clientTime;
                _serverTime = serverTime;
                _serverStartTime = serverTime;
                return true;
            }

            if (clientTime < _clientTime)
                return false;

            var clientDiff = clientTime - _clientTime;
            var serverDiff = serverTime - _serverTime;
            var startDiff = Math.Abs((serverTime - _serverStartTime) - (clientTime - _clientStartTime));

            if (clientDiff < 0 || serverDiff < 0 || clientDiff > MaxLatencyMS || serverDiff > MaxLatencyMS)
                return false;

            if (startDiff > MaxLatencyMS)
                return false;

            _clientTime = clientTime;
            _serverTime = serverTime;
            return true;
        }

        public bool TooLongSinceLastValidation()
        {
            if (_serverTime == -1)
                return false;
            var serverDiff = Manager.TotalTimeUnsynced - _serverTime;
            return serverDiff > MaxLatencyMS;
        }

        public override string ToString()
        {
            return $"<{Name}> <{Parent.Name}:{Parent.Id}> <{Position.ToIntPoint()}>";
        }

        public override void Dispose()
        {
            TileUpdates = null;

            EntityUpdates.Clear();
            Entities.Clear();
            CalculatedSightCircle.Clear();
            AwaitingProjectiles.Clear();
            AckedExplosions.Clear();
            AckedProjectiles.Clear();
            ShotProjectiles.Clear();
            AwaitingAoes.Clear();
            ShootAEs.Clear();
            AwaitingGoto.Clear();
            PrivateSVs.Clear();
            UniqueEffectsFromSet.Clear();
            base.Dispose();
        }

        public void ForceMove(Vector2 vec, int time)
        {
            Parent.MoveEntity(this, vec);
            AwaitingGoto.Enqueue(time);
            var go = GameServer.Goto(Id, vec);
            Client.Send(go);
        }
    }
}
