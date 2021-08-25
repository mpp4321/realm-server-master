using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using RotMG.Game.Logic;
using RotMG.Game.Worlds;
using RotMG.Networking;

namespace RotMG.Game
{
    public class Tile
    {
        public int UpdateCount;
        public ushort Type;
        public Region Region;
        public StaticObject StaticObject;
        public bool BlocksSight;
        public string Key;
    }

    public class World
    {
        public Loot WorldLoot = new Loot();

        public int Id;
        public int NextObjectId;
        public int NextProjectileId;

        public Dictionary<int, Entity> Entities;
        public Dictionary<int, Entity> Quests;
        public Dictionary<int, Entity> Constants;
        public Dictionary<int, Player> Players;
        public Dictionary<int, StaticObject> Statics;

        public ChunkController EntityChunks;
        public ChunkController PlayerChunks;

        public int UpdateCount;
        public List<string> ChatMessages;

        public Tile[,] Tiles;
        public Map Map;

        public int Width;
        public int Height;

        public int Background;
        public bool ShowDisplays;
        public bool AllowTeleport;
        public int BlockSight;
        public bool Persist;
        public bool IsTemplate;

        public string Name;
        public string DisplayName;
        public string Music;

        public Portal Portal;

        private bool _closed;
        public bool Closed
        {
            get => _closed;
            protected set
            {
                _closed = value;
                OnClosedChanged?.Invoke();
            }
        }

        private event Action OnClosedChanged;

        protected int AliveTime;

        public World(Map map, WorldDesc desc)
        {
            Map = map;
            Width = map.Width;
            Height = map.Height;

            Background = desc.Background;
            ShowDisplays = desc.ShowDisplays;
            AllowTeleport = desc.AllowTeleport;
            BlockSight = desc.BlockSight;
            Persist = desc.Persist;
            IsTemplate = desc.IsTemplate;

            Name = desc.Name;
            DisplayName = desc.DisplayName;
            Music = desc.Music;

            OnClosedChanged += UpdatePortalState;

            Entities = new Dictionary<int, Entity>();
            Quests = new Dictionary<int, Entity>();
            Constants = new Dictionary<int, Entity>();
            Players = new Dictionary<int, Player>();
            Statics = new Dictionary<int, StaticObject>();

            EntityChunks = new ChunkController(Width, Height);
            PlayerChunks = new ChunkController(Width, Height);

            ChatMessages = new List<string>();

            Tiles = new Tile[Width, Height];

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    var js = map.Tiles[x, y];
                    var tile = Tiles[x, y] = new Tile()
                    {
                        Type = js.GroundType,
                        Region = js.Region,
                        Key = js.Key,
                        UpdateCount = int.MaxValue / 2
                    };

                    if (js.ObjectType != 0xff)
                    {
                        var entity = Entity.Resolve(js.ObjectType);
                        if (entity.Desc.Static)
                        {
                            if (entity.Desc.BlocksSight)
                                tile.BlocksSight = true;
                            tile.StaticObject = (StaticObject)entity;
                        }

                        AddEntity(entity, new Vector2(x + 0.5f, y + 0.5f));
                    }
                }
            UpdateCount = int.MaxValue / 2;
        }

