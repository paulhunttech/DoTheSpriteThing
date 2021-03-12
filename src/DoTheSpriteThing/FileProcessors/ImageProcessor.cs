using DoTheSpriteThing.FileProcessors.Interfaces;
using ImageMagick;

namespace DoTheSpriteThing.FileProcessors
{
    internal class ImageProcessor : IImageProcessor
    {
        public void CreateSprite(MagickImageCollection spriteImages, ISpriteSettings spriteSettings)
        {
            using var result = spriteImages.AppendVertically();
            spriteSettings.SaveSpriteAsync(result);
        }
    }
}