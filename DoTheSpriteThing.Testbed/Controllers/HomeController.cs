using System.IO;
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
            string spriteFolder = Path.Combine(_hostingEnvironment.WebRootPath, @"images\sprites");

            if (!Directory.Exists(spriteFolder))
            {
                Directory.CreateDirectory(spriteFolder);
            }

            string spriteFilename = Path.Combine(spriteFolder, "sprite.png");
            const string spriteUrl = "../images/sprites/sprite.png";
            string cssFilename = Path.Combine(_hostingEnvironment.WebRootPath, @"css\sprite.css");            

            spriteManager.CreateSprite(imagesFolder, new SpriteSettings(spriteFilename, spriteUrl, cssFilename));            

            return View();
        }                
    }
}
