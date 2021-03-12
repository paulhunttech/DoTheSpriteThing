using DoTheSpriteThing.FileProcessors.Interfaces;

namespace DoTheSpriteThing.FileProcessors
{
    internal class CssProcessor : ICssProcessor
    {
        public void CreateCss(string css, ISpriteSettings spriteSettings)
        {
            spriteSettings.SaveCssAsync(css);
        }
    }
}