using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class LordoftheLostLands : ISetPiece
    {
        public int Size { get { return 5; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var entity = Entity.Resolve(Resources.Id2Object["Lord of the Lost Lands"].Type);
            world.AddEntity(entity, new Vector2(pos.X + 2.5f, pos.Y + 2.5f));
        }
    }
}
