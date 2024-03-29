﻿using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        private const float MinMoveSpeed = 0.004f;
        private const float MaxMoveSpeed = 0.0096f;
        private const float MinAttackFreq = 0.0015f;
        private const float MaxAttackFreq = 0.008f;
        private const float MinAttackMult = 0.5f;
        private const float MaxAttackMult = 2f;
        private const float MaxSinkLevel = 18f;

        public static readonly string[] StatNames = new string[] {
            "HP",
            "MP",
            "Attack",
            "Defense",
            "Speed",
            "Dexterity",
            "Vitality",
            "Wisdom",
            "Protection",
            "CritChance",
        };

        public int[] Stats;
        public int[] Boosts;
        //Temporary effect boosts, like tinctures and elixrs

        public class BoostTimer
        {
            public int index;

            //Seconds left in timer
            public float timer;

            public int amount;

            public int id = -1;
        }

        public List<BoostTimer> EffectBoosts = new List<BoostTimer>();

        public Dictionary<StatType, object> PrivateSVs;

        float _hpRegenCounter;
        float _mpRegenCounter;
        public void TickRegens()
        {
            if (HasConditionEffect(ConditionEffectIndex.Bleeding))
                Hp = Math.Max(1, Hp - (int)(20 * Settings.SecondsPerTick));

            if (Hp == GetStat(0) || !CanHPRegen())
                _hpRegenCounter = 0;
            else
            {
                _hpRegenCounter += GetHPRegen() * Settings.SecondsPerTick;
                if (HasConditionEffect(ConditionEffectIndex.Healing))
                    _hpRegenCounter += 20 * Settings.SecondsPerTick;
                var regen = (int)_hpRegenCounter;
                if (regen > 0)
                {
                    Hp = Math.Min(GetStat(0), Hp + regen);
                    _hpRegenCounter -= regen;
                }
            }

            if (MP == GetStat(1) || !CanMPRegen())
                _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += GetMPRegen() * Settings.SecondsPerTick;
                var regen = (int)_mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(GetStat(1), MP + regen);
                    _mpRegenCounter -= regen;
                }
            }
        }

        public int GetStat(int index)
        {
#if DEBUG
            if (index < 0 || index >= Stats.Length)
                throw new Exception("Stat out of bounds");
#endif
            return Stats[index] + Boosts[index] + GetTemporaryStatBoost(index);
        }

        public float GetMovementSpeed()
        {
            if (HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return 0;
            
            if (HasConditionEffect(ConditionEffectIndex.Slowed))
                return MinMoveSpeed * MoveMultiplier;

            var ret = MinMoveSpeed + GetStat(4) / 75f * (MaxMoveSpeed - MinMoveSpeed);
            if (HasConditionEffect(ConditionEffectIndex.Speedy))
            {
                ret = ret * 1.5f;
            }
            ret = ret * MoveMultiplier;
            return ret;
        }

        public float GetMoveMultiplier()
        {
            var tile = Parent.Tiles[(int)Position.X, (int)Position.Y];
            var desc = Resources.Type2Tile[tile.Type];

            if (desc.Sinking)
            {
                SinkLevel = Math.Min(SinkLevel + 1, (int)MaxSinkLevel);
                return 0.1f + (1 - SinkLevel / MaxSinkLevel) * (desc.Speed - 0.1f);
            }
            else
            {
                SinkLevel = 0;
                return desc.Speed;
            }
        }

        public float GetAttackFrequency()
        {
            if (HasConditionEffect(ConditionEffectIndex.Dazed))
                return MinAttackFreq;

            var ret = MinAttackFreq + GetStat(5) / 75f * (MaxAttackFreq - MinAttackFreq);
            if (HasConditionEffect(ConditionEffectIndex.Berserk))
            {
                ret = ret * 1.5f;
            }
            return ret;
        }

        public float GetAttackMultiplier()
        {
            if (HasConditionEffect(ConditionEffectIndex.Weak))
                return MinAttackMult;

            var ret = MinAttackMult + Math.Max(0,GetStat(2)) / 75f * (MaxAttackMult - MinAttackMult);
            if (HasConditionEffect(ConditionEffectIndex.Damaging))
                ret = ret * 1.5f;
            return ret;
        }

        public float GetHPRegen()
        {
            return 1 + GetStat(6) * .24f;
        }

        public float GetMPRegen()
        {
            return 0.5f + GetStat(7) * .18f;
        }

        public bool CanMPRegen()
        {
            return !HasConditionEffect(ConditionEffectIndex.Quiet);
        }

        public bool CanHPRegen()
        {
            return !HasConditionEffect(ConditionEffectIndex.Bleeding) && !HasConditionEffect(ConditionEffectIndex.Sick);
        }

        public int GetMaxedStats()
        {
            return (Desc as PlayerDesc).Stats.Where((t, i) => Stats[i] >= t.MaxValue).Count();
        }

        public void InitStats(CharacterModel character)
        {
            var playerType = Resources.Type2Player[(ushort)character.ClassType];

            /*if(character.Stats.Length < playerType.Stats.Length)
            {
                var oldStats = character.Stats;
                character.Stats = new int[playerType.Stats.Length];
                for(var i = 0; i < playerType.Stats.Length; i++)
                {
                    if(i > oldStats.Length)
                    {
                        character.Stats[i] = playerType.StartingValues[i];
                    } else
                    {
                        character.Stats[i] = oldStats[i];
                    }
                }
            }*/

            Stats = character.Stats
                .Zip(Enumerable.Range(0, playerType.Stats.Length))
                .Select(
                    tupl => Math.Min(
                            playerType.Stats[tupl.Second].MaxValue,
                            tupl.First
                            )
                ).ToArray();
            Boosts = new int[Stats.Length];
        }

        public int GetCurrency(Currency currency)
        {
            if (currency == Currency.Gold)
                return Client.Account.Stats.Credits;
            return Client.Account.Stats.Fame;
        }

        public void SetPrivateSV(StatType type, object value)
        {
            if(type <= StatType.ItemData19 && type >= StatType.ItemData0)
            {
                value = ItemDesc.ExportItemDataJson(value as ItemDataJson);
            }
            PrivateSVs[type] = value;
        }

        public void AddIdentifiedEffectBoost(BoostTimer t, bool agg = false, Func<int, int, int> aggf=null)
        {
            //Find matching boost
            for(int i = 0; i < EffectBoosts.Count; i++)
            {
                var boost = EffectBoosts[i];
                if(boost.id == t.id)
                {
                    if(agg)
                    {
                        EffectBoosts[i].amount = aggf(EffectBoosts[i].amount, t.amount);
                        EffectBoosts[i].index = t.index;
                    } else
                    { 
                        EffectBoosts[i].amount = t.amount;
                        EffectBoosts[i].index = t.index;
                    }
                    EffectBoosts[i].timer = t.timer;
                    return;
                }
            }
            //Not found add 
            EffectBoosts.Add(t);
        }

        public int GetTemporaryStatBoost(int index)
        {
            return EffectBoosts.Where(a => a.index == index).Select(a => a.amount).Sum();
        }

        public void UpdateRunes()
        {
            if (Client != null)
                TrySetSV(StatType.RuneEffects, string.Join(",", Client.Character.SelectedRunes));
        }

        public void UpdateStats()
        {
            TrySetSV(StatType.MaxHp, GetStatTotal(0));
            TrySetSV(StatType.MaxHpBoost, GetBoosts(0));
            TrySetSV(StatType.MaxMp, GetStatTotal(1));
            TrySetSV(StatType.MaxMpBoost, GetBoosts(1));
            SetPrivateSV(StatType.Attack, GetStatTotal(2));
            SetPrivateSV(StatType.AttackBoost, GetBoosts(2));
            SetPrivateSV(StatType.Defense, GetStatTotal(3));
            SetPrivateSV(StatType.DefenseBoost, GetBoosts(3));
            TrySetSV(StatType.Speed, GetStatTotal(4));
            TrySetSV(StatType.SpeedBoost, GetBoosts(4));
            TrySetSV(StatType.Dexterity, GetStatTotal(5));
            TrySetSV(StatType.DexterityBoost, GetBoosts(5));
            SetPrivateSV(StatType.Vitality, GetStatTotal(6));
            SetPrivateSV(StatType.VitalityBoost, GetBoosts(6));
            SetPrivateSV(StatType.Wisdom, GetStatTotal(7));
            SetPrivateSV(StatType.WisdomBoost, GetBoosts(7));
            TrySetSV(StatType.Protection, GetStatTotal(8));
            TrySetSV(StatType.ProtectionBoost, GetBoosts(8));
            TrySetSV(StatType.CritChance, GetStatTotal(9));
            TrySetSV(StatType.CritChanceBoost, GetBoosts(9));
        }

        public void TickBoosts()
        {
            for(int i = 0; i < EffectBoosts.Count; i++)
            {
                EffectBoosts[i].timer -= Settings.SecondsPerTick;
            }
            if(this.EffectBoosts.Any(a => a.timer < 0f))
            {
                this.EffectBoosts = this.EffectBoosts.Where(a => a.timer > 0f).ToList();
                UpdateStats();
            }
        }

        internal int GetStatTotal(int v)
        {
            return Math.Max(0, Stats[v] + GetBoosts(v));
        }

        internal int GetBoosts(int v)
        {
            return Boosts[v] + GetTemporaryStatBoost(v);
        }
        internal int GetStatTotalNotTemporary(int v)
        {
            return Math.Max(0, Stats[v] + Boosts[v]);
        }

        public void ClearItemData(int slot)
        {
            ItemDatas[slot] = new ItemDataJson();
        }

        public void ExchangeCrystals()
        {
            var crystals = new List<int>();
            var crystalType = Resources.Id2Item["Realm Equipment Crystal"].Type;
            for(var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != -1)
                {
                    if (Inventory[i] == crystalType)
                    {
                        crystals.Add(i);
                    }
                }
            }
            if(crystals.Count >= 3)
            {
                Inventory[crystals[0]] = Resources.Id2Item["Upgrading Equipment Crystal"].Type;
                ClearItemData(crystals[0]);
                for(var i = 1; i < 3; i++)
                {
                    Inventory[crystals[i]] = -1;
                    ClearItemData(crystals[i]);
                }
                UpdateInventory();
            }
        }

        public void ChangeSkin(int id)
        {
            if (this.Client.Account.OwnedSkins.Contains(id))
            {
                this.SkinType = id;
            }
        }
    }
}
