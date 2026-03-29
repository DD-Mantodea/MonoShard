using Mantodea;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoShard.Assets;
using MonoShard.Contents.GameObjects.TileMapObjects.Entities;
using MonoShard.Contents.GameObjects.TileMapObjects.Entities.Players;
using MonoShard.Contents.Scenes;
using System.Threading.Tasks;

namespace MonoShard
{
    public class StoneShard : Core
    {
        public StoneShard() : base(1920, 1200)
        {
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            TextureManager = new();

            RoomManager = new();

            FontManager = new();

            GameScene = new();
        }

        public const int TileSize = 52;

        public static TextureManager TextureManager;

        public static RoomManager RoomManager;

        public static FontManager FontManager;

        public static GameScene GameScene;

        public bool AssetLoaded = false;

        protected async override void Initialize()
        {
            base.Initialize();

            await Task.Run(async () =>
            {
                await TextureManager.Load();

                await RoomManager.Load();

                await FontManager.Load();

                AssetLoaded = true;
            });

            GameScene.CurrentRoom = RoomManager["r_taverninside1floor"];

            Player.LocalPlayer = new Jonna(GameScene.CurrentRoom);

            Player.LocalPlayer.GoToRoom(GameScene.CurrentRoom, new(6, 11));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, sortMode: SpriteSortMode.BackToFront);

            if (AssetLoaded)
                GameScene.Draw(SpriteBatch, gameTime);

            SpriteBatch.End();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GameScene.Update(gameTime);
        }
    }

    public class GameColors
    {
        public static Color UIDark = new(6, 0, 16);

        public static Color RoomDark = new(17, 16, 26);
    }
}
