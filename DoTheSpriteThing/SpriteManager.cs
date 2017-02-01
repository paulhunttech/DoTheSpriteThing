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
                    imageCollection.Add(new MagickImage(new FileInfo(fileName)));
                    css.AppendLine(GetCss(fileName, spriteSettings.ImageHeight, spriteSettings.ImageWidth, spriteSettings.SpriteUrl, imageTop));
                    imageTop += spriteSettings.ImageHeight;
                }

                GenerateSpriteAndCss(imageCollection, css, spriteSettings.SpriteFilename, spriteSettings.CssFilename, spriteSettings.ImageHeight, spriteSettings.ImageWidth);                             
            }
        }

        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite. The key is the ID of the HTML element in which to display the image and the value is the image data.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>            
        public void CreateSprite(IReadOnlyDictionary<string, byte[]> images, SpriteSettings spriteSettings)
        {
            using (var imageCollection = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var imageTop = 0;

                foreach (KeyValuePair<string, byte[]> image in images.Where(x => x.Value != null))
                {
                    imageCollection.Add(new MagickImage(image.Value));
                    css.AppendLine(GetCss(image.Key, spriteSettings.ImageHeight, spriteSettings.ImageWidth, spriteSettings.SpriteUrl, imageTop));
                    imageTop += spriteSettings.ImageHeight;
                }

                GenerateSpriteAndCss(imageCollection, css, spriteSettings.SpriteFilename, spriteSettings.CssFilename, spriteSettings.ImageHeight, spriteSettings.ImageWidth);
            }
        }

        private static string GetCss(string imageKey, int imageHeight, int imageWidth, string spriteUrl, int imageTop) => $"#{imageKey} {{ height: {imageHeight}px; width: {imageWidth}px; background-image: url('{spriteUrl}'); background-position: 0px -{imageTop}px; }}";

        private static void GenerateSpriteAndCss(MagickImageCollection images, StringBuilder css, string spriteFilename, string cssFilename, int imageHeight, int imageWidth)
        {            
            var montageSettings = new MontageSettings
            {
                Geometry = new MagickGeometry(imageWidth, imageHeight),
                TileGeometry = new MagickGeometry(1, images.Count)
            };

            using (MagickImage result = images.Montage(montageSettings))
            {
                result.Write(spriteFilename);
            }

            File.WriteAllText(cssFilename, css.ToString());            
        }
    }
}
