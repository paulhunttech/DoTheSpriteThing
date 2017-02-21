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
            Assert.Equal("images", exception.ParamName);
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
            Assert.Equal("The list of keys must be unique.\r\nParameter name: images", exception.Message);
            Assert.Equal("images", exception.ParamName);
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
            Assert.Equal("The list of keys must be unique.\r\nParameter name: placeholderImages", exception.Message);
            Assert.Equal("placeholderImages", exception.ParamName);
        }

        [Fact]
        public void CreateSprite_NullSpriteSettingsParam_ShouldThrow()
        {
            // Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _spriteManager.CreateSprite(new List<IImage>(), null, null));
            Assert.Equal("spriteSettings", exception.ParamName);
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
            Assert.Equal(2, spriteImages.Count);  
            Assert.Equal(image1Colour, spriteImages[0].BackgroundColor);
            Assert.Equal(image1Width, spriteImages[0].Width);
            Assert.Equal(image1Height, spriteImages[0].Height);
            Assert.Equal(image2Colour, spriteImages[1].BackgroundColor);
            Assert.Equal(image2Width, spriteImages[1].Width);
            Assert.Equal(image2Height, spriteImages[1].Height);
        }

        [Fact]
        public void CreateSprite_ValidByteArrayListInImagesParam_ShouldGenerateSprite()
        {
            // Arrange
            const string spriteFilename = @"C:\sprite.png";
            const string image1Base64 = "iVBORw0KGgoAAAANSUhEUgAAAPwAAACvCAMAAADub0MMAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURQAAAKGhoaKioqOjo6SkpKWlpaampqenp6ioqKmpqaqqqqurqwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMqx+eoAAAEAdFJOU////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wBT9wclAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAGXRFWHRTb2Z0d2FyZQBwYWludC5uZXQgNC4wLjEzNANbegAABghJREFUeF7t3Ol62jgYQOHOtJ1u93+9DIG3CYt2ybGFfX6Fb5PO4yZBjumX05l/9se3N++zvJe7403elzvk9MUXe+SQ3yu/Dvmdcshfefutvwv+I7xL+fd3Nof8FZkdQPiQh8wOIHzIQ2YHED7kIbMDCB/ykNkBhA95yOwAwoc8ZHYA4UMeMkG+XSr+9Wp2LjJnSuTlr/wSnBkqBfKyH/yRmBciWfnvknfITQuNnLzUI7KzwiIjL/OM/KSQSMtLhFAxJxyS8v9KBFEzJRSS8uIRFM0Ig5S8cAxVM8IgIf9bOIq6CSGQkBeNo25CCPTIf1U4HwTi8tejTBKV82H/cXnBFCrnw/4PechAMIXK+bD/LvnfSqfD/rvkp72vYf/HP3vIQDCFyvmw/0MeMvgqmkDlfNh/XL7g0iucD/vvkVc3IQQS8sH7treomxACCfncpVc1IwxS8j/FI6hahOvTsV6M5zL9TEI+fenVLMHHPSSB0ZielE/Zq1iC2zsJy/xpzPC0fNz+h4IFuH+D8V10KGZn5GP2Cx5pHn/SLHF0NDonH7aXWwRLfCA+EpOz8qc/sh8seuPSGrfIDMTgvPzp9EP+yrJPZ1jkHrlxmFsif+bvu72ln0v5ZZ17hv+EMbdQ/rOwk0dkh2HstuRt5Bn5UZi6Kfn4n8QHf7uZuiX58Df8lbHf9oZuSd42wqgZg5kbkreLGKqGYOR25HP3DEe+tTJyO/I2EUfdCEzcjLw9pFA5AAO3Im8LadT2Y95G5AuehDhz+a9tRmDeRuTtIIfqbozbhrwN5FHfi2mbkLd+CTo6MWwL8vf3C9KMuXdo2BbkLV+Gnj7M2oC81UvR1YVR68tbvBx9PZi0unzqHBtmwOnWpNXlrV2Dzg4MWlve0nXobcecleWTn+WI0n1Ty5x15Z//IFKG9maMWVfewvXob8WUVeWt24IJjRiypnzZOTZM3+nWkDXlLduGGW2YsaK8VVsxpQkj1pO3aDvmtGDCavLZp/yydDywYsJq8tbswaQGDBgjXz9ARx9m1aN/hPzNryyRLMo7aX6bq3+A/N37c7EM9efYMK2nW+398g9nE9E0avsxrxbd3fJPF1E8hcoRmFiJ5m55vTdIxGk7x4Zp+7bX3Cuv9ZbcfkZ9w19pejhXb6d88CLKxVA1ClOr0NonH7mIsmHUjMPcGnT2yWt8QjpEwYe2Kmk43ersktf3TGI/KkZicgUae+QTP7VVPCM/FrPL0dcjry2IkkdkR2N6Mdo65HVFUHRPz42rFLWnW23t8ppiBPcjNx7zS9HVLJ+9iOpukVkCKxSiqVleTwKFH4gvgzXK0NMqryWJ0r/UPIBRT9UjG3oa5XWkeThtiy6FVYrQ0iZfeBFVXxFbDuuUoKNNXkMW5W+ILEjF6VZHk7z6PB/7GXuODVN+U0tDi7zyEt5P214vi7XyqG+Qr7qIerxaGqtlUd4gr7qQS8vIG1cpSj+QoLxeXnEpl9Otr5fnssM8qqvlqy/iucdXn8F1kzkUV8urreAz3SslKuWVbpeim1pqK+VVbhk7TaK0Tn6pmxFDsdcUKuvkFW4cm02gsEpe3dbJn24V1sgr2z72G0ddhXz/UzSfhh1HUVYhr2oKbDmGqnJ5RXOQOd2qKpZXMwt2HUFRqXzmf0jbHvYdRk2pvJJ5SN7UUlMor2Im7DyIkjL5z7oZMRR7D6GiSL710yArY/cBFBTJy89G/HSroEReej7s/xn5AvkpzrFhGDwhXSAvOyUUHpHNy0vOSeSRDdmsvNyssHhAMic/0Tk2DI975HLyUhND5A6pjLzMzPykcotUWl5ibrjcIpOUX/Ypmk+DzQ0SSXnx2Xk+3Uqk5IXn5+kDCeIJ+SnPsWEYvSMcl/+Mp2g+DU5/EY3LC74GD6db0ai82KvACoIx+fGfBlkZXlfEYvJCLwSxC0IReZFX4vZ0KxSWF3gtuL0hEpSf/hwbht0ZgaC81y8Hvcw/+1eH8CEPmR1A+JCHzA4gfMhDZgcQvpXfH4f8Xjnk98rvPcufvrzoAbaE05f333q743SWf727lWWcTqf/AWh9bguWJclIAAAAAElFTkSuQmCC";
            const string image2Base64 = "iVBORw0KGgoAAAANSUhEUgAAAPwAAACvCAMAAADub0MMAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURQAAAKGhoaKioqOjo6SkpKWlpaampqenp6ioqKmpqaqqqqurqwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMqx+eoAAAEAdFJOU////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wBT9wclAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAGXRFWHRTb2Z0d2FyZQBwYWludC5uZXQgNC4wLjEzNANbegAABghJREFUeF7t3Ol62jgYQOHOtJ1u93+9DIG3CYt2ybGFfX6Fb5PO4yZBjumX05l/9se3N++zvJe7403elzvk9MUXe+SQ3yu/Dvmdcshfefutvwv+I7xL+fd3Nof8FZkdQPiQh8wOIHzIQ2YHED7kIbMDCB/ykNkBhA95yOwAwoc8ZHYA4UMeMkG+XSr+9Wp2LjJnSuTlr/wSnBkqBfKyH/yRmBciWfnvknfITQuNnLzUI7KzwiIjL/OM/KSQSMtLhFAxJxyS8v9KBFEzJRSS8uIRFM0Ig5S8cAxVM8IgIf9bOIq6CSGQkBeNo25CCPTIf1U4HwTi8tejTBKV82H/cXnBFCrnw/4PechAMIXK+bD/LvnfSqfD/rvkp72vYf/HP3vIQDCFyvmw/0MeMvgqmkDlfNh/XL7g0iucD/vvkVc3IQQS8sH7treomxACCfncpVc1IwxS8j/FI6hahOvTsV6M5zL9TEI+fenVLMHHPSSB0ZielE/Zq1iC2zsJy/xpzPC0fNz+h4IFuH+D8V10KGZn5GP2Cx5pHn/SLHF0NDonH7aXWwRLfCA+EpOz8qc/sh8seuPSGrfIDMTgvPzp9EP+yrJPZ1jkHrlxmFsif+bvu72ln0v5ZZ17hv+EMbdQ/rOwk0dkh2HstuRt5Bn5UZi6Kfn4n8QHf7uZuiX58Df8lbHf9oZuSd42wqgZg5kbkreLGKqGYOR25HP3DEe+tTJyO/I2EUfdCEzcjLw9pFA5AAO3Im8LadT2Y95G5AuehDhz+a9tRmDeRuTtIIfqbozbhrwN5FHfi2mbkLd+CTo6MWwL8vf3C9KMuXdo2BbkLV+Gnj7M2oC81UvR1YVR68tbvBx9PZi0unzqHBtmwOnWpNXlrV2Dzg4MWlve0nXobcecleWTn+WI0n1Ty5x15Z//IFKG9maMWVfewvXob8WUVeWt24IJjRiypnzZOTZM3+nWkDXlLduGGW2YsaK8VVsxpQkj1pO3aDvmtGDCavLZp/yydDywYsJq8tbswaQGDBgjXz9ARx9m1aN/hPzNryyRLMo7aX6bq3+A/N37c7EM9efYMK2nW+398g9nE9E0avsxrxbd3fJPF1E8hcoRmFiJ5m55vTdIxGk7x4Zp+7bX3Cuv9ZbcfkZ9w19pejhXb6d88CLKxVA1ClOr0NonH7mIsmHUjMPcGnT2yWt8QjpEwYe2Kmk43ersktf3TGI/KkZicgUae+QTP7VVPCM/FrPL0dcjry2IkkdkR2N6Mdo65HVFUHRPz42rFLWnW23t8ppiBPcjNx7zS9HVLJ+9iOpukVkCKxSiqVleTwKFH4gvgzXK0NMqryWJ0r/UPIBRT9UjG3oa5XWkeThtiy6FVYrQ0iZfeBFVXxFbDuuUoKNNXkMW5W+ILEjF6VZHk7z6PB/7GXuODVN+U0tDi7zyEt5P214vi7XyqG+Qr7qIerxaGqtlUd4gr7qQS8vIG1cpSj+QoLxeXnEpl9Otr5fnssM8qqvlqy/iucdXn8F1kzkUV8urreAz3SslKuWVbpeim1pqK+VVbhk7TaK0Tn6pmxFDsdcUKuvkFW4cm02gsEpe3dbJn24V1sgr2z72G0ddhXz/UzSfhh1HUVYhr2oKbDmGqnJ5RXOQOd2qKpZXMwt2HUFRqXzmf0jbHvYdRk2pvJJ5SN7UUlMor2Im7DyIkjL5z7oZMRR7D6GiSL710yArY/cBFBTJy89G/HSroEReej7s/xn5AvkpzrFhGDwhXSAvOyUUHpHNy0vOSeSRDdmsvNyssHhAMic/0Tk2DI975HLyUhND5A6pjLzMzPykcotUWl5ibrjcIpOUX/Ypmk+DzQ0SSXnx2Xk+3Uqk5IXn5+kDCeIJ+SnPsWEYvSMcl/+Mp2g+DU5/EY3LC74GD6db0ai82KvACoIx+fGfBlkZXlfEYvJCLwSxC0IReZFX4vZ0KxSWF3gtuL0hEpSf/hwbht0ZgaC81y8Hvcw/+1eH8CEPmR1A+JCHzA4gfMhDZgcQvpXfH4f8Xjnk98rvPcufvrzoAbaE05f333q743SWf727lWWcTqf/AWh9bguWJclIAAAAAElFTkSuQmCC";

            byte[] imageByteArray1 = Convert.FromBase64String(image1Base64);
            byte[] imageByteArray2 = Convert.FromBase64String(image2Base64);

            var magickImage1 = new MagickImage(imageByteArray1);
            var magickImage2 = new MagickImage(imageByteArray2);
            _magickImageHelper.Setup(x => x.Create(imageByteArray1)).Returns(magickImage1);
            _magickImageHelper.Setup(x => x.Create(imageByteArray2)).Returns(magickImage2);            

            var spriteImages = new List<MagickImage>();

            _imageProcessorMock.Setup(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), It.IsAny<string>())).Callback((IList<MagickImage> a, string b) => spriteImages.AddRange(a));

            var images = new List<IImage>
            {
                new ByteArrayImage("a", imageByteArray1),
                new ByteArrayImage("b", imageByteArray2),                
            };

            // Act
            _spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, "/sprite.png", @"c:\sprite.css"));

            // Assert
            _imageProcessorMock.Verify(x => x.CreateSprite(It.IsAny<MagickImageCollection>(), spriteFilename));
            Assert.Equal(2, spriteImages.Count);
            Assert.Equal(252, spriteImages[0].Width);
            Assert.Equal(175, spriteImages[0].Height);
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
