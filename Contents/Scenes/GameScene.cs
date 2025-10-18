using Mantodea;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Scenes;
using Mantodea.Contents.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Rooms;
using static MonoShard.StoneShard;

namespace MonoShard.Contents.Scenes
{
    public class GameScene : Scene
    {
        public GameScene()
        {
            Timer = new();
        }

        public Room CurrentRoom;

        public Timer Timer;

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawRectangle(new(0, 0, Core.GameWidth, Core.GameHeight), GameColors.RoomDark, layerDepth: 1);

            CurrentRoom?.Draw(spriteBatch, gameTime);

            spriteBatch.DrawRectangle(new(0, 0, Core.GameWidth, Core.GameHeight), GameColors.RoomDark * (Timer[0] / 255), layerDepth: 0);

            spriteBatch.Rebegin(samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CurrentRoom.Update(gameTime);
        }
    }
}
