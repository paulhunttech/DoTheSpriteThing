using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageMagick;
using Thinktecture.IO;
using Thinktecture.IO.Adapters;

namespace DoTheSpriteThing
{
    public class SpriteManager
    {
        private const string DefaultPlaceholderImageKey = "placeholder";
        private static readonly List<IImage> DefaultPlaceholderImages;
        private readonly IImageProcessor _imageProcessor;
        private readonly ICssProcessor _cssProcessor;
        private readonly IFile _file;
        private readonly IMagickImageHelper _magickImageHelper;

        static SpriteManager()
        {
            const string defaultPlaceholderImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAPwAAACvCAMAAADub0MMAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURQAAAKGhoaKioqOjo6SkpKWlpaampqenp6ioqKmpqaqqqqurqwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMqx+eoAAAEAdFJOU////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wBT9wclAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAGXRFWHRTb2Z0d2FyZQBwYWludC5uZXQgNC4wLjEzNANbegAABghJREFUeF7t3Ol62jgYQOHOtJ1u93+9DIG3CYt2ybGFfX6Fb5PO4yZBjumX05l/9se3N++zvJe7403elzvk9MUXe+SQ3yu/Dvmdcshfefutvwv+I7xL+fd3Nof8FZkdQPiQh8wOIHzIQ2YHED7kIbMDCB/ykNkBhA95yOwAwoc8ZHYA4UMeMkG+XSr+9Wp2LjJnSuTlr/wSnBkqBfKyH/yRmBciWfnvknfITQuNnLzUI7KzwiIjL/OM/KSQSMtLhFAxJxyS8v9KBFEzJRSS8uIRFM0Ig5S8cAxVM8IgIf9bOIq6CSGQkBeNo25CCPTIf1U4HwTi8tejTBKV82H/cXnBFCrnw/4PechAMIXK+bD/LvnfSqfD/rvkp72vYf/HP3vIQDCFyvmw/0MeMvgqmkDlfNh/XL7g0iucD/vvkVc3IQQS8sH7treomxACCfncpVc1IwxS8j/FI6hahOvTsV6M5zL9TEI+fenVLMHHPSSB0ZielE/Zq1iC2zsJy/xpzPC0fNz+h4IFuH+D8V10KGZn5GP2Cx5pHn/SLHF0NDonH7aXWwRLfCA+EpOz8qc/sh8seuPSGrfIDMTgvPzp9EP+yrJPZ1jkHrlxmFsif+bvu72ln0v5ZZ17hv+EMbdQ/rOwk0dkh2HstuRt5Bn5UZi6Kfn4n8QHf7uZuiX58Df8lbHf9oZuSd42wqgZg5kbkreLGKqGYOR25HP3DEe+tTJyO/I2EUfdCEzcjLw9pFA5AAO3Im8LadT2Y95G5AuehDhz+a9tRmDeRuTtIIfqbozbhrwN5FHfi2mbkLd+CTo6MWwL8vf3C9KMuXdo2BbkLV+Gnj7M2oC81UvR1YVR68tbvBx9PZi0unzqHBtmwOnWpNXlrV2Dzg4MWlve0nXobcecleWTn+WI0n1Ty5x15Z//IFKG9maMWVfewvXob8WUVeWt24IJjRiypnzZOTZM3+nWkDXlLduGGW2YsaK8VVsxpQkj1pO3aDvmtGDCavLZp/yydDywYsJq8tbswaQGDBgjXz9ARx9m1aN/hPzNryyRLMo7aX6bq3+A/N37c7EM9efYMK2nW+398g9nE9E0avsxrxbd3fJPF1E8hcoRmFiJ5m55vTdIxGk7x4Zp+7bX3Cuv9ZbcfkZ9w19pejhXb6d88CLKxVA1ClOr0NonH7mIsmHUjMPcGnT2yWt8QjpEwYe2Kmk43ersktf3TGI/KkZicgUae+QTP7VVPCM/FrPL0dcjry2IkkdkR2N6Mdo65HVFUHRPz42rFLWnW23t8ppiBPcjNx7zS9HVLJ+9iOpukVkCKxSiqVleTwKFH4gvgzXK0NMqryWJ0r/UPIBRT9UjG3oa5XWkeThtiy6FVYrQ0iZfeBFVXxFbDuuUoKNNXkMW5W+ILEjF6VZHk7z6PB/7GXuODVN+U0tDi7zyEt5P214vi7XyqG+Qr7qIerxaGqtlUd4gr7qQS8vIG1cpSj+QoLxeXnEpl9Otr5fnssM8qqvlqy/iucdXn8F1kzkUV8urreAz3SslKuWVbpeim1pqK+VVbhk7TaK0Tn6pmxFDsdcUKuvkFW4cm02gsEpe3dbJn24V1sgr2z72G0ddhXz/UzSfhh1HUVYhr2oKbDmGqnJ5RXOQOd2qKpZXMwt2HUFRqXzmf0jbHvYdRk2pvJJ5SN7UUlMor2Im7DyIkjL5z7oZMRR7D6GiSL710yArY/cBFBTJy89G/HSroEReej7s/xn5AvkpzrFhGDwhXSAvOyUUHpHNy0vOSeSRDdmsvNyssHhAMic/0Tk2DI975HLyUhND5A6pjLzMzPykcotUWl5ibrjcIpOUX/Ypmk+DzQ0SSXnx2Xk+3Uqk5IXn5+kDCeIJ+SnPsWEYvSMcl/+Mp2g+DU5/EY3LC74GD6db0ai82KvACoIx+fGfBlkZXlfEYvJCLwSxC0IReZFX4vZ0KxSWF3gtuL0hEpSf/hwbht0ZgaC81y8Hvcw/+1eH8CEPmR1A+JCHzA4gfMhDZgcQvpXfH4f8Xjnk98rvPcufvrzoAbaE05f333q743SWf727lWWcTqf/AWh9bguWJclIAAAAAElFTkSuQmCC";
            byte[] defaultPlaceholderImageBytes = Convert.FromBase64String(defaultPlaceholderImageBase64);

            DefaultPlaceholderImages = new List<IImage>
            {
                new ByteArrayImage(DefaultPlaceholderImageKey, defaultPlaceholderImageBytes)
            };
        }

