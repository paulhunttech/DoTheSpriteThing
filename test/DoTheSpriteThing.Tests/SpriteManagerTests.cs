using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;
using Moq;
using Thinktecture.IO;
using Xunit;

namespace DoTheSpriteThing.Tests
{
    public class SpriteManagerTests
    {
        private const string Image100X56Base64 = "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";
        private const int Image100X56Height = 56;
        private const int Image100X56Width = 100;
        private const int Image252X175Height = 175;
        private const int Image252X175Width = 252;
        private const string Image272X100Base64 = "iVBORw0KGgoAAAANSUhEUgAAAPwAAACvCAMAAADub0MMAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURQAAAKGhoaKioqOjo6SkpKWlpaampqenp6ioqKmpqaqqqqurqwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMqx+eoAAAEAdFJOU////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wBT9wclAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAGXRFWHRTb2Z0d2FyZQBwYWludC5uZXQgNC4wLjEzNANbegAABghJREFUeF7t3Ol62jgYQOHOtJ1u93+9DIG3CYt2ybGFfX6Fb5PO4yZBjumX05l/9se3N++zvJe7403elzvk9MUXe+SQ3yu/Dvmdcshfefutvwv+I7xL+fd3Nof8FZkdQPiQh8wOIHzIQ2YHED7kIbMDCB/ykNkBhA95yOwAwoc8ZHYA4UMeMkG+XSr+9Wp2LjJnSuTlr/wSnBkqBfKyH/yRmBciWfnvknfITQuNnLzUI7KzwiIjL/OM/KSQSMtLhFAxJxyS8v9KBFEzJRSS8uIRFM0Ig5S8cAxVM8IgIf9bOIq6CSGQkBeNo25CCPTIf1U4HwTi8tejTBKV82H/cXnBFCrnw/4PechAMIXK+bD/LvnfSqfD/rvkp72vYf/HP3vIQDCFyvmw/0MeMvgqmkDlfNh/XL7g0iucD/vvkVc3IQQS8sH7treomxACCfncpVc1IwxS8j/FI6hahOvTsV6M5zL9TEI+fenVLMHHPSSB0ZielE/Zq1iC2zsJy/xpzPC0fNz+h4IFuH+D8V10KGZn5GP2Cx5pHn/SLHF0NDonH7aXWwRLfCA+EpOz8qc/sh8seuPSGrfIDMTgvPzp9EP+yrJPZ1jkHrlxmFsif+bvu72ln0v5ZZ17hv+EMbdQ/rOwk0dkh2HstuRt5Bn5UZi6Kfn4n8QHf7uZuiX58Df8lbHf9oZuSd42wqgZg5kbkreLGKqGYOR25HP3DEe+tTJyO/I2EUfdCEzcjLw9pFA5AAO3Im8LadT2Y95G5AuehDhz+a9tRmDeRuTtIIfqbozbhrwN5FHfi2mbkLd+CTo6MWwL8vf3C9KMuXdo2BbkLV+Gnj7M2oC81UvR1YVR68tbvBx9PZi0unzqHBtmwOnWpNXlrV2Dzg4MWlve0nXobcecleWTn+WI0n1Ty5x15Z//IFKG9maMWVfewvXob8WUVeWt24IJjRiypnzZOTZM3+nWkDXlLduGGW2YsaK8VVsxpQkj1pO3aDvmtGDCavLZp/yydDywYsJq8tbswaQGDBgjXz9ARx9m1aN/hPzNryyRLMo7aX6bq3+A/N37c7EM9efYMK2nW+398g9nE9E0avsxrxbd3fJPF1E8hcoRmFiJ5m55vTdIxGk7x4Zp+7bX3Cuv9ZbcfkZ9w19pejhXb6d88CLKxVA1ClOr0NonH7mIsmHUjMPcGnT2yWt8QjpEwYe2Kmk43ersktf3TGI/KkZicgUae+QTP7VVPCM/FrPL0dcjry2IkkdkR2N6Mdo65HVFUHRPz42rFLWnW23t8ppiBPcjNx7zS9HVLJ+9iOpukVkCKxSiqVleTwKFH4gvgzXK0NMqryWJ0r/UPIBRT9UjG3oa5XWkeThtiy6FVYrQ0iZfeBFVXxFbDuuUoKNNXkMW5W+ILEjF6VZHk7z6PB/7GXuODVN+U0tDi7zyEt5P214vi7XyqG+Qr7qIerxaGqtlUd4gr7qQS8vIG1cpSj+QoLxeXnEpl9Otr5fnssM8qqvlqy/iucdXn8F1kzkUV8urreAz3SslKuWVbpeim1pqK+VVbhk7TaK0Tn6pmxFDsdcUKuvkFW4cm02gsEpe3dbJn24V1sgr2z72G0ddhXz/UzSfhh1HUVYhr2oKbDmGqnJ5RXOQOd2qKpZXMwt2HUFRqXzmf0jbHvYdRk2pvJJ5SN7UUlMor2Im7DyIkjL5z7oZMRR7D6GiSL710yArY/cBFBTJy89G/HSroEReej7s/xn5AvkpzrFhGDwhXSAvOyUUHpHNy0vOSeSRDdmsvNyssHhAMic/0Tk2DI975HLyUhND5A6pjLzMzPykcotUWl5ibrjcIpOUX/Ypmk+DzQ0SSXnx2Xk+3Uqk5IXn5+kDCeIJ+SnPsWEYvSMcl/+Mp2g+DU5/EY3LC74GD6db0ai82KvACoIx+fGfBlkZXlfEYvJCLwSxC0IReZFX4vZ0KxSWF3gtuL0hEpSf/hwbht0ZgaC81y8Hvcw/+1eH8CEPmR1A+JCHzA4gfMhDZgcQvpXfH4f8Xjnk98rvPcufvrzoAbaE05f333q743SWf727lWWcTqf/AWh9bguWJclIAAAAAElFTkSuQmCC";
        private readonly Mock<ICssProcessor> _cssProcessorMock;
        private readonly Mock<IFile> _fileMock;
        private readonly Mock<IImageProcessor> _imageProcessorMock;
        private readonly Mock<IMagickImageHelper> _magickImageHelper;

