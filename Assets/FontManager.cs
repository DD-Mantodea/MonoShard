using FontStashSharp;
using Mantodea;
using Mantodea.Assets;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MonoShard.Assets
{
    public class FontManager : AssetManager<FontSystem>
    {
        public ConcurrentDictionary<string, FontSystem> Fonts = new();

        public override void LoadOne(string dir, ConcurrentDictionary<string, FontSystem> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, dir);

            if (Directory.Exists(path))
            {
                Directory.GetFiles(path, "*.ttf").ToList().ForEach(file =>
                {
                    var font = new FontSystem();

                    font.AddFont(File.ReadAllBytes(file));

                    dictronary.TryAdd(Path.GetFileNameWithoutExtension(file), font);
                });
            }
        }

        public SpriteFontBase this[string index, float size]
        {
            get => Fonts[index].GetFont(size);
        }
    }
}
