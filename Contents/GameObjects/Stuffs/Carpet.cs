using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects.Stuffs
{
    public class Carpet(Room room) : RoomObject(room)
    {
        public override int Width => Texture.Width;

        public override int Height => Texture.Height;

        public override float Alpha => 1;

        public override float ZIndex => 0.001f;
    }
}
