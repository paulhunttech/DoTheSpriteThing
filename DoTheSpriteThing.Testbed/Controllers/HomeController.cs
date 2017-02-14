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
            var spriteManager = new SpriteManager();
            string imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            IReadOnlyCollection<IImage> imageFiles = Directory.GetFiles(imagesFolder).Select(x => new FileImage(new FileInfo(x)/*, 128, 128*/)).ToList();

            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "sprite.png");
            const string spriteUrl = "../images/sprites/sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\sprite.css");            

            spriteManager.CreateSprite(imageFiles, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));            

            return View();
        }                
    }
}
