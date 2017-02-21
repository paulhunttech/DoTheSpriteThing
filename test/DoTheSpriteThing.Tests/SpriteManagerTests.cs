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
        private readonly SpriteManager _spriteManager;
        private readonly Mock<IImageProcessor> _imageProcessorMock;
        private readonly Mock<ICssProcessor> _cssProcessorMock;
        private readonly Mock<IFile> _fileMock;
        private readonly Mock<IMagickImageHelper> _magickImageHelper;
        
        public SpriteManagerTests()
        {
            _imageProcessorMock = new Mock<IImageProcessor>();
            _cssProcessorMock = new Mock<ICssProcessor>();
            _fileMock = new Mock<IFile>();
            _magickImageHelper = new Mock<IMagickImageHelper>();
            _spriteManager = new SpriteManager(_imageProcessorMock.Object, _cssProcessorMock.Object, _fileMock.Object, _magickImageHelper.Object);
        }

        [Fact]
        public void CreateSprite_NullImagesParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(null, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal(exception.ParamName, "images");
        }

        [Fact]
        public void CreateSprite_DuplicateKeysInImagesParam_ShouldThrow()
        {
            // Arrange
            var images = new List<IImage>
            {
                new ByteArrayImage("a", null),
                new ByteArrayImage("a", null)
            };

            // Act + Assert
            var exception = Assert.Throws<ArgumentException>(() => _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal(exception.Message, "The list of keys must be unique.\r\nParameter name: images");
            Assert.Equal(exception.ParamName, "images");
        }

        [Fact]
        public void CreateSprite_DuplicateKeysInPlaceholderImagesParam_ShouldThrow()
        {
            // Arrange
            var placeholderImages = new List<IImage>
            {
                new ByteArrayImage("a", null),
                new ByteArrayImage("a", null)
            };

            // Act + Assert
            var exception = Assert.Throws<ArgumentException>(() => _spriteManager.CreateSprite(new List<IImage>(), placeholderImages, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.Equal(exception.Message, "The list of keys must be unique.\r\nParameter name: placeholderImages");
            Assert.Equal(exception.ParamName, "placeholderImages");
        }

        [Fact]
        public void CreateSprite_NullSpriteSettingsParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(new List<IImage>(), null, null));
            Assert.Equal(exception.ParamName, "spriteSettings");
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

            var images = new List<IImage>
            {
                new FileImage(imageFile1),
                new FileImage(imageFile2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, "/sprite.png", @"c:\sprite.css"));            

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), spriteFilename));
            Assert.Equal(spriteImages.Count, 2);  
            Assert.Equal(spriteImages[0].BackgroundColor, image1Colour);
            Assert.Equal(spriteImages[0].Width, image1Width);
            Assert.Equal(spriteImages[0].Height, image1Height);
            Assert.Equal(spriteImages[1].BackgroundColor, image2Colour);
            Assert.Equal(spriteImages[1].Width, image2Width);
            Assert.Equal(spriteImages[1].Height, image2Height);
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

            var images = new List<IImage>
            {
                new FileImage(imageFile1),
                new FileImage(imageFile2)
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(@"c:\sprite.png", spriteUrl, cssFilename));

            // Assert
            _cssProcessorMock.Verify(x => x.CreateCss($"#a-png {{ height: {image1Height}px; width: {image1Width}px; background-image: url('{spriteUrl}'); background-position: 0px -0px; }}\r\n#b-png {{ height: {image2Height}px; width: {image2Width}px; background-image: url('{spriteUrl}'); background-position: 0px -{image1Height}px; }}\r\n", cssFilename));            
        }
    }
}
