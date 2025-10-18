using Mantodea.Contents.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.Graphics
{
    public class StoneShardTextureRegion(Texture2D texture, int x, int y, int width, int height, int boundingWidth, int boundingHeight, int targetX, int targetY, int targetWidth, int targetHeight) : TextureRegion(texture, x, y, width, height)
    {
        public override int Width => boundingWidth;

        public override int Height => boundingHeight;

        public Vector2 DrawOffset => _drawOffset;

        private Vector2 _drawOffset = new(targetX, targetY);

        internal void SetDrawOffset(int x, int y)
        {
            _drawOffset = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.Draw(spriteBatch, position + _drawOffset, color, rotation, origin, scale, effects, layerDepth);
        }

        public static StoneShardTextureRegion FromRegion(Region region, Texture2D texture)
        {
            return new StoneShardTextureRegion(texture, region.X * 2, region.Y * 2, region.Width * 2, region.Height * 2, region.BoundingWidth * 2, region.BoundingHeight * 2, region.TargetX * 2, region.TargetY * 2, region.TargetWidth * 2, region.TargetHeight * 2);
        }

        public static StoneShardTextureRegion FromSingle(Texture2D texture)
        {
            return new StoneShardTextureRegion(texture, 0, 0, texture.Width * 2, texture.Height * 2, texture.Width * 2, texture.Height * 2, 0, 0, texture.Width * 2, texture.Height * 2);
        }
    }

    public record Region(int X, int Y, int Width, int Height, int BoundingWidth, int BoundingHeight, int TargetX, int TargetY, int TargetWidth, int TargetHeight);
}
