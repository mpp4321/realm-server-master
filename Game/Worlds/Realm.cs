using System.Collections.Generic;
using RotMG.Common;
using RotMG.Game.Entities;

namespace RotMG.Game.Worlds
{
    public sealed partial class Realm : World
    {
        private static readonly Queue<string> WorldNames = new Queue<string>(
    new []
        {
            "Pirate", "Deathmage", "Spectre", "Titan", "Gorgon", "Kraken", "Satyr", "Drake", "Chimera", "Dragon",
            "Wyrm", "Hydra", "Leviathan", "Minotaur", "Mummy", "Reaper", "Phoenix", "Giant", "Unicorn", "Harpy",
            "Gargoyle", "Snake", "Cube", "Goblin", "Hobbit", "Skeleton", "Scorpion", "Bat", "Ghost", "Slime", "Lich",
            "Orc", "Imp", "Spider", "Demon", "Blob", "Golem", "Sprite", "Flayer", "Ogre", "Djinn", "Cyclops",
            "Beholder", "Medusa"
        });

        public new readonly WMap Map;

        public Realm(Map map, WorldDesc desc) : base(map, desc)
        {
            Map = map as WMap;
            DisplayName = WorldNames.Dequeue();

            foreach (var terrain in Map.Terrains.Keys)
                _enemies[terrain] = new List<Enemy>();
            
            InitMobs();
        }

        public override int AddEntity(Entity e, Vector2 at)
        {
            if(e is Enemy en && en.Terrain != Terrain.None)
            {
                this._enemyCount[en.Terrain]++;
            }
            return base.AddEntity(e, at);
        }

        public override void Tick()
        {
            if (Closed && Players.Count <= 0)
                Manager.RemoveWorld(this);

            if (AliveTime % 60000 == 0)
                EnsurePopulation();

            if (AliveTime % 20000 == 0)
                OryxTaunt();

            // Closes every 20 mins AliveTime > 0 needed because it'll close at AliveTime = 0

            if (AliveTime % 900000 == 0 && AliveTime > 0)
            {
                // Oryx warn close
                foreach (var c in Manager.Clients.Values)
                {
                    if (c.Player == null)
                        continue;
                    c.Player.SendInfo("The realm is closing in 5 minutes; mortals prepare to meet your demise!");
                }
            }

            if (AliveTime % 1200000 == 0 && AliveTime > 0)
                Close();
            
            base.Tick();
        }

        public void Close()
        {
            if (Closed) return;
            Closed = true;
            foreach (var player in Players.Values)
            {
                player.SendInfo("I HAVE CLOSED THIS REALM!");
                player.SendInfo("YOU WILL NOT LIVE TO SEE THE LIGHT OF DAY!");
            }

            Manager.AddTimedAction(60000, () =>
            {
                foreach (var player in Players.Values)
                {
                    player.SendInfo("MY MINIONS HAVE FAILED ME!");
                    player.SendInfo("BUT NOW YOU SHALL FEEL MY WRATH!");
                    player.SendInfo("COME MEET YOUR DOOM AT THE WALLS OF MY CASTLE!");
                }

                QuakeToWorld(Manager.AddWorld(Resources.Worlds["OryxCastle"]));
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            WorldNames.Enqueue(DisplayName);
        }
    }
}