        private readonly SpriteManager _spriteManager;

        public SpriteManagerTests()
        {
            _imageProcessorMock = new Mock<IImageProcessor>();
            _cssProcessorMock = new Mock<ICssProcessor>();
            _fileMock = new Mock<IFile>();
            _magickImageHelper = new Mock<IMagickImageHelper>();
            _spriteManager = new SpriteManager(_imageProcessorMock.Object, _cssProcessorMock.Object, _fileMock.Object, _magickImageHelper.Object);
        }

        [Fact]
        public void CreateSprite_DuplicateKeysInImagesParam_ShouldThrow()
        {
            // Arrange
            var images = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", null),
                new ByteArraySpriteImage("a", null)
            };

            // Act + Assert
            var exception = Assert.Throws<ArgumentException>(() => _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal("The list of keys must be unique.\r\nParameter name: images", exception.Message);
            Assert.Equal("images", exception.ParamName);
        }

        [Fact]
        public void CreateSprite_DuplicateKeysInPlaceholderImagesParam_ShouldThrow()
        {
            // Arrange
            var placeholderImages = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", null),
                new ByteArraySpriteImage("a", null)
            };

            // Act + Assert
            var exception = Assert.Throws<ArgumentException>(() => _spriteManager.CreateSprite(new List<ISpriteImage>(), placeholderImages, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal("The list of keys must be unique.\r\nParameter name: placeholderImages", exception.Message);
            Assert.Equal("placeholderImages", exception.ParamName);
        }

        [Fact]
        public void CreateSprite_ImageFileDoesNotExist_ShouldUseDefaultPlaceholder()
        {
            // Arrange
            var imageFile1 = new FileInfo(@"C:\a.png");

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(false);

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1)
            };

            byte[] placeholderImageByteArray = Convert.FromBase64String(Image272X100Base64);

