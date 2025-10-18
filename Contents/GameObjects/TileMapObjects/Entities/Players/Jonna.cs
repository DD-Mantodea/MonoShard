using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects.TileMapObjects.Entities.Players
{
    public class Jonna : Player
    {
        public Jonna(Room room) : base(room, "s_JonnaHead")
        {
            SetBody("s_human_female");

            SetHeadOffset(new(0, 0));
            
            SetBodyOffset(new(0, 2));
        }
    }
}
