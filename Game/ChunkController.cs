using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;

namespace RotMG.Game
{
    public class Chunk
    {
        public HashSet<Entity> Entities;
        public readonly int X;
        public readonly int Y;

        public Chunk(int x, int y)
        {
            Entities = new HashSet<Entity>();
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return (Y << 16) ^ X;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            if (obj is null || !(obj is Chunk))
                throw new Exception("Invalid object comparison.");
#endif
            return GetHashCode() == (obj as Chunk).GetHashCode();
        }
    }

    public class ChunkController
    {
        public const int Size = 8;
        public const int ActiveRadius = 32 / Size;

        public Chunk[,] Chunks;
        public int Width;
        public int Height;

        public ChunkController(int width, int height)
        {
            Width = width;
            Height = height;
            Chunks = new Chunk[Convert(Width) + 1, Convert(Height) + 1];
            for (var x = 0; x < Chunks.GetLength(0); x++)
                for (var y = 0; y < Chunks.GetLength(1); y++)
                    Chunks[x, y] = new Chunk(x, y);
        }

        public Chunk GetChunk(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Chunks.GetLength(0) || y >= Chunks.GetLength(1))
                return null;
            return Chunks[x, y];
        }

        public static int Convert(float value) => (int)Math.Ceiling(value / Size);

        public void Insert(Entity en)
        {
#if DEBUG
            if (en == null)
                throw new Exception("Entity is undefined.");
#endif
            var nx = Convert(en.Position.X);
            var ny = Convert(en.Position.Y);
            var chunk = Chunks[nx, ny];

            if (en.CurrentChunk != chunk)
            {
                en.CurrentChunk?.Entities.Remove(en);
                en.CurrentChunk = chunk;
                en.CurrentChunk.Entities.Add(en);
            }
        }

        public void Remove(Entity en)
        {
#if DEBUG
            if (en == null)
                throw new Exception("Entity is undefined.");
            if (en.CurrentChunk == null)
                throw new Exception("Chunk is undefined.");
            if (!en.CurrentChunk.Entities.Contains(en))
                throw new Exception("Chunk doesn't contain entity.");
#endif
            en.CurrentChunk.Entities.Remove(en);
            en.CurrentChunk = null;
        }

        public List<Entity> HitTest(Vector2 target, float radius)
        {
            var result = new List<Entity>();
            var size = Convert(radius);
            var beginX = Convert(target.X);
            var beginY = Convert(target.Y);
            var startX = Math.Max(0, beginX - size);
            var startY = Math.Max(0, beginY - size);
            var endX = Math.Min(Chunks.GetLength(0) - 1, beginX + size);
            var endY = Math.Min(Chunks.GetLength(1) - 1, beginY + size);

            for (var x = startX; x <= endX; x++)
                for (var y = startY; y <= endY; y++)
                    foreach (var en in Chunks[x, y].Entities)
                        if (target.Distance(en) < radius)
                            result.Add(en);

            return result;
        }

        public List<Entity> GetActiveChunks(HashSet<Chunk> chunks)
        {
            var result = new List<Entity>();
            foreach (var c in chunks)
                foreach (var en in c.Entities)
                    result.Add(en);
            return result;
        }

        public void Dispose()
        {
            for (var w = 0; w < Chunks.GetLength(0); w++)
                for (var h = 0; h < Chunks.GetLength(1); h++)
                    Chunks[w, h].Entities.Clear();
            Chunks = null;
        }
    }
}
