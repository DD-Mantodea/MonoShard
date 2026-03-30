using Mantodea;
using Mantodea.Assets;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.GameObjects;
using MonoShard.Contents.GameObjects.Stuffs;
using MonoShard.Contents.Rooms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

                    if (data["Stuffs"] != null)
                    {
                        foreach (var obj in data["Stuffs"])
                        {
                            Stuff stuff = null;

                            foreach (var pair in Stuff.StuffFactories)
                            {
                                if (pair.Key.IsMatch(obj["Sprite"].ToString()))
                                {
                                    stuff = pair.Value(room, 0);

                                    break;
                                }
                            }

                            stuff ??= new DefaultStuff(room, 0);

                            stuff.Position = new((int)obj["X"] * 2, (int)obj["Y"] * 2);

                            stuff.Texture = TextureManager.GetSprite(obj["Sprite"].ToString());

                            stuff.Texture.Frame = obj["ImageIndex"].ToObject<int>();

                            room.RoomObjects.Add(stuff);
                        }
                    }

                    if (data["TransparentForegrounds"] != null)
                    {
                        foreach (var fore in data["TransparentForegrounds"])
                        {
                            var transparentFore = new TransparentForeground(room)
                            {
                                Position = new((int)fore["X"] * 2, (int)fore["Y"] * 2),
                                Texture = TextureManager.GetSprite(fore["Sprite"].ToString()),
                            };

                            transparentFore.Texture.Frame = fore["ImageIndex"].ToObject<int>();
                            room.RoomObjects.Add(transparentFore);
                        }
                    }

                    if (data["OpaqueForegrounds"] != null)
                    {
                        foreach (var fore in data["OpaqueForegrounds"])
                        {
                            var transparentFore = new OpaqueForeground(room)
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
