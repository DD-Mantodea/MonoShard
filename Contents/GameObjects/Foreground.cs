using Mantodea;
using Mantodea.Contents;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects
{
    public class Foreground(Room room) : RoomObject(room)
    {
        public override int Height => Texture.Height;

        public override int Width => Texture.Width;

        public override float Alpha => 79f / 255;

        public override float ZIndex => 1 - (Position.Y + Height - 1) / 1000 / StoneShard.TileSize;
    }
}
