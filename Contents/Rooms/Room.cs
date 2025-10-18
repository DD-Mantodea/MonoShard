using Mantodea;
using Mantodea.Contents;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Graphics;
using Mantodea.Contents.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoShard.Contents.Animations;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.GameObjects;
using MonoShard.Contents.GameObjects.TileMapObjects.Entities;
using MonoShard.Contents.Graphics;
using MonoShard.Contents.Logic;
using System.Collections.Generic;
using System.Linq;

namespace MonoShard.Contents.Rooms
{
    public class Room : GameObject
    {
        public Room()
        {
            OnClickEvent.AddListener("OnClick", OnClick);

            OnRightClickEvent.AddListener("OnRightClick", OnRightClick);
        }

        public static Player LocalPlayer => Player.LocalPlayer;

        public List<RoomObject> RoomObjects { get; set; } = [];

        public int[,] Collision;

        public int TileWidth;

        public int TileHeight;

        public override int Width => TileWidth * StoneShard.TileSize;

        public override int Height => TileHeight * StoneShard.TileSize;

        public bool InBattle = false;

        public Vector2 PreviousMouseTile;

        public Vector2 MouseTile;

        public Vector2 TilePosition => (Position / StoneShard.TileSize);

        public Room LastRoom;

        public StoneShardSprite Background;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!TurnController.PlayerTurn)
                NPCsAction();

            if (CheckAllNPCsDoneAction())
                TurnController.EndNPCTurn();

            var vec = (Core.GameSize - Size) / 2;

            Position = new Vector2((int)vec.X / StoneShard.TileSize, (int)vec.Y / StoneShard.TileSize) * StoneShard.TileSize;

            PreviousMouseTile = MouseTile;

            MouseTile = new Vector2((int)(UserInput.GetMousePos().X / StoneShard.TileSize), (int)(UserInput.GetMousePos().Y / StoneShard.TileSize)) - TilePosition;
        
            foreach (var obj in RoomObjects)
                obj.Update(gameTime);

            LocalPlayer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Background, Position, Color.White, layerDepth: 1);

            foreach (var obj in RoomObjects.Where(o => !o.UseAdditive))
                obj.Draw(spriteBatch, gameTime);

            foreach (var obj in RoomObjects.Where(o => o.UseAdditive))
                obj.Draw(spriteBatch, gameTime);

            LocalPlayer?.Draw(spriteBatch, gameTime);
        }

        public bool Reachable(int x, int y)
        {
            if (x < 0 || y < 0 || x >= TileWidth || y >= TileHeight)
                return false;
            return Collision[x, y] == 0;
        }

        public bool Reachable(Vector2 vec)
        {
            if (vec.X < 0 || vec.Y < 0 || vec.X >= TileWidth || vec.Y >= TileHeight)
                return false;
            return Collision[(int)vec.X, (int)vec.Y] == 0;
        }

        public void OnClick(GameObject sender)
        {
            if (Reachable(MouseTile) && TurnController.PlayerTurn)
            {
                if (LocalPlayer.CurrentAnimation is EntityMoveChain moveChain && LocalPlayer.CurrentAnimation.MaxTime != 0)
                    moveChain.ShouldBreak = true;
                else if (!InBattle)
                {
                    var chain = new EntityMoveChain(LocalPlayer);

                    foreach (var pos in LocalPlayer.CurrentPath)
                        chain.RegisterAnimation(new EntityMoveAnimation(pos, LocalPlayer));

                    LocalPlayer.PlayAnimation(chain);
                }
            }
        }

        public void OnRightClick(GameObject sender)
        {
            if (Reachable(MouseTile) && TurnController.PlayerTurn)
            {
                var dir = (MouseTile - LocalPlayer.TilePosition).X;

                LocalPlayer.Direction = dir > 0 ? 1 : dir == 0 ? LocalPlayer.Direction : -1;

                LocalPlayer.TilePosition = MouseTile;

                LocalPlayer.LastTilePos = MouseTile;

                LocalPlayer.TargetTilePos = MouseTile;
            }
        }

        public virtual void NPCsAction()
        {
            foreach (var npc in RoomObjects.Where(o => o is NPC).Select(o => o as NPC))
            {
                npc.ActionDone = false;

                npc.DoAction();
            }
        }

        public bool CheckAllNPCsDoneAction()
        {
            foreach (var npc in RoomObjects.Where(o => o is NPC).Select(o => o as NPC))
            {
                if (npc.ActionDone) 
                    continue;
                else 
                    return false;
            }

            return true;
        }
    }
}
