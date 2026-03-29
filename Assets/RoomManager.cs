using Mantodea;
using Mantodea.Assets;
using Mantodea.Contents.Extensions;
using MonoShard.Contents.GameObjects;
using MonoShard.Contents.GameObjects.Stuffs;
using MonoShard.Contents.Rooms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonoShard.Assets
{
    public class RoomManager : AssetManager<Room>
    {
        public ConcurrentDictionary<string, Room> Rooms = [];

        public override void LoadOne(string dir, ConcurrentDictionary<string, Room> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, dir);

            if (Directory.Exists(path))
            {
                var jsonFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);

                Parallel.ForEach(jsonFiles, file =>
                {
                    var jsonData = Task.Run(() => File.ReadAllText(file)).Result;

                    var data = JsonConvert.DeserializeObject<JObject>(jsonData);

                    var room = new Room
                    {
                        Background = StoneShard.TextureManager.Sprites[data["Background"].ToString()].GetClone(),

                        Collision = data["Collision"].ToObject<int[,]>()
                    };

                    room.TileWidth = room.Collision.GetLength(0);
                    room.TileHeight = room.Collision.GetLength(1);

                    if (data["Barriers"] != null)
                    {
                        foreach (var obj in data["Barriers"])
                        {
                            var barrier = new Barrier(room, 0)
                            {
                                Position = new((int)obj["X"] * 2, (int)obj["Y"] * 2),
                                Texture = TextureManager.GetSprite(obj["Sprite"].ToString()),
                            };

                            barrier.Texture.Frame = obj["ImageIndex"].ToObject<int>();
                            room.RoomObjects.Add(barrier);
                        }
                    }

                    if (data["Foregrounds"] != null)
                    {
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
                    }

                    lock (dictronary)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);

                        dictronary.TryAdd(fileName, room);
                    }
                });
            }
        }

        public Room this[string index] => Rooms.TryGetValue(index, out Room value) ? value : null;
    }
}
