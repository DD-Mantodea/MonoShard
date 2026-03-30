using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonoShard.Contents.GameObjects.Stuffs
{
    public class Decoration(Room room, float zIndexModify) : Stuff(room, zIndexModify)
    {
        public Decoration() : this(null, 0) { }

        public override float ZIndex => 1f;

        public override void Register()
        {
            StuffFactories.Add(new("(carpet|pelt)"), (room, zIndexModify) => new Decoration(room, zIndexModify));
        }
    }
}
