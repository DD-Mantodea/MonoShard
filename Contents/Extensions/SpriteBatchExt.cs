using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.Extensions
{
    public static class SpriteBatchExt
    {
        public static void Draw(this SpriteBatch spriteBatch, StoneShardSprite sprite, Vector2 position, Color color, float rotation = 0, Vector2 origin = default, Vector2 scale = default, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1)
        {
            sprite.Draw(spriteBatch, position, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
