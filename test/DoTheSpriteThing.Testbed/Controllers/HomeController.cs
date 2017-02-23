using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DoTheSpriteThing.Testbed.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> images = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, @"images\facebook.png")), "noimage1-png"),
                new ByteArraySpriteImage("b", null, "noimage2-png", 128, 128),
                new ByteArraySpriteImage("c", null, "noimage1-png", 128, 128),
                new ByteArraySpriteImage("d", null, "noimage2-png", 128, 128),
                new ByteArraySpriteImage("e", null, "faceboocksjdncjsdncjds")
            };

            IReadOnlyCollection<ISpriteImage> placeholderImages = new List<ISpriteImage>
            {
                new FileSpriteImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage1.png"))),
                new FileSpriteImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage2.png")))
            };

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "bytearrays-sprite.png");
            const string spriteUrl = "../images/sprites/bytearrays-sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\bytearrays-sprite.css");

            spriteManager.CreateSprite(images, placeholderImages, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult Files()
        {
            var spriteManager = new SpriteManager();
            string imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            List<FileSpriteImage> imageFiles = Directory.GetFiles(imagesFolder).Select(x => new FileSpriteImage(new FileInfo(x), 128, 128)).ToList();            
            imageFiles.Add(new FileSpriteImage(new FileInfo("aaaaaaaaaaa"), 128, 128));

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "files-sprite.png");
            const string spriteUrl = "../images/sprites/files-sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\files-sprite.css");

            spriteManager.CreateSprite(imageFiles, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult FilesAndByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> imageFiles = new List<ISpriteImage>
            {
                new ByteArraySpriteImage("a", System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, @"images\facebook.png")), Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage1.png"), 128, 128),
                new FileSpriteImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\bbc.png")), 128, 128)
            };

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "filesandbytearrays-sprite.png");
            const string spriteUrl = "../images/sprites/filesandbytearrays-sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\filesandbytearrays-sprite.css");

            spriteManager.CreateSprite(imageFiles, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult Hover()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<ISpriteImage> images = new List<ISpriteImage>
            {
                new FileSpriteImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\bbc.png")), new HoverFileImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\facebook.png"))))
            };            

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "hover-sprite.png");
            const string spriteUrl = "../images/sprites/hover-sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\hover-sprite.css");

            spriteManager.CreateSprite(images, null, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}