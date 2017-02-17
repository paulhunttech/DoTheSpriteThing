using System.IO;

namespace DoTheSpriteThing
{
    internal class CssProcessor : ICssProcessor
    {
        public void CreateCss(string css, string cssFilename)
        {
            File.WriteAllText(cssFilename, css);
        }
    }
}