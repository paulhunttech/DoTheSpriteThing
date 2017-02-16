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
            var placeholderImages = new List<PlaceholderImage>();
            
            using (var spriteImages = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var nextSpriteImageTop = 0;                
                
                foreach (IImage image in images)
                {
                    var hasImageBeenAddedToSprite = false;
                    string imageKey = string.Empty;
                    var selectedImageHeight = 0;
                    var selectedImageTop = 0;
                    var selectedImageWidth = 0;                    

                    if (image is FileImage)
                    {
                        var imageFile = (FileImage)image;
                        imageKey = Path.GetFileName(imageFile.FilePath.FullName).Replace(".", "-");

                        var imageForSprite = new MagickImage(imageFile.FilePath) { Quality = spriteSettings.Quality };

                        if (imageFile.Resize)
                        {
                            imageForSprite.Resize(new MagickGeometry($"{imageFile.ResizeToHeight}x{imageFile.ResizeToWidth}!"));
                        }

                        spriteImages.Add(imageForSprite);
                        hasImageBeenAddedToSprite = true;

                        selectedImageTop = nextSpriteImageTop;
                        selectedImageHeight = imageForSprite.Height;
                        selectedImageWidth = imageForSprite.Width;                        
                    }
                    else if (image is ByteArrayImage)
                    {
                        var byteArrayImage = (ByteArrayImage)image;
                        imageKey = byteArrayImage.Key;                                                                    

                        if (byteArrayImage.ImageData != null)
                        {
                            var imageForSprite = new MagickImage(byteArrayImage.ImageData) {Quality = spriteSettings.Quality};

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            spriteImages.Add(imageForSprite);
                            hasImageBeenAddedToSprite = true;

                            selectedImageTop = nextSpriteImageTop;
                            selectedImageHeight = imageForSprite.Height;
                            selectedImageWidth = imageForSprite.Width;                            
                        }
                        else
                        {
                            var imageForSprite = new MagickImage(new FileInfo(byteArrayImage.PlaceholderImageFilename)) { Quality = spriteSettings.Quality };

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            PlaceholderImage placeholderImage = placeholderImages.FirstOrDefault(x => x.Key == byteArrayImage.PlaceholderImageFilename && x.Width == imageForSprite.Width && x.Height == imageForSprite.Height);

                            if (placeholderImage == null)
                            {                                
                                placeholderImage = new PlaceholderImage(byteArrayImage.PlaceholderImageFilename, nextSpriteImageTop, imageForSprite.Height, imageForSprite.Width);
                                placeholderImages.Add(placeholderImage);

                                spriteImages.Add(imageForSprite);
                                hasImageBeenAddedToSprite = true;                                
                            }

                            selectedImageTop = placeholderImage.Top;
                            selectedImageHeight = placeholderImage.Height;
                            selectedImageWidth = placeholderImage.Width;
                        }
                    }
                    
                    css.AppendLine($"#{imageKey} {{ height: {selectedImageHeight}px; width: {selectedImageWidth}px; background-image: url('{spriteSettings.SpriteUrl}'); background-position: 0px -{selectedImageTop}px; }}");

                    if (hasImageBeenAddedToSprite)
                    {
                        nextSpriteImageTop += selectedImageHeight;
                    }
                }                

                using (MagickImage result = spriteImages.AppendVertically())
                {
                    result.Write(spriteSettings.SpriteFilename);
                }

                File.WriteAllText(spriteSettings.CssFilename, css.ToString());
            }
        }                
    }
}
