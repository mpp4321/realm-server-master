using RotMG.Common;
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

        public int[] Stats;
        public int[] Boosts;
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
            return Stats[index] + Boosts[index];
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

            var ret = MinAttackMult + GetStat(2) / 75f * (MaxAttackMult - MinAttackMult);
            if (HasConditionEffect(ConditionEffectIndex.Damaging))
                ret = ret * 1.5f;
            return ret;
        }

        public float GetHPRegen()
        {
            return 1 + GetStat(6) * .12f;
        }

        public float GetMPRegen()
        {
            return 0.5f + GetStat(7) * .06f;
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
            Stats = character.Stats.ToArray();
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
            PrivateSVs[type] = value;
        }

        public void UpdateStats()
        {
            TrySetSV(StatType.MaxHp, Stats[0] + Boosts[0]);
            TrySetSV(StatType.MaxHpBoost, Boosts[0]);
            TrySetSV(StatType.MaxMp, Stats[1] + Boosts[1]);
            TrySetSV(StatType.MaxMpBoost, Boosts[1]);
            SetPrivateSV(StatType.Attack, Stats[2] + Boosts[2]);
            SetPrivateSV(StatType.AttackBoost, Boosts[2]);
            SetPrivateSV(StatType.Defense, Stats[3] + Boosts[3]);
            SetPrivateSV(StatType.DefenseBoost, Boosts[3]);
            TrySetSV(StatType.Speed, Stats[4] + Boosts[4]);
            TrySetSV(StatType.SpeedBoost, Boosts[4]);
            TrySetSV(StatType.Dexterity, Stats[5] + Boosts[5]);
            TrySetSV(StatType.DexterityBoost, Boosts[5]);
            SetPrivateSV(StatType.Vitality, Stats[6] + Boosts[6]);
            SetPrivateSV(StatType.VitalityBoost, Boosts[6]);
            SetPrivateSV(StatType.Wisdom, Stats[7] + Boosts[7]);
            SetPrivateSV(StatType.WisdomBoost, Boosts[7]);
        }
    }
}
