namespace DoTheSpriteThing
{
    /// <summary>
    /// The sprite settings.
    /// </summary>
    public class SpriteSettings
    {
        /// <summary>
        /// Create the sprite settings.
        /// </summary>
        /// <param name="spriteFilename">The name of the sprite file that will be created or overwritten.</param>
        /// <param name="spriteUrl">The URL of the sprite file.</param>
        /// <param name="cssFilename">The name of the CSS file that will be created or overwritten.</param>        
        /// <param name="quality">The quality of the image as a percentage.</param>
        public SpriteSettings(string spriteFilename, string spriteUrl, string cssFilename, int quality = 100)
        {
            SpriteFilename = spriteFilename;
            SpriteUrl = spriteUrl;
            CssFilename = cssFilename;            
            Quality = quality;
        }

        /// <summary>
        /// The name of the sprite file that will be created or overwritten.
        /// </summary>
        public string SpriteFilename { get; }

        /// <summary>
        /// The URL of the sprite file.
        /// </summary>
        public string SpriteUrl { get; }

        /// <summary>
        /// The name of the CSS file that will be created or overwritten.
        /// </summary>
        public string CssFilename { get; }        

        /// <summary>
        /// The quality of the image as a percentage.
        /// </summary>
        public int Quality { get; }        
    }
}