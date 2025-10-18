using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects.TileMapObjects.Entities
{
    public abstract class NPC : Entity
    {
        public NPC(Room room) : base(room)
        {
        }

        public bool ActionDone;

        public virtual void DoAction() { }
    }
}
