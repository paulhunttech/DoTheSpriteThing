using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DoTheSpriteThing.Images;

namespace DoTheSpriteThing.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var images = new List<FileSpriteImage>();            
            string[] imageFilenames = Directory.GetFiles(@"..\DoTheSpriteThing.Testbed\wwwroot\images");            

            foreach (string imageFilename in imageFilenames)
            {
                images.Add(new FileSpriteImage(new FileInfo(imageFilename)));
            }

            var spriteManager = new SpriteManager();
            var runTimes = new List<long>();

            for (var i = 0; i < 20; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                spriteManager.CreateSprite(images, new SpriteSettings("sprite.png", "/sprite.png", "sprite.css"));
                stopwatch.Stop();
                runTimes.Add(stopwatch.ElapsedMilliseconds);
                Console.WriteLine($"Execution {i} time: {stopwatch.ElapsedMilliseconds}ms");
            }  
            
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Finished");
            Console.WriteLine($"Total execution time: {runTimes.Sum()}ms");
            Console.WriteLine($"Average execution time: {runTimes.Average()}ms");
            Console.WriteLine("--------------------------------------");

            Console.ReadLine();
        }
    }
}