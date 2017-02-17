using System;
using System.Collections.Generic;
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
        
        public SpriteManagerTests()
        {
            _imageProcessorMock = new Mock<IImageProcessor>();
            _cssProcessorMock = new Mock<ICssProcessor>();
            _fileMock = new Mock<IFile>();
            _spriteManager = new SpriteManager(_imageProcessorMock.Object, _cssProcessorMock.Object, _fileMock.Object);
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
    }
}
