using Mantodea.Contents;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Graphics;
using Mantodea.Contents.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Graphics;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects
{
    public class Barrier(Room room) : RoomObject(room)
    {
        public override int Width => Texture.Width;

        public override int Height => Texture.Height;

        public override float ZIndex => 1 - Position.Y / 1000 / StoneShard.TileSize;
    }
}
