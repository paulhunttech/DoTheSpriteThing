using ImageMagick;

namespace DoTheSpriteThing
{
    internal interface IImageProcessor
    {
        void CreateSprite(MagickImageCollection spriteImages, string spriteFilename);
    }
}