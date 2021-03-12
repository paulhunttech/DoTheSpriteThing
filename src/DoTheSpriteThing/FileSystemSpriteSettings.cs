using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace DoTheSpriteThing
{
    /// <summary>
    /// The sprite settings.
    /// </summary>
    public class FileSystemSpriteSettings : ISpriteSettings
    {
        /// <summary>
        /// Create the sprite settings.
        /// </summary>
        /// <param name="spriteFilename">The name of the sprite file that will be created or overwritten.</param>
        /// <param name="spriteUrl">The URL of the sprite file.</param>
        /// <param name="cssFilename">The name of the CSS file that will be created or overwritten.</param>                
        public FileSystemSpriteSettings(string spriteFilename, string spriteUrl, string cssFilename)
        {
            SpriteFilename = spriteFilename;
            SpriteUrl = spriteUrl;
            CssFilename = cssFilename;
        }

        /// <summary>
        /// The name of the CSS file that will be created or overwritten.
        /// </summary>
        public string CssFilename { get; }

        /// <summary>
        /// The name of the sprite file that will be created or overwritten.
        /// </summary>
        public string SpriteFilename { get; }

        /// <summary>
        /// The URL of the sprite file.
        /// </summary>
        public string SpriteUrl { get; }

        public Task SaveSpriteAsync(IMagickImage sprite)
        {
            sprite.Write(SpriteFilename);
            return Task.CompletedTask;
        }

        public Task SaveCssAsync(string css)
        {
            File.WriteAllText(CssFilename, css);
            return Task.CompletedTask;
        }
    }
}