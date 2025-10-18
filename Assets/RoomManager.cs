using Mantodea;
using Mantodea.Assets;
using Mantodea.Contents.Extensions;
using MonoShard.Contents.GameObjects;
using MonoShard.Contents.Rooms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace MonoShard.Assets
{
    public class RoomManager : AssetManager<Room>
    {
        public Dictionary<string, Room> Rooms = [];

        public override void LoadOne(string dir, Dictionary<string, Room> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, dir);

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path, "*.json", SearchOption.AllDirectories))
                {
                    var room = new Room();

                    var data = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file));

                    room.Background = StoneShard.TextureManager.Sprites[data["Background"].ToString()].GetClone();

                    room.Collision = data["Collision"].ToObject<int[,]>();

                    room.TileWidth = room.Collision.GetLength(0);

                    room.TileHeight = room.Collision.GetLength(1);

                    foreach (var obj in data["Barriers"])
                    {
                        var barrier = new Barrier(room)
                        {
                            Position = new((int)obj["X"] * 2, (int)obj["Y"] * 2),
                            Texture = TextureManager.GetSprite(obj["Sprite"].ToString()),
                        };

                        barrier.Texture.Frame = obj["ImageIndex"].ToObject<int>();

                        room.RoomObjects.Add(barrier);
                    }

                    foreach (var fore in data["Foregrounds"])
                    {
                        var transparentFore = new Foreground(room)
                        {
                            Position = new((int)fore["X"] * 2, (int)fore["Y"] * 2),
                            Texture = TextureManager.GetSprite(fore["Sprite"].ToString()),
                        };

                        transparentFore.Texture.Frame = fore["ImageIndex"].ToObject<int>();

                        room.RoomObjects.Add(transparentFore);
                    }

                    dictronary.Add(Path.GetFileNameWithoutExtension(file), room);
                }
            }
        }

        public Room this[string index] => Rooms.TryGetValue(index, out Room value) ? value : null;
    }
}
