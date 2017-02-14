using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageMagick;

namespace DoTheSpriteThing
{
    public class SpriteManager
    {
        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>
        public void CreateSprite(IReadOnlyCollection<IImage> images, SpriteSettings spriteSettings)
        {
            var noImageRequired = false;
            var noImageTop = 0;
            var noImageHeight = 0;
            var noImageWidth = 0;

            using (var imageCollection = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var imageTop = 0;

                foreach (IImage image in images)
                {
                    if (image is FileImage)
                    {
                        var imageFile = (FileImage)image;
                        var imageForSprite = new MagickImage(imageFile.FilePath) { Quality = spriteSettings.Quality };

                        if (imageFile.Resize)
                        {
                            imageForSprite.Resize(new MagickGeometry($"{imageFile.ResizeToHeight}x{imageFile.ResizeToWidth}!"));
                        }

                        imageCollection.Add(imageForSprite);
                        css.AppendLine(GetCss(Path.GetFileName(imageFile.FilePath.FullName).Replace(".", "-"), imageForSprite.Height, imageForSprite.Width, spriteSettings.SpriteUrl, imageTop));
                        imageTop += imageForSprite.Height;
                    }
                    else if (image is ByteArrayImage)
                    {
                        var byteArrayImage = (ByteArrayImage)image;

                        int useImageTop;
                        int useImageHeight;
                        int useImageWidth;
                        var imageAdded = false;
                        MagickImage imageForSprite;

                        if (byteArrayImage.ImageData != null)
                        {
                            imageForSprite = new MagickImage(byteArrayImage.ImageData) { Quality = spriteSettings.Quality };

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            imageCollection.Add(imageForSprite);
                            useImageTop = imageTop;
                            useImageHeight = imageForSprite.Height;
                            useImageWidth = imageForSprite.Width;
                            imageAdded = true;
                        }
                        else if (!noImageRequired)
                        {
                            noImageRequired = true;
                            imageForSprite = new MagickImage(new FileInfo(byteArrayImage.NoImageFilename)) { Quality = spriteSettings.Quality };

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            imageCollection.Add(imageForSprite);
                            noImageTop = imageTop;
                            noImageHeight = imageForSprite.Height;
                            noImageWidth = imageForSprite.Width;
                            useImageTop = imageTop;
                            useImageHeight = imageForSprite.Height;
                            useImageWidth = imageForSprite.Width;
                            imageAdded = true;
                        }
                        else
                        {
                            useImageTop = noImageTop;
                            useImageHeight = noImageHeight;
                            useImageWidth = noImageWidth;
                        }

                        css.AppendLine(GetCss(byteArrayImage.Key, useImageHeight, useImageWidth, spriteSettings.SpriteUrl, useImageTop));

                        if (imageAdded)
                        {
                            imageTop += useImageHeight;
                        }
                    }
                }

                GenerateSpriteAndCss(imageCollection, css, spriteSettings.SpriteFilename, spriteSettings.CssFilename);
            }
        }        

        private static string GetCss(string imageKey, int imageHeight, int imageWidth, string spriteUrl, int imageTop) => $"#{imageKey} {{ height: {imageHeight}px; width: {imageWidth}px; background-image: url('{spriteUrl}'); background-position: 0px -{imageTop}px; }}";

        private static void GenerateSpriteAndCss(MagickImageCollection images, StringBuilder css, string spriteFilename, string cssFilename)
        {            
            using (MagickImage result = images.AppendVertically())
            {
                result.Write(spriteFilename);
            }

            File.WriteAllText(cssFilename, css.ToString());            
        }
    }
}