            _magickImageHelper.Setup(x => x.Create(It.IsAny<byte[]>())).Returns(new MagickImage(placeholderImageByteArray));

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>()));
            Assert.Equal(1, spriteImages.Count);
            Assert.Equal(Image252X175Width, spriteImages[0].Width);
            Assert.Equal(Image252X175Height, spriteImages[0].Height);
        }

        [Fact]
        public void CreateSprite_ImageFileDoesNotExist_ShouldUseSelectedPlaceholder()
        {
            // Arrange
            var imageFile1 = new FileInfo(@"C:\a.png");

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(false);

            const string placeholderImageKey1 = "1";
            const string placeholderImageKey2 = "2";

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1, placeholderImageKey1)
            };

            byte[] placeholderImageByteArray1 = Convert.FromBase64String(Image272X100Base64);
            byte[] placeholderImageByteArray2 = Convert.FromBase64String(Image100X56Base64);

            _magickImageHelper.Setup(x => x.Create(It.IsAny<byte[]>())).Returns(new MagickImage(placeholderImageByteArray2));

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            var placeholderImages = new List<ISpriteImage>
            {
                new ByteArraySpriteImage(placeholderImageKey1, placeholderImageByteArray1),
                new ByteArraySpriteImage(placeholderImageKey2, placeholderImageByteArray2)
            };

            // Act
            _spriteManager.CreateSprite(images, placeholderImages, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>()));
            Assert.Equal(1, spriteImages.Count);
            Assert.Equal(Image100X56Width, spriteImages[0].Width);
            Assert.Equal(Image100X56Height, spriteImages[0].Height);
        }

        [Fact]
        public void CreateSprite_NullImagesParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(null, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal("images", exception.ParamName);
        }

        [Fact]
        public void CreateSprite_NullSpriteSettingsParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(new List<ISpriteImage>(), null, null));
            Assert.Equal("spriteSettings", exception.ParamName);
        }

        [Fact]
        public void CreateSprite_ValidByteArrayListInImagesParam_ShouldGenerateCss()
        {
            // Arrange            
            const string cssFilename = @"c:\sprite.css";
            const string spriteUrl = "/sprite.png";

            byte[] imageByteArray1 = Convert.FromBase64String(Image272X100Base64);
            byte[] imageByteArray2 = Convert.FromBase64String(Image100X56Base64);

            var magickImage1 = new MagickImage(imageByteArray1);
            var magickImage2 = new MagickImage(imageByteArray2);
            _magickImageHelper.Setup(x => x.Create(imageByteArray1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageByteArray2)).Returns(magickImage2);

            var images = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", imageByteArray1),
                new ByteArraySpriteImage("b", imageByteArray2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", spriteUrl, cssFilename));

            // Assert
            _cssProcessorMock.Verify(x => x.CreateCss($"#a {{ height: 175px; width: 252px; background-image: url('{spriteUrl}'); background-position: 0px -0px; }}\r\n#b {{ height: 56px; width: 100px; background-image: url('{spriteUrl}'); background-position: 0px -175px; }}\r\n", cssFilename));
        }

        [Fact]
        public void CreateSprite_ValidByteArrayListInImagesParam_ShouldGenerateSprite()
        {
            // Arrange
            const string spriteFilename = @"C:\sprite.png";

            byte[] imageByteArray1 = Convert.FromBase64String(Image272X100Base64);
            byte[] imageByteArray2 = Convert.FromBase64String(Image100X56Base64);

            var magickImage1 = new MagickImage(imageByteArray1);
            var magickImage2 = new MagickImage(imageByteArray2);
            _magickImageHelper.Setup(x => x.Create(imageByteArray1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageByteArray2)).Returns(magickImage2);

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            var images = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", imageByteArray1),
                new ByteArraySpriteImage("b", imageByteArray2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), spriteFilename));
            Assert.Equal(2, spriteImages.Count);
            Assert.Equal(Image252X175Width, spriteImages[0].Width);
            Assert.Equal(Image252X175Height, spriteImages[0].Height);
            Assert.Equal(Image100X56Width, spriteImages[1].Width);
            Assert.Equal(Image100X56Height, spriteImages[1].Height);
        }

        [Fact]
        public void CreateSprite_ValidFileListInImagesParam_ShouldGenerateCss()
        {
            // Arrange
            const string cssFilename = @"c:\sprite.css";
            const string spriteUrl = "/sprite.png";
            const int image1Height = 1;
            const int image1Width = 2;
            const int image2Height = 3;
            const int image2Width = 4;
            MagickColor image1Colour = MagickColor.FromRgb(1, 1, 1);
            MagickColor image2Colour = MagickColor.FromRgb(2, 2, 2);

            var imageFile1 = new FileInfo(@"C:\a.png");
            var imageFile2 = new FileInfo(@"C:\b.png");

            var magickImage1 = new MagickImage(image1Colour, image1Width, image1Height);
            var magickImage2 = new MagickImage(image2Colour, image2Width, image2Height);
            _magickImageHelper.Setup(x => x.Create(imageFile1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageFile2)).Returns(magickImage2);

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(true);
            _fileMock.Setup(x => x.Exists(imageFile2.FullName)).Returns(true);

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1),
                new FileSpriteImage(imageFile2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", spriteUrl, cssFilename));

            // Assert
            _cssProcessorMock.Verify(x => x.CreateCss($"#a-png {{ height: {image1Height}px; width: {image1Width}px; background-image: url('{spriteUrl}'); background-position: 0px -0px; }}\r\n#b-png {{ height: {image2Height}px; width: {image2Width}px; background-image: url('{spriteUrl}'); background-position: 0px -{image1Height}px; }}\r\n", cssFilename));
        }

        [Fact]
        public void CreateSprite_ValidFileListInImagesParam_ShouldGenerateSprite()
        {
            // Arrange
            const string spriteFilename = @"C:\sprite.png";
            const int image1Height = 1;
            const int image1Width = 2;
            const int image2Height = 3;
            const int image2Width = 4;
            MagickColor image1Colour = MagickColor.FromRgb(5, 6, 7);
            MagickColor image2Colour = MagickColor.FromRgb(8, 9, 10);

            var imageFile1 = new FileInfo(@"C:\a.png");
            var imageFile2 = new FileInfo(@"C:\b.png");

            var magickImage1 = new MagickImage(image1Colour, image1Width, image1Height);
            var magickImage2 = new MagickImage(image2Colour, image2Width, image2Height);
            _magickImageHelper.Setup(x => x.Create(imageFile1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageFile2)).Returns(magickImage2);

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(true);
            _fileMock.Setup(x => x.Exists(imageFile2.FullName)).Returns(true);

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1),
                new FileSpriteImage(imageFile2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), spriteFilename));
            Assert.Equal(2, spriteImages.Count);
            Assert.Equal(image1Colour, spriteImages[0].BackgroundColor);
            Assert.Equal(image1Width, spriteImages[0].Width);
            Assert.Equal(image1Height, spriteImages[0].Height);
            Assert.Equal(image2Colour, spriteImages[1].BackgroundColor);
            Assert.Equal(image2Width, spriteImages[1].Width);
            Assert.Equal(image2Height, spriteImages[1].Height);
        }

        [Fact]
        public void CreateSprite_ValidMixedFileAndByteArrayListInImagesParam_ShouldGenerateCss()
        {
            // Arrange
            const string cssFilename = @"c:\sprite.css";
            const string spriteUrl = "/sprite.png";
            const int image1Height = 1;
            const int image1Width = 2;
            MagickColor image1Colour = MagickColor.FromRgb(1, 1, 1);

            var imageFile1 = new FileInfo(@"C:\a.png");
            byte[] imageByteArray2 = Convert.FromBase64String(Image100X56Base64);

            var magickImage1 = new MagickImage(image1Colour, image1Width, image1Height);
            var magickImage2 = new MagickImage(imageByteArray2);
            _magickImageHelper.Setup(x => x.Create(imageFile1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageByteArray2)).Returns(magickImage2);

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(true);

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1),
                new ByteArraySpriteImage("b", imageByteArray2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", spriteUrl, cssFilename));

            // Assert
            _cssProcessorMock.Verify(x => x.CreateCss($"#a-png {{ height: {image1Height}px; width: {image1Width}px; background-image: url('{spriteUrl}'); background-position: 0px -0px; }}\r\n#b {{ height: 56px; width: 100px; background-image: url('{spriteUrl}'); background-position: 0px -{image1Height}px; }}\r\n", cssFilename));
        }

        [Fact]
        public void CreateSprite_ValidMixedFileAndByteArrayListInImagesParam_ShouldGenerateSprite()
        {
            // Arrange
            const string spriteFilename = @"C:\sprite.png";
            const int image1Height = 1;
            const int image1Width = 2;
            MagickColor image1Colour = MagickColor.FromRgb(5, 6, 7);

            var imageFile1 = new FileInfo(@"C:\a.png");
            byte[] imageByteArray2 = Convert.FromBase64String(Image272X100Base64);

            var magickImage1 = new MagickImage(image1Colour, image1Width, image1Height);
            var magickImage2 = new MagickImage(imageByteArray2);
            _magickImageHelper.Setup(x => x.Create(imageFile1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageByteArray2)).Returns(magickImage2);

            _fileMock.Setup(x => x.Exists(imageFile1.FullName)).Returns(true);

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            var images = new List<ISpriteImage>
            {
                new FileSpriteImage(imageFile1),
                new ByteArraySpriteImage("b", imageByteArray2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), spriteFilename));
            Assert.Equal(2, spriteImages.Count);
            Assert.Equal(image1Colour, spriteImages[0].BackgroundColor);
            Assert.Equal(image1Width, spriteImages[0].Width);
            Assert.Equal(image1Height, spriteImages[0].Height);
            Assert.Equal(Image252X175Width, spriteImages[1].Width);
            Assert.Equal(Image252X175Height, spriteImages[1].Height);
        }
    }
}