using Mantodea;
using Mantodea.Contents;
using Mantodea.Contents.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects
{
    public class OpaqueForeground(Room room) : RoomObject(room)
    {
        public override int Height => Texture.Height;

        public override int Width => Texture.Width;

        public override float Alpha => 1f;

        public override float ZIndex => 0;
    }

    public class TransparentForeground(Room room) : RoomObject(room)
    {
        public override int Height => Texture.Height;

        public override int Width => Texture.Width;

        public override float Alpha => 79f / 255;

        public override float ZIndex => 0;
    }
}
