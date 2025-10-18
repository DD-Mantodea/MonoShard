using Mantodea.Contents;
using Mantodea.Contents.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.Graphics;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects
{
    public class RoomObject(Room room) : GameObject
    {
        public virtual float ZIndex { get; set; }

        public Room Room { get; set; } = room;

        public StoneShardSprite Texture { get; set; }

        public bool UseAdditive { get; set; }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position + Room.Position, Color.White * Alpha, layerDepth: ZIndex);
        }
    }
}
