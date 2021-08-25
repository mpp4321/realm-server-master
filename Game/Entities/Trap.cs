using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Entities
{
    public class Trap : Entity
    {
        public Player Player;
        public float Radius;
        public int Damage;
        public ConditionEffectDesc[] CondEffects;

        public Trap(Player player, float radius, int damage, ConditionEffectDesc[] effects) : base(0x070f, 10000)
        {
            Player = player;
            Radius = radius;
            Damage = damage;
            CondEffects = effects;
        }

        public override void Tick()
        {
            if (Player.Parent == null)
            {
                Parent.RemoveEntity(this);
                return;
            }

            var elapsed = 10000 - Lifetime.Value;
            if (elapsed % 1000 == 0)
            {
                var ring = GameServer.ShowEffect(ShowEffectIndex.Ring,
                    Id, 0xff9000ff, new Vector2(Radius / 2, 0));
                foreach (var j in Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                    if (j is Player k && (k.Client.Account.Effects || k.Equals(Player)))
                        k.Client.Send(ring);
            }

            if (this.GetNearestEnemy(Radius) != null)
            {
                OnLifeEnd();
                Parent.RemoveEntity(this);
                return;
            }

            base.Tick();
        }

        public override void OnLifeEnd()
        {
            var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, 0xff9000ff, new Vector2(Radius, 0));

            foreach (var j in Parent.EntityChunks.HitTest(Position, Radius))
                if (j is Enemy k && 
                    !k.HasConditionEffect(ConditionEffectIndex.Invincible) && 
                    !k.HasConditionEffect(ConditionEffectIndex.Stasis))
                    k.Damage(Player, Damage, CondEffects, false, true);

            foreach (var j in Parent.PlayerChunks.HitTest(Position, Player.SightRadius))
                if (j is Player k && (k.Client.Account.Effects || k.Equals(Player)))
                    k.Client.Send(nova);
        }
    }
}
