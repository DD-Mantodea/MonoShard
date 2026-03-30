using MonoShard.Contents.Attributes;
using MonoShard.Contents.Rooms;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonoShard.Contents.GameObjects.Stuffs
{
    [AutoRegister]
    public abstract class Stuff(Room room, float zIndexModify) : RoomObject(room)
    {
        public override int Width => Texture?.Width ?? 0;

        public override int Height => Texture?.Height ?? 0;

        public override float Alpha => 1;

        public override float ZIndex => 1 - Position.Y / 1000 / StoneShard.TileSize + zIndexModify;

        public virtual float ZIndexModify => zIndexModify;

        public abstract void Register();

        public static Dictionary<Regex, Func<Room, float, Stuff>> StuffFactories = [];
    }

    public class DefaultStuff(Room room, float zIndexModify) : Stuff(room, zIndexModify)
    {
        public DefaultStuff() : this(null, 0) { }

        public override void Register() { }
    }
}

