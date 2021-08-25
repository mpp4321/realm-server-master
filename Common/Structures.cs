using RotMG.Game;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;

namespace RotMG.Common
{
    public struct FameStats
    {
        public int BaseFame;
        public int TotalFame;
        public List<FameBonus> Bonuses;
    }

    public struct FameBonus
    {
        public string Name;
        public int Fame;
    }

    public struct TileData
    {
        public ushort TileType;
        public short X;
        public short Y;

        public void Write(PacketWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
            wtr.Write(TileType);
        }
    }

    public struct ObjectDrop
    {
        public int Id;
        public bool Explode;

        public void Write(PacketWriter wtr)
        {
            wtr.Write(Id);
            wtr.Write(Explode);
        }
    }

    public struct ObjectDefinition
    {
        public ushort ObjectType;
        public ObjectStatus ObjectStatus;

        public void Write(PacketWriter wtr)
        {
            wtr.Write(ObjectType);
            ObjectStatus.Write(wtr);
        }
    }

    public struct ObjectStatus
    {
        public int Id;
        public Vector2 Position;
        public Dictionary<StatType, object> Stats;

        public void Write(PacketWriter wtr)
        {
            wtr.Write(Id);
            Position.Write(wtr);

            wtr.Write((byte)Stats.Count);
            foreach (var k in Stats)
            {
                wtr.Write((byte)k.Key);
                if (IsStringStat(k.Key))
                    wtr.Write((string)k.Value);
                else 
                    wtr.Write((int)k.Value);
            }
        }

        public static bool IsStringStat(StatType stat)
        {
            switch (stat)
            {
                case StatType.Name:
                case StatType.GuildName:
                    return true;
            }
            return false;
        }
    }

    public struct IntPoint
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(IntPoint a, IntPoint b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(IntPoint a, IntPoint b) => a.X != b.X || a.Y != b.Y;

        public bool Equals(IntPoint other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj)
        {
            if (obj is IntPoint p)
            {
                return Equals(p);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Y << 16) ^ X;
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }

    public struct SlotData
    {
        public int ObjectId;
        public byte SlotId;

        public SlotData(PacketReader rdr)
        {
            ObjectId = rdr.ReadInt32();
            SlotId = rdr.ReadByte();
        }
    }

    public struct TradeItem
    {
        public int Item;
        public int ItemData;
        public ItemType SlotType;
        public bool Tradeable;
        public bool Included;

        public void Write(PacketWriter wtr)
        {
            wtr.Write(Item);
            wtr.Write(ItemData);
            wtr.Write((int)SlotType);
            wtr.Write(Tradeable);
            wtr.Write(Included);
        }
    }

    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(PacketReader rdr)
        {
            X = rdr.ReadSingle();
            Y = rdr.ReadSingle();
        }

        public void Write(PacketWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static Vector2 operator -(Vector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }


        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }


        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }


        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }


        public static Vector2 operator +(Vector2 value1, float value2)
        {
            value1.X += value2;
            value1.Y += value2;
            return value1;
        }


        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }
        
        public static Vector2 operator -(Vector2 value1, float value2)
        {
            value1.X += value2;
            value1.Y += value2;
            return value1;
        }


        public static Vector2 operator *(Vector2 value1, Vector2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }


        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }


        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }


        public static Vector2 operator /(Vector2 value1, Vector2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }


        public static Vector2 operator /(Vector2 value1, float divider)
        {
            var factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            return new Vector2(
                MathUtils.Lerp(value1.X, value2.X, amount),
                MathUtils.Lerp(value1.Y, value2.Y, amount));
        }

        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            result = new Vector2(
                MathUtils.Lerp(value1.X, value2.X, amount),
                MathUtils.Lerp(value1.Y, value2.Y, amount));
        }

        public void Normalize()
        {
            var val = 1.0f / (float)Math.Sqrt(X * X + Y * Y);
            X *= val;
            Y *= val;
        }

        public static Vector2 Normalize(Vector2 value)
        {
            var val = 1.0f / (float)Math.Sqrt(value.X * value.X + value.Y * value.Y);
            value.X *= val;
            value.Y *= val;
            return value;
        }

        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            var val = 1.0f / (float)Math.Sqrt(value.X * value.X + value.Y * value.Y);
            result.X = value.X * val;
            result.Y = value.Y * val;
        }
        
        public float Length()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }
}
