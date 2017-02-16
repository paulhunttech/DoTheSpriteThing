using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var placeholderImages = new List<PlaceholdeImage>();
            
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

                        PlaceholdeImage placeholdeImage = placeholderImages.FirstOrDefault(x => x.Key == byteArrayImage.PlaceholderImageFilename);

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
                        else if (placeholdeImage == null)
                        {
                            imageForSprite = new MagickImage(new FileInfo(byteArrayImage.PlaceholderImageFilename)) { Quality = spriteSettings.Quality };

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            imageCollection.Add(imageForSprite);

                            var placeholderImage = new PlaceholdeImage
                            {
                                Key = byteArrayImage.PlaceholderImageFilename,
                                Top = imageTop,
                                Height = imageForSprite.Height,
                                Width = imageForSprite.Width
                            };

                            placeholderImages.Add(placeholderImage);
                            
                            useImageTop = imageTop;
                            useImageHeight = imageForSprite.Height;
                            useImageWidth = imageForSprite.Width;

                            imageAdded = true;
                        }
                        else
                        {
                            useImageTop = placeholdeImage.Top;
                            useImageHeight = placeholdeImage.Height;
                            useImageWidth = placeholdeImage.Width;
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
