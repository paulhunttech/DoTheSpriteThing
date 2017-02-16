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
            IReadOnlyCollection<IImage> imageFiles = new List<ByteArrayImage>
            {
                new ByteArrayImage("a", null, Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage1.png"), 128, 128),
                new ByteArrayImage("b", null, Path.Combine(_hostingEnvironment.WebRootPath, @"images\noimage2.png"), 128, 128)
            };

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "bytearrays-sprite.png");
            const string spriteUrl = "../images/sprites/bytearrays-sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\bytearrays-sprite.css");

            spriteManager.CreateSprite(imageFiles, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));

            return View();
        }
    }
}
