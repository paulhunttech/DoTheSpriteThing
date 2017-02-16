using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Files()
        {
            var spriteManager = new SpriteManager();
            string imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            IReadOnlyCollection<IImage> imageFiles = Directory.GetFiles(imagesFolder).Select(x => new FileImage(new FileInfo(x), 128, 128)).ToList();

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

        public IActionResult ByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<IImage> images = new List<IImage>
            {
                new ByteArrayImage("a", System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, @"images\facebook.png")), "noimage1-png"),
                new ByteArrayImage("b", null, "noimage2-png", 128, 128),
                new ByteArrayImage("c", null, "noimage1-png", 128, 128),
                new ByteArrayImage("d", null, "faceboocksjdncjsdncjds")
            };

            IReadOnlyCollection<IImage> placeholderImages = new List<IImage>
            {                
                new FileImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage1.png"))),
                new FileImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage2.png")))
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

        public IActionResult FilesAndByteArrays()
        {
            var spriteManager = new SpriteManager();
            IReadOnlyCollection<IImage> imageFiles = new List<IImage>
            {
                new ByteArrayImage("a", System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, @"images\facebook.png")), Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage1.png"), 128, 128),
                new FileImage(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, @"images\bbc.png")), 128, 128)                
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
    }
}
