using Ionic.Zlib;
using Newtonsoft.Json;
using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.IO;

namespace RotMG.Game
{
    public class JSMap : Map
    {
        public JSMap(string data)
        {
            var json = JsonConvert.DeserializeObject<json_dat>(data);
            var buffer = ZlibStream.UncompressBuffer(json.data);
            var dict = new Dictionary<ushort, MapTile>();
            var tiles = new MapTile[json.width, json.height];

            for (var i = 0; i < json.dict.Length; i++)
            {
                var o = json.dict[i];
                dict[(ushort)i] = new MapTile
                {
                    GroundType = o.ground == null ? (ushort)255 : Resources.Id2Tile[o.ground].Type,
                    ObjectType = o.objs == null ? (ushort)255 : Resources.Id2Object[o.objs[0].id].Type,
                    Key = o.objs?[0].name,
                    Region = o.regions == null ? Region.None : (Region)Enum.Parse(typeof(Region), o.regions[0].id.Replace(" ", ""))
                };
            }

            using (var rdr = new PacketReader(new MemoryStream(buffer)))
            {
                for (var y = 0; y < json.height; y++)
                    for (var x = 0; x < json.width; x++)
                        tiles[x, y] = dict[(ushort)rdr.ReadInt16()];
            }

            //Add composite under cave walls
            for (var x = 0; x < json.width; x++)
            {
                for (var y = 0; y < json.height; y++)
                {
                    if (tiles[x, y].ObjectType != 255)
                    {
                        var desc = Resources.Type2Object[tiles[x, y].ObjectType];
                        if ((desc.Class == "CaveWall" || desc.Class == "ConnectedWall") && tiles[x, y].GroundType == 255)
                        {
                            tiles[x, y].GroundType = 0xfd;
                        }
                    }
                }
            }

            Tiles = tiles;
            Width = json.width;
            Height = json.height;

            InitRegions();
        }

        private void InitRegions()
        {
            Regions = new Dictionary<Region, List<IntPoint>>();
            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    var tile = Tiles[x, y];
                    if (!Regions.ContainsKey(tile.Region))
                        Regions[tile.Region] = new List<IntPoint>();
                    Regions[tile.Region].Add(new IntPoint(x, y));
                }
        }

        private struct json_dat
        {
            public byte[] data { get; set; }
            public loc[] dict { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        private struct loc
        {
            public string ground { get; set; }
            public obj[] objs { get; set; }
            public obj[] regions { get; set; }
        }

        private struct obj
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }

}
