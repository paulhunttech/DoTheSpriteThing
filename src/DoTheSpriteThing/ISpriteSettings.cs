using System.Threading.Tasks;
using ImageMagick;

namespace DoTheSpriteThing
{
    public interface ISpriteSettings
    {
        /// <summary>
        /// The URL of the sprite file.
        /// </summary>
        string SpriteUrl { get; }

        /// <summary>
        /// Save the sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        Task SaveSpriteAsync(IMagickImage sprite);

        /// <summary>
        /// Save the CSS
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        Task SaveCssAsync(string css);
    }
}