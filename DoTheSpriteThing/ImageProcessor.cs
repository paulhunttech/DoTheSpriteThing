using ImageMagick;

namespace DoTheSpriteThing
{
    internal class ImageProcessor : IImageProcessor
    {
        public void CreateSprite(MagickImageCollection spriteImages, string spriteFilename)
        {
            using (MagickImage result = spriteImages.AppendVertically())
            {
                result.Write(spriteFilename);
            }
        }
    }
}