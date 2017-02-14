﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ImageMagick;

namespace DoTheSpriteThing
{
    public class SpriteManager
    {
        /// <summary>
        /// Create a sprite image from a folder of images and the CSS to render each image.
        /// </summary>
        /// <param name="imagesFolderPath">The path of the folder that contains the images to include in the sprite.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>                 
        public void CreateSprite(string imagesFolderPath, SpriteSettings spriteSettings)
        {
            using (var imageCollection = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var imageTop = 0;

                foreach (string fileName in Directory.GetFiles(imagesFolderPath))
                {
                    var imageForSprite = new MagickImage(new FileInfo(fileName)) { Quality = spriteSettings.Quality };                    
                    imageForSprite.Resize(new MagickGeometry($"{spriteSettings.ImageWidth}x{spriteSettings.ImageWidth}!"));
                    imageCollection.Add(imageForSprite);                    
                    css.AppendLine(GetCss(Path.GetFileName(fileName).Replace(".", "-"), spriteSettings.ImageHeight, spriteSettings.ImageWidth, spriteSettings.SpriteUrl, imageTop));
                    imageTop += spriteSettings.ImageHeight;
                }

                GenerateSpriteAndCss(imageCollection, css, spriteSettings.SpriteFilename, spriteSettings.CssFilename);                             
            }
        }

        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite. The key is the ID of the HTML element in which to display the image and the value is the image data.</param>
        /// <param name="noImageFilename">The name of the image file to use when the image data is null.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>            
        public void CreateSprite(IReadOnlyDictionary<string, byte[]> images, string noImageFilename, SpriteSettings spriteSettings)
        {
            var noImageRequired = false;
            var noImageTop = 0;

            using (var imageCollection = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var imageTop = 0;

                foreach (KeyValuePair<string, byte[]> image in images)
                {
                    int useImageTop;
                    var imageAdded = false;

                    if (image.Value != null)
                    {
                        var imageForSprite = new MagickImage(image.Value) { Quality = spriteSettings.Quality };
                        imageForSprite.Resize(new MagickGeometry($"{spriteSettings.ImageWidth}x{spriteSettings.ImageWidth}!"));
                        imageCollection.Add(imageForSprite);
                        useImageTop = imageTop;
                        imageAdded = true;
                    }
                    else if (!noImageRequired)
                    {
                        noImageRequired = true;
                        var imageForSprite = new MagickImage(new FileInfo(noImageFilename)) { Quality = spriteSettings.Quality };
                        imageForSprite.Resize(new MagickGeometry($"{spriteSettings.ImageWidth}x{spriteSettings.ImageWidth}!"));
                        imageCollection.Add(imageForSprite);                                                
                        noImageTop = imageTop;
                        useImageTop = imageTop;
                        imageAdded = true;
                    }
                    else
                    {
                        useImageTop = noImageTop;
                    }
                    
                    css.AppendLine(GetCss(image.Key, spriteSettings.ImageHeight, spriteSettings.ImageWidth, spriteSettings.SpriteUrl, useImageTop));

                    if (imageAdded)
                    {
                        imageTop += spriteSettings.ImageHeight;
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