        public SpriteManager()
        {
            _imageProcessor = new ImageProcessor(); 
            _cssProcessor = new CssProcessor();
            _file = new FileAdapter();
            _magickImageHelper = new MagickImageHelper();
        }

        internal SpriteManager(IImageProcessor imageProcessor, ICssProcessor cssProcessor, IFile file, IMagickImageHelper magickImageHelper)
        {
            _imageProcessor = imageProcessor;
            _cssProcessor = cssProcessor;
            _file = file;
            _magickImageHelper = magickImageHelper;
        }

        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite.</param>        
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>
        public void CreateSprite(IReadOnlyCollection<IImage> images, SpriteSettings spriteSettings)
        {
            CreateSprite(images, DefaultPlaceholderImages, spriteSettings);
        }

        private MagickImage ImageToMagickImage(IHoverImage image, MagickImage parentImage)
        {
            MagickImage imageForSprite = null;

            if (image is HoverFileImage)
            {
                var fileImage = (HoverFileImage)image;

                if (_file.Exists(fileImage.FilePath.FullName))
                {
                    imageForSprite = _magickImageHelper.Create(fileImage.FilePath);

                    if (imageForSprite.Height != parentImage.Height || imageForSprite.Width != parentImage.Width)
                    {
                        imageForSprite.Resize(new MagickGeometry($"{parentImage.Height}x{parentImage.Width}!"));
                    }
                }
            }
            else if (image is HoverByteArrayImage)
            {
                var byteArrayImage = (HoverByteArrayImage)image;

                if (byteArrayImage.ImageData != null)
                {
                    imageForSprite = _magickImageHelper.Create(byteArrayImage.ImageData);

                    if (imageForSprite.Height != parentImage.Height || imageForSprite.Width != parentImage.Width)
                    {
                        imageForSprite.Resize(new MagickGeometry($"{parentImage.Height}x{parentImage.Width}!"));
                    }
                }
            }

            return imageForSprite;
        }

        private MagickImage ImageToMagickImage(IImage image)
        {
            MagickImage imageForSprite = null;

            if (image is FileImage)
            {
                var fileImage = (FileImage)image;

                if (_file.Exists(fileImage.FilePath.FullName))
                {                    
                    imageForSprite = _magickImageHelper.Create(fileImage.FilePath);

                    if (fileImage.Resize)
                    {
                        imageForSprite.Resize(new MagickGeometry($"{fileImage.ResizeToHeight}x{fileImage.ResizeToWidth}!"));
                    }                    
                }                
            }
            else if (image is ByteArrayImage)
            {
                var byteArrayImage = (ByteArrayImage)image;

                if (byteArrayImage.ImageData != null)
                {
                    imageForSprite = _magickImageHelper.Create(byteArrayImage.ImageData);
                    
                    if (byteArrayImage.Resize)
                    {
                        imageForSprite.Resize(new MagickGeometry($"{byteArrayImage.ResizeToWidth}x{byteArrayImage.ResizeToHeight}!"));
                    }                    
                }                
            }

            return imageForSprite;
        }

