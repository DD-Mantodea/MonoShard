using Mantodea.Contents;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Graphics;
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
    public class TileMapObject(Room room) : RoomObject(room)
    {
        public override int Width => Texture.Width;

        public override int Height => Texture.Height;

        public Vector2 TilePosition { get; set; }

        public override Vector2 Position => TilePosition * StoneShard.TileSize + Room?.Position ?? Vector2.Zero;

        public override float ZIndex => 1 - TilePosition.Y / 1000;
    }
}
