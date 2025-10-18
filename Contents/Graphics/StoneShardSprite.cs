using Mantodea.Contents.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoShard.Contents.Graphics
{
    public class StoneShardSprite(int originX, int originY, List<StoneShardTextureRegion> textures, int frame = 0)
    {
        public StoneShardSprite() : this(0, 0, []) { }

        public List<StoneShardTextureRegion> Textures { get; set; } = textures;

        public int Frame = frame;

        public int Width => Textures[Frame].Width;

        public int Height => Textures[Frame].Height;

        private Vector2 _drawOffset = new(originX, originY);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation = 0, Vector2 origin = default, Vector2 scale = default, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1)
        {
            spriteBatch.Draw(Textures[Frame], position - _drawOffset, color, rotation, origin, scale == default ? Vector2.One : scale, effects, layerDepth);
        }

        public StoneShardSprite GetClone()
        {
            return new((int)_drawOffset.X, (int)_drawOffset.Y, Textures, 0);
        }

        public static StoneShardSprite FromSingle(Texture2D texture)
        {
            return new StoneShardSprite(0, 0, [StoneShardTextureRegion.FromSingle(texture)]);
        }

        public void SetOffset(Vector2 vec)
        {
            _drawOffset = vec;
        }

        public StoneShardSprite RemoveOffset()
        {
            _drawOffset = Vector2.Zero;

            return this;
        }
    }
}
