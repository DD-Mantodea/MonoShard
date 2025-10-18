using Mantodea;
using Mantodea.Assets;
using Mantodea.Contents.Graphics;
using Mantodea.Extensions;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Contents.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoShard.Assets
{
    public class TextureManager : AssetManager<StoneShardTextureRegion>
    {
        public Dictionary<string, StoneShardTextureRegion> Textures = [];

        public Dictionary<string, StoneShardSprite> Sprites = [];

        public override void LoadOne(string dir, Dictionary<string, StoneShardTextureRegion> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, dir);

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
                {
                    var dataFile = file.Replace(".png", ".json");

                    var tex = Texture2D.FromFile(Core.Instance.GraphicsDevice, file).Scale(2);

                    var data = JsonConvert.DeserializeObject<Dictionary<string, Region>>(File.ReadAllText(dataFile));

                    foreach (var pair in data)
                    {
                        var region = pair.Value;

                        dictronary.Add(pair.Key, StoneShardTextureRegion.FromRegion(region, tex));
                    }
                }

                var spriteData = JsonConvert.DeserializeObject<Dictionary<string, (int, int, List<string>)>>(File.ReadAllText($"{path}/TextureItems.json"));

                foreach (var pair in spriteData)
                    Sprites.Add(pair.Key, new(pair.Value.Item1 * 2, pair.Value.Item2 * 2, [.. pair.Value.Item3.Where(dictronary.ContainsKey).Select(v => dictronary[v])]));
            }
        }

        public StoneShardTextureRegion this[string index] => Textures.TryGetValue(index, out StoneShardTextureRegion value) ? value : null;
    
        public static StoneShardSprite GetSprite(string name)
        {
            return StoneShard.TextureManager.Sprites.TryGetValue(name, out StoneShardSprite sprite) ? sprite.GetClone() : null;
        }
    }
}
