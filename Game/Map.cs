using System.Collections.Generic;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game
{
    public enum Region
    {
        None,
        Spawn,
        Regen,
        BlocksSight,
        Note,
        Enemy,
        Enemy1,
        Enemy2,
        Enemy3,
        Enemy4,
        Enemy5,
        Enemy6,
        Decoration1,
        Decoration2,
        Decoration3,
        Decoration4,
        Decoration5,
        Decoration6,
        Trigger1,
        Callback1,
        Trigger2,
        Callback2,
        Trigger3,
        Callback3,
        VaultChest,
        GiftChest,
        Store1,
        Store2,
        Store3,
        Store4,
        VaultPortal,
        RealmPortal,
        GuildPortal,
    }
    
    public enum Terrain : byte
    {
        None = 0,
        Mountains,
        HighSand,
        HighPlains,
        HighForest,
        MidSand,
        MidPlains,
        MidForest,
        LowSand,
        LowPlains,
        LowForest,
        ShoreSand,
        ShorePlains,
        BeachTowels,
    }

    public class MapTile
    {
        public MapTile OriginalTile;
        public Terrain Terrain;
        public byte Elevation;
        
        public ushort GroundType;
        public ushort ObjectType;
        public Region Region;
        public string Key;

        public void CopyTo(Tile tile, World world, int x, int y)
        {
            tile.Region = Region;
            tile.Key = Key;
            world.UpdateTile(x, y, GroundType);

        }
    }
    
    public abstract class Map
    {
        public MapTile[,] Tiles;
        public int Width;
        public int Height;
        public DictionaryWithDefault<Region, List<IntPoint>> Regions;

        public bool IsWithin(IntPoint x)
        {
            return x.X >= 0 && x.Y >= 0 && x.X < Width && x.Y < Height;
        }
        
        public void ProjectOntoWorld(World world, IntPoint pos)
        {
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var projX = pos.X + x;
                var projY = pos.Y + y;
                var tile = world.GetTile(projX, projY);
                if (tile == null)
                    continue;

                var spTile = Tiles[x, y];
                if (spTile.GroundType == 255)
                    continue;
                
                if (tile.Region != 0)
                {
                    world.Map.Regions[tile.Region].Remove(new IntPoint(projX, projY));
                }

                if (spTile.Region != 0)
                {
                    // incase region doesnt exist in map already
                    var intPointsInRegion = world.Map.Regions[spTile.Region];
                    intPointsInRegion.Add(new IntPoint(projX, projY));
                    world.Map.Regions[spTile.Region] = intPointsInRegion;
                }
                
                tile.Region = spTile.Region;
                tile.Key = spTile.Key;
                world.UpdateTile(projX, projY, spTile.GroundType);
                if (spTile.ObjectType != 0xff)
                {
                    var desc = Resources.Type2Object[spTile.ObjectType];
                    if (desc.Static)
                    {
                        tile.BlocksSight = desc.BlocksSight;
                        world.UpdateStatic(projX, projY, spTile.ObjectType);
                    }
                    else
                        world.AddEntity(Entity.Resolve(spTile.ObjectType), new Vector2(projX + 0.5f, projY + 0.5f));
                } else
                {
                    world.RemoveStatic(projX, projY);
                }
            }
        }
    }
}