        protected void OverwriteMap(Map map)
        {
            foreach (var player in Players.Values)
                player.Client.Disconnect();
            
            Dispose();
            Map = map;
            Width = map.Width;
            Height = map.Height;
            
            EntityChunks = new ChunkController(Width, Height);
            PlayerChunks = new ChunkController(Width, Height);
            
            Tiles = new Tile[Width, Height];

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var js = map.Tiles[x, y];
                var tile = Tiles[x, y] = new Tile()
                {
                    Type = js.GroundType,
                    Region = js.Region,
                    Key = js.Key,
                    UpdateCount = int.MaxValue / 2
                };

                if (js.ObjectType != 0xff)
                {
                    var entity = Entity.Resolve(js.ObjectType);
                    if (entity.Desc.Static)
                    {
                        if (entity.Desc.BlocksSight)
                            tile.BlocksSight = true;
                        tile.StaticObject = (StaticObject)entity;
                    }

                    AddEntity(entity, new Vector2(x + 0.5f, y + 0.5f));
                }
            }
            UpdateCount = int.MaxValue / 2;
        }

        public IntPoint GetRegion(Region region)
        {
            if (!Map.Regions.ContainsKey(region))
                return new IntPoint(0, 0);
            return Map.Regions[region][MathUtils.Next(Map.Regions[region].Count)];
        }

        public List<IntPoint> GetAllRegion(Region region)
        {
            if (!Map.Regions.ContainsKey(region))
                return new List<IntPoint>();
            return Map.Regions[region];
        }

        public virtual IntPoint GetSpawnRegion()
        {
            return GetRegion(Region.Spawn);
        }

        public void UpdateTile(int x, int y, ushort type)
        {
            var tile = GetTile(x, y);
            if (tile != null)
            {
                tile.Type = type;
                tile.UpdateCount++;

                UpdateCount++;
            }
        }

        //public IntPoint CastLine(int x, int y, int x2, int y2)
        //{
        //    int w = x2 - x;
        //    int h = y2 - y;

        //    int dx1 = w < 0 ? -1 : w > 0 ? 1 : 0;
        //    int dy1 = h < 0 ? -1 : h > 0 ? 1 : 0;
        //    int dx2 = dx1;
        //    int dy2 = 0;

        //    int longest = w < 0 ? -w : w;
        //    int shortest = h < 0 ? -h : h;

        //    if (!(longest > shortest))
        //    {
        //        longest = h < 0 ? -h : h;
        //        shortest = w < 0 ? -w : w;
        //        if (h < 0)
        //            dy2 = -1;
        //        else if (h > 0)
        //            dy2 = 1;
        //        dx2 = 0;
        //    }

        //    int numerator = longest >> 1;
        //    for (int i = 0; i <= longest; i++)
        //    {
        //        if (BlocksSight(x, y))
        //            return new IntPoint(x, y);

        //        numerator += shortest;
        //        if (!(numerator < longest))
        //        {
        //            numerator -= longest;
        //            x += dx1;
        //            y += dy1;
        //        }
        //        else
        //        {
        //            x += dx2;
        //            y += dy2;
        //        }
        //    }

        //    return new IntPoint(-1, -1);
        //}

        public void UpdateStatic(int x, int y, ushort type)
        {
            var tile = GetTile(x, y);
            if (tile != null)
            {
                if (!Resources.Type2Object[type].Static)
                {
#if DEBUG
                    Program.Print(PrintType.Error, $"Entity <{type}> is not a static object");
#endif
                    return;
                }
                if (tile.StaticObject != null)
                {
                    RemoveEntity(tile.StaticObject);
                    tile.StaticObject = null;
                }
                tile.StaticObject = new StaticObject(type);
                tile.BlocksSight = tile.StaticObject.Desc.BlocksSight;
                tile.UpdateCount++;
                AddEntity(tile.StaticObject, new Vector2(x + 0.5f, y + 0.5f));

                UpdateCount++;
            }
        }

        public void UpdateStatic(int x, int y, ConnectedObject obj)
        {
            var tile = GetTile(x, y);
            if (tile != null)
            {
                if (tile.StaticObject != null)
                {
                    RemoveEntity(tile.StaticObject);
                    tile.StaticObject = null;
                }

                tile.StaticObject = obj;
                tile.BlocksSight = tile.StaticObject.Desc.BlocksSight;
                tile.UpdateCount++;
                AddEntity(tile.StaticObject, new Vector2(x + 0.5f, y + 0.5f));

                UpdateCount++;
            }
        }

        public void RemoveStatic(int x, int y)
        {
            var tile = GetTile(x, y);
            if (tile != null)
            {
                if (tile.StaticObject != null)
                {
                    RemoveEntity(tile.StaticObject);
                    tile.StaticObject = null;
                    tile.BlocksSight = false;
                    tile.UpdateCount++;

                    UpdateCount++;
                }
            }
        }

        public bool BlocksSight(int x, int y)
        {
            var tile = GetTile(x, y);
            return tile == null || tile.BlocksSight;
        }

        public Tile GetTileF(float x, float y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return null;
            return Tiles[(int)x, (int)y];
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return null;
            return Tiles[x, y];
        }

        public Entity GetEntity(int id)
        {
            if (Entities.TryGetValue(id, out var en))
                return en;
            if (Players.TryGetValue(id, out var player))
                return player;
            if (Statics.TryGetValue(id, out var st))
                return st;
            return null;
        }
        
        public bool IsUnblocked(Vector2 pos, bool spawning = false)
        {
            var tile = GetTile((int)pos.X, (int)pos.Y);
            if (tile == null || tile.Type == 255)
                return false;

            if (Resources.Type2Tile[tile.Type].NoWalk)
                return false;

            var objDesc = tile.StaticObject?.Desc;
            if (objDesc != null && (objDesc.FullOccupy || objDesc.EnemyOccupySquare || (spawning && objDesc.OccupySquare)))
                return false;
            

            return true;
        }

        public bool PlayerNearby(Vector2 pos)
        {
            return PlayerChunks.HitTest(pos, Player.SightRadius).Count > 0;
        }

        public void MoveEntity(Entity en, Vector2 to)
        {
#if DEBUG
            if (en == null)
                throw new Exception("Undefined entity.");
#endif
            if (en.Position != to)
            {
                en.Position = to;
                en.UpdateCount++;

                if (en is StaticObject)
                    return;

                var controller = en is Player || en is Decoy 
                    ? PlayerChunks : EntityChunks;
                controller.Insert(en);
            }
        }

        public virtual int AddEntity(Entity en, Vector2 at)
        {
#if DEBUG
            if (en == null)
                throw new Exception("Entity is null.");
            if (en.Id != 0)
                throw new Exception("Entity has already been added.");
#endif

            if (GetTileF(at.X, at.Y) == null)
                return -1;

            en.Id = ++NextObjectId;
            en.Parent = this;
            en.Position = at;
            MoveEntity(en, en.Position);

            if (en is StaticObject)
            {
                Statics.Add(en.Id, en as StaticObject);
                return en.Id;
            }

            if (en is Player)
            {
                Players.Add(en.Id, en as Player);
                PlayerChunks.Insert(en);
            }
            else if (en is Decoy)
            {
                Entities.Add(en.Id, en);
                PlayerChunks.Insert(en);
            }
            else
            {
                Entities.Add(en.Id, en);
                EntityChunks.Insert(en);

                if (en.Desc.Quest)
                    Quests.Add(en.Id, en);
            }

            if (en.Constant)
            {
                Constants.Add(en.Id, en);
            }

            en.Init();
            return en.Id;
        }

        public virtual void RemoveEntity(Entity en)
        {
#if DEBUG
            if (en == null)
                throw new Exception("Entity is null.");
            if (en.Id == 0)
                throw new Exception("Entity has not been added yet.");
#endif     
            if (en is StaticObject)
            {
                Statics.Remove(en.Id);
                en.Dispose();
                return;
            }

            if (en is Player)
            {
                Players.Remove(en.Id);
                PlayerChunks.Remove(en);
            }
            else if (en is Decoy)
            {
                Entities.Remove(en.Id);
                PlayerChunks.Remove(en);
            }
            else
            {
                Entities.Remove(en.Id);
                EntityChunks.Remove(en);

                if (en.Desc.Quest)
                {
                    Quests.Remove(en.Id);
                    foreach (var player in Players.Values)
                        player.TryGetNextQuest(en);
                    
                }
            }

            if (Constants.ContainsKey(en.Id))
            {
                Constants.Remove(en.Id);
            }

            en.Dispose();
        }

        public virtual void Tick()
        {
            if (IsTemplate)
                return;
            
            AliveTime += Settings.MillisecondsPerTick;
            
            if (!Persist && Players.Count <= 0 && AliveTime >= 30000)
                Manager.RemoveWorld(this);
            
            var chunks = new HashSet<Chunk>();
            foreach (Entity en in Players.Values)
            {
                for (var k = -ChunkController.ActiveRadius; k <= ChunkController.ActiveRadius; k++)
                    for (var j = -ChunkController.ActiveRadius; j <= ChunkController.ActiveRadius; j++)
                    {
                        var chunk = EntityChunks.GetChunk(en.CurrentChunk.X + k, en.CurrentChunk.Y + j);
                        if (chunk != null)
                            chunks.Add(chunk);
                    }
            }

            var entities = new HashSet<Entity>();
            entities.UnionWith(Players.Values);
            entities.UnionWith(Constants.Values);
            entities.UnionWith(EntityChunks.GetActiveChunks(chunks));

            //Send Updates to players
            foreach (var player in Players.Values)
                player.SendUpdate();

            //Tick logic first
            foreach (var en in entities) 
                if (en.TickEntity())
                    en.Tick();

            //Send NewTick to players
            foreach (var player in Players.Values)
                player.SendNewTick();

            //Clear new stats
            foreach (var en in entities)
                if (en.TickEntity())
                    en.NewSVs.Clear();

            ChatMessages.Clear();
        }

        public virtual bool AllowedAccess(Client client)
        {
            return !Closed || client.Account.Ranked;
        }

        public virtual World GetInstance(Client client)
        {
            if (IsTemplate)
            {
                var world = Manager.AddWorld(Resources.Worlds[Name], client);
                world.IsTemplate = false;
                world.Portal = null;
                return world;
            }
            return this;
        }

        private void UpdatePortalState()
        {
            if (Portal == null)
                return;

            Portal.Usable = !Closed;
        }

        public void QuakeToWorld(World newWorld)
        {
            if (!Persist || this is Realm)
                Closed = true;

            foreach (var player in Players.Values)
                player.Client.Send(GameServer.ShowEffect(ShowEffectIndex.Jitter, 0, 0));

            Manager.AddTimedAction(5000, () =>
            {
                if (newWorld is OryxCastle castle)
                    castle.IncomingPlayers = Players.Count;
                
                foreach (var player in Players.Values)
                    player.Client.Send(GameServer.Reconnect(newWorld.Id));
            });
        }

        public virtual void Dispose()
        {
            foreach (var en in Entities.Values) RemoveEntity(en);
            foreach (Entity en in Players.Values) RemoveEntity(en);
            foreach (Entity en in Statics.Values) RemoveEntity(en);

            PlayerChunks.Dispose();
            EntityChunks.Dispose();

            ChatMessages.Clear();

            Tiles = null;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            if (obj == null || !(obj is World))
                throw new Exception("Invalid object comparison.");
#endif
            return Id == (obj as World).Id;
        }
        
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
