using Mantodea;
using Mantodea.Assets;
using Mantodea.Contents.Graphics;
using Mantodea.Extensions;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Graphics;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MonoShard.Assets
{
    public class TextureManager : AssetManager<StoneShardTextureRegion>
    {
        public ConcurrentDictionary<string, StoneShardTextureRegion> Textures = [];

        public Dictionary<string, StoneShardSprite> Sprites = [];

        public override void LoadOne(string dir, ConcurrentDictionary<string, StoneShardTextureRegion> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, dir);

            if (Directory.Exists(path))
            {
                var pngFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

                var textureRegions = File.ReadAllText(Path.Combine(path, "TextureRegions.json"));

                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Region>>>(textureRegions);

                Parallel.ForEach(pngFiles, file =>
                {
                    var tex = Texture2D.FromFile(Core.Instance.GraphicsDevice, file).Scale(2);

                    if (data.TryGetValue(Path.GetFileNameWithoutExtension(file), out var regions))
                    {
                        foreach (var pair in regions)
                        {
                            var region = pair.Value;

                            dictronary.TryAdd(pair.Key, StoneShardTextureRegion.FromRegion(region, tex));
                        }
                    }
                });

                var spriteDataPath = $"{path}/TextureItems.json";

                if (File.Exists(spriteDataPath))
                {
                    var spriteJsonData = Task.Run(() => File.ReadAllText(spriteDataPath)).Result;

                    var spriteData = JsonConvert.DeserializeObject<Dictionary<string, (int, int, List<string>)>>(spriteJsonData);

                    foreach (var pair in spriteData)
                    {
                        if (!Sprites.ContainsKey(pair.Key))
                        {
                            Sprites.Add(pair.Key, new(pair.Value.Item1 * 2, pair.Value.Item2 * 2,
                                [.. pair.Value.Item3.Where(dictronary.ContainsKey).Select(v => dictronary[v])]));
                        }
                    }
                }
            }
        }

        public StoneShardTextureRegion this[string index] => Textures.TryGetValue(index, out StoneShardTextureRegion value) ? value : null;

        public static StoneShardSprite GetSprite(string name)
        {
            return StoneShard.TextureManager.Sprites.TryGetValue(name, out StoneShardSprite sprite) ? sprite.GetClone() : null;
        }
    }
}
