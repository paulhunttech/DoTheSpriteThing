using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Thinktecture.IO;

namespace DoTheSpriteThing.Tests
{
    [TestFixture]
    public class SpriteManagerTests
    {
        private SpriteManager _spriteManager;
        private Mock<IImageProcessor> _imageProcessorMock;
        private Mock<ICssProcessor> _cssProcessorMock;
        private Mock<IFile> _fileMock;

        [SetUp]
        public void TestSetup()
        {
            _imageProcessorMock = new Mock<IImageProcessor>();
            _cssProcessorMock = new Mock<ICssProcessor>();
            _fileMock = new Mock<IFile>();
            _spriteManager = new SpriteManager(_imageProcessorMock.Object, _cssProcessorMock.Object, _fileMock.Object);
        }

        [Test]
        public void CreateSprite_NullImagesParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(null, null, new SpriteSettings(@"c:\sprite.png", "/sprite.png", @"c:\sprite.css")));
            Assert.That(exception.ParamName, Is.EqualTo("images"));
        }

        [Test]
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
            Assert.That(exception.Message, Is.EqualTo("The list of keys must be unique.\r\nParameter name: images"));
            Assert.That(exception.ParamName, Is.EqualTo("images"));
        }

        [Test]
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
            Assert.That(exception.Message, Is.EqualTo("The list of keys must be unique.\r\nParameter name: placeholderImages"));
            Assert.That(exception.ParamName, Is.EqualTo("placeholderImages"));
        }

        [Test]
        public void CreateSprite_NullSpriteSettingsParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(new List<IImage>(), null, null));
            Assert.That(exception.ParamName, Is.EqualTo("spriteSettings"));
        }
    }
}
