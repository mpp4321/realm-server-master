using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using RotMG.Common;

namespace RotMG.Game
{
    public class WMap : Map
    {
        public Dictionary<Terrain, List<IntPoint>> Terrains;

        public WMap(byte[] mapData)
        {
            var memStream = new MemoryStream(mapData);
            var version = memStream.ReadByte();
            if (version < 0 || version > 2)
                throw new NotSupportedException("WMap version " + version);

            using (var rdr = new BinaryReader(new ZlibStream(memStream, CompressionMode.Decompress)))
            {
                var tiles = new List<MapTile>();
                var tileCount = rdr.ReadInt16();
                for (var i = 0; i < tileCount; i++)
                {
                    var tile = new MapTile();
                    tile.GroundType = rdr.ReadUInt16();
                    var obj = rdr.ReadString();
                    tile.ObjectType = 255;
                    if (Resources.Id2Object.ContainsKey(obj))
                        tile.ObjectType = Resources.Id2Object[obj].Type;
#if DEBUG
                    else if (!string.IsNullOrEmpty(obj))
                        Program.Print(PrintType.Warn, $"Object: {obj} not found.");
#endif
                    
                    tile.Key = rdr.ReadString();
                    tile.Terrain = (Terrain) rdr.ReadByte();
                    tile.Region = (Region) rdr.ReadByte();
                    if (version == 1)
                        tile.Elevation = rdr.ReadByte();
                    tiles.Add(tile);
                }

                Width = rdr.ReadInt32();
                Height = rdr.ReadInt32();
                Tiles = new MapTile[Width, Height];
                
                Regions = new Dictionary<Region, List<IntPoint>>();
                Terrains = new Dictionary<Terrain, List<IntPoint>>();
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                    {
                        var tile = tiles[rdr.ReadInt16()];
                        if (version == 2)
                            tile.Elevation = rdr.ReadByte();
                        
                        if (!Regions.ContainsKey(tile.Region))
                            Regions[tile.Region] = new List<IntPoint>();
                        Regions[tile.Region].Add(new IntPoint(x, y));
                        
                        if (!Terrains.ContainsKey(tile.Terrain))
                            Terrains[tile.Terrain] = new List<IntPoint>();
                        Terrains[tile.Terrain].Add(new IntPoint(x, y));

                        Tiles[x, y] = tile;
                    }
                
                //Add composite under cave walls
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        if (Tiles[x, y].ObjectType != 255)
                        {
                            var desc = Resources.Type2Object[Tiles[x, y].ObjectType];
                            if ((desc.Class == "CaveWall" || desc.Class == "ConnectedWall") && Tiles[x, y].GroundType == 255)
                            {
                                Tiles[x, y].GroundType = 0xfd;
                            }
                        }
                    }
                }
            }
        }
    }
}