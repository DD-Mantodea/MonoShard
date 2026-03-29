using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.Rooms;

namespace MonoShard.Contents.GameObjects.Stuffs
{
    public class Barrier(Room room, float zIndexModify) : RoomObject(room)
    {
        public override int Width => Texture.Width;

        public override int Height => Texture.Height;

        public override float Alpha => 1;

        public override float ZIndex => 1 - Position.Y / 1000 / StoneShard.TileSize + zIndexModify;

        public float ZIndexModify => zIndexModify;
    }
}

