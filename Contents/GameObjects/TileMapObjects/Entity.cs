using Mantodea.Contents;
using Mantodea.Contents.Animations;
using Microsoft.Xna.Framework;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects.TileMapObjects
{
    public class Entity(Room room) : TileMapObject(room)
    {
        public Vector2 LastTilePos { get; set; }

        public Vector2 TargetTilePos { get; set; }

        public List<Vector2> CurrentPath = [];

        public Animation<Entity> CurrentAnimation = null;

        public bool IsMove = false;

        public int Direction;

        public void SetPos(Vector2 pos)
        {
            LastTilePos = pos;

            TilePosition = pos;

            TargetTilePos = pos;
        }
    }
}
