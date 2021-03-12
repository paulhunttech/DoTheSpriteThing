using System.Collections.Generic;
using System.IO;
using System.Linq;
using DoTheSpriteThing.Images;
using DoTheSpriteThing.Images.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace DoTheSpriteThing.Testbed.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _webRootPath;

        public HomeController(IHostEnvironment hostEnvironment)
        {
            _webRootPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");
        }

        public IActionResult ByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> images = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", System.IO.File.ReadAllBytes(Path.Combine(_webRootPath, @"images\png\facebook.png")), "noimage1-png"),
                new ByteArraySpriteImage("b", null, "noimage2-png", 128, 128),
                new ByteArraySpriteImage("c", null, "noimage1-png", 128, 128),
                new ByteArraySpriteImage("d", null, "noimage2-png", 128, 128),
                new ByteArraySpriteImage("e", null, "faceboocksjdncjsdncjds")
            };

            IReadOnlyCollection<ISpriteImage> placeholderImages = new List<ISpriteImage>
            {
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\png\noimage1.png"))),
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\png\noimage2.png")))
            };

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            var spriteFilename = Path.Combine(spriteFolder, "bytearrays-sprite.png");
            const string spriteUrl = "../images/sprites/bytearrays-sprite.png";
            var cssFilename = Path.Combine(_webRootPath, @"css\bytearrays-sprite.css");

            spriteManager.CreateSprite(images, placeholderImages, new FileSystemSpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult FilesPng()
        {
            var spriteManager = new SpriteManager();
            var imagesFolder = Path.Combine(_webRootPath, "images", "png");
            var imageFiles = Directory.GetFiles(imagesFolder).Select(x => new FileSpriteImage(new FileInfo(x), 128, 128)).ToList();
            imageFiles.Add(new FileSpriteImage(new FileInfo("aaaaaaaaaaa"), 128, 128));

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            var spriteFilename = Path.Combine(spriteFolder, "files-sprite.png");
            const string spriteUrl = "../images/sprites/files-sprite.png";
            var cssFilename = Path.Combine(_webRootPath, @"css\files-sprite.css");

            spriteManager.CreateSprite(imageFiles, new FileSystemSpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult FilesJpg()
        {
            var spriteManager = new SpriteManager();
            var imagesFolder = Path.Combine(_webRootPath, "images", "jpg");
            var imageFiles = Directory.GetFiles(imagesFolder).Select(x => new FileSpriteImage(new FileInfo(x), 128, 128)).ToList();
            imageFiles.Add(new FileSpriteImage(new FileInfo("aaaaaaaaaaa"), 128, 128));

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            var spriteFilename = Path.Combine(spriteFolder, "files-sprite.jpg");
            const string spriteUrl = "../images/sprites/files-sprite.jpg";
            var cssFilename = Path.Combine(_webRootPath, @"css\files-sprite.css");

            spriteManager.CreateSprite(imageFiles, new FileSystemSpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult FilesAndByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> imageFiles = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", System.IO.File.ReadAllBytes(Path.Combine(_webRootPath, @"images\png\facebook.png")), Path.Combine(_webRootPath, @"images\noimage1.png"), 128, 128),
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\bbc.png")), 128, 128)
            };

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            var spriteFilename = Path.Combine(spriteFolder, "filesandbytearrays-sprite.png");
            const string spriteUrl = "../images/sprites/filesandbytearrays-sprite.png";
            var cssFilename = Path.Combine(_webRootPath, @"css\filesandbytearrays-sprite.css");

            spriteManager.CreateSprite(imageFiles, new FileSystemSpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult Hover()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> images = new List<ISpriteImage>
            {
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\png\bbc.png")), new HoverFileImage(new FileInfo(Path.Combine(_webRootPath, @"images\facebook.png"))))
            };

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            var spriteFilename = Path.Combine(spriteFolder, "hover-sprite.png");
            const string spriteUrl = "../images/sprites/hover-sprite.png";
            var cssFilename = Path.Combine(_webRootPath, @"css\hover-sprite.css");

            spriteManager.CreateSprite(images, null, new FileSystemSpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AzureStorage()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> images = new List<ISpriteImage>
            {
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\png\facebook.png")), 128, 128),
                new FileSpriteImage(new FileInfo(Path.Combine(_webRootPath, @"images\bbc.png")), 128, 128)
            };

            var spriteFolder = Path.Combine(_webRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            const string spriteFilename = "azure-storage-sprite.png";
            const string spriteUrl = "http://127.0.0.1:10000/devstoreaccount1/sprite-testbed/azure-storage-sprite.png";
            const string cssFilename = "azure-storage-sprite.css";

            spriteManager.CreateSprite(images, null, new AzureStorageSpriteSettings(spriteFilename, spriteUrl, cssFilename, "UseDevelopmentStorage=true", "css-testbed", "sprite-testbed"));

            return View();
        }
    }
}