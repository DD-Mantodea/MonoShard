using FontStashSharp;
using Mantodea;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Scenes;
using Mantodea.Contents.UI;
using Mantodea.Contents.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.GameObjects.TileMapObjects.Entities;
using MonoShard.Contents.Rooms;

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

            spriteBatch.DrawString(StoneShard.FontManager["SSFont", 20], "InRoom: " + CurrentRoom.MouseTile.ToString(), UserInput.GetMousePos() + new Vector2(30, 10), Color.White);

            spriteBatch.DrawString(StoneShard.FontManager["SSFont", 20], "Player: " + Player.LocalPlayer.TilePosition.ToString(), UserInput.GetMousePos() + new Vector2(30, 30), Color.White);

            spriteBatch.DrawString(StoneShard.FontManager["SSFont", 20], "PathIndex: " + Player.LocalPlayer.PathIndex.ToString(), UserInput.GetMousePos() + new Vector2(30, 50), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CurrentRoom?.Update(gameTime);
        }
    }
}
