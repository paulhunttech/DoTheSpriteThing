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
            var placeholderImages = new List<IImage>
            {
                new ByteArrayImage("placeholder", File.ReadAllBytes(@"C:\Users\paul.hunt\Source\Repos\DoTheSpriteThing\DoTheSpriteThing.Testbed\wwwroot\images\facebook.png"), "placeholder")
            };

            CreateSprite(images, placeholderImages, spriteSettings);
        }

        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite.</param>
        /// <param name="placeholderImages">The list of custom placeholder images.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>
        public void CreateSprite(IReadOnlyCollection<IImage> images, IReadOnlyCollection<IImage> placeholderImages, SpriteSettings spriteSettings)
        {
            var spritePlaceholderImages = new List<SpritePlaceholderImage>();

            using (var spriteImages = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var nextSpriteImageTop = 0;                
                
                foreach (IImage image in images)
                {
                    var hasImageBeenAddedToSprite = false;                    
                    var selectedImageHeight = 0;
                    var selectedImageTop = 0;
                    var selectedImageWidth = 0;                    

                    if (image is FileImage)
                    {
                        var imageFile = (FileImage)image;
                        
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
                            string selectedPlaceholderImageKey = placeholderImages.Any(x => x.Key == byteArrayImage.PlaceholderImageKey) ? byteArrayImage.PlaceholderImageKey : placeholderImages.First().Key;
                            IImage selectedPlaceholderImage = placeholderImages.FirstOrDefault(x => x.Key == selectedPlaceholderImageKey);

                            MagickImage imageForSprite;

                            if (selectedPlaceholderImage is FileImage)
                            {                                
                                imageForSprite = new MagickImage(((FileImage)selectedPlaceholderImage).FilePath) {Quality = spriteSettings.Quality};
                            }
                            else
                            {
                                imageForSprite = new MagickImage(((ByteArrayImage)selectedPlaceholderImage).ImageData) { Quality = spriteSettings.Quality };
                            }

                            if (image.Resize)
                            {
                                imageForSprite.Resize(new MagickGeometry($"{image.ResizeToWidth}x{image.ResizeToHeight}!"));
                            }

                            SpritePlaceholderImage spritePlaceholderImage = spritePlaceholderImages.FirstOrDefault(x => x.Key == selectedPlaceholderImageKey && x.Width == imageForSprite.Width && x.Height == imageForSprite.Height);

                            if (spritePlaceholderImage == null)
                            {                                
                                spritePlaceholderImage = new SpritePlaceholderImage(selectedPlaceholderImageKey, nextSpriteImageTop, imageForSprite.Height, imageForSprite.Width);
                                spritePlaceholderImages.Add(spritePlaceholderImage);

                                spriteImages.Add(imageForSprite);
                                hasImageBeenAddedToSprite = true;                                
                            }

                            selectedImageTop = spritePlaceholderImage.Top;
                            selectedImageHeight = spritePlaceholderImage.Height;
                            selectedImageWidth = spritePlaceholderImage.Width;
                        }
                    }
                    
                    css.AppendLine($"#{image.Key} {{ height: {selectedImageHeight}px; width: {selectedImageWidth}px; background-image: url('{spriteSettings.SpriteUrl}'); background-position: 0px -{selectedImageTop}px; }}");

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