        /// <summary>
        /// Create a sprite image from a list of images and the CSS to render each image.
        /// </summary>
        /// <param name="images">The list of images to include in the sprite.</param>
        /// <param name="placeholderImages">The list of custom placeholder images.</param>
        /// <param name="spriteSettings">The settings to use when creating the sprite.</param>
        public void CreateSprite(IReadOnlyCollection<IImage> images, IReadOnlyCollection<IImage> placeholderImages, SpriteSettings spriteSettings)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            if (images.GroupBy(x => x.Key).Any(x => x.Skip(1).Any()))
            {
                throw new ArgumentException("The list of keys must be unique.", nameof(images));
            }

            if (placeholderImages == null || !placeholderImages.Any())
            {
                placeholderImages = DefaultPlaceholderImages;
            }

            if (placeholderImages.GroupBy(x => x.Key).Any(x => x.Skip(1).Any()))
            {
                throw new ArgumentException("The list of keys must be unique.", nameof(placeholderImages));
            }

            if (spriteSettings == null)
            {
                throw new ArgumentNullException(nameof(spriteSettings));
            }

            var spritePlaceholderImages = new List<SpritePlaceholderImage>();

            using (var spriteImages = new MagickImageCollection())
            {
                var css = new StringBuilder();
                var nextSpriteImageTop = 0;

                foreach (IImage image in images)
                {
                    var hasImageBeenAddedToSprite = false;
                    int selectedImageHeight;
                    int selectedImageTop;
                    int selectedImageWidth;
                    var selectedHoverImageHeight = 0;
                    var selectedHoverImageTop = 0;
                    var selectedHoverImageWidth = 0;

                    MagickImage imageForSprite = ImageToMagickImage(image);

                    if (imageForSprite != null)
                    {
                        spriteImages.Add(imageForSprite);
                        hasImageBeenAddedToSprite = true;
                        selectedImageTop = nextSpriteImageTop;
                        selectedImageHeight = imageForSprite.Height;
                        selectedImageWidth = imageForSprite.Width;
                    }
                    else
                    {
                        string selectedPlaceholderImageKey = placeholderImages.Any(x => x.Key == image.PlaceholderImageKey) ? image.PlaceholderImageKey : placeholderImages.First().Key;
                        IImage selectedPlaceholderImage = placeholderImages.FirstOrDefault(x => x.Key == selectedPlaceholderImageKey);                                                
                        imageForSprite = ImageToMagickImage(selectedPlaceholderImage);                        

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

                    css.AppendLine($"#{image.Key} {{ height: {selectedImageHeight}px; width: {selectedImageWidth}px; background-image: url('{spriteSettings.SpriteUrl}'); background-position: 0px -{selectedImageTop}px; }}");                    

                    if (hasImageBeenAddedToSprite)
                    {
                        nextSpriteImageTop += selectedImageHeight;
                    }

                    if (image.HoverImage != null)
                    {
                        var hoverImageForSprite = ImageToMagickImage(image.HoverImage, imageForSprite);

                        if (hoverImageForSprite != null)
                        {
                            spriteImages.Add(hoverImageForSprite);
                            hasImageBeenAddedToSprite = true;
                            selectedHoverImageTop = nextSpriteImageTop;
                            selectedHoverImageHeight = hoverImageForSprite.Height;
                            selectedHoverImageWidth = hoverImageForSprite.Width;
                        }                        
                        
                        css.AppendLine($"#{image.Key}:hover {{ height: {selectedHoverImageHeight}px; width: {selectedHoverImageWidth}px; background-image: url('{spriteSettings.SpriteUrl}'); background-position: 0px -{selectedHoverImageTop}px; }}");

                        if (hasImageBeenAddedToSprite)
                        {
                            nextSpriteImageTop += selectedHoverImageHeight;
                        }
                    }                    
                }

                _imageProcessor.CreateSprite(spriteImages, spriteSettings.SpriteFilename);                                
                _cssProcessor.CreateCss(css.ToString(), spriteSettings.CssFilename);                
            }
        }        
    }
}