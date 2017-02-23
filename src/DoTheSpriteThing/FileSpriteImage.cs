using System.IO;

namespace DoTheSpriteThing
{
    /// <summary>
    /// A file image.
    /// </summary>
    public class FileSpriteImage : ISpriteImage, IFileImage
    {
        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>
        /// <param name="placeholderImageKey">The key of the placeholder image to use when image data is null.</param>        
        public FileSpriteImage(FileInfo filePath, string placeholderImageKey)
        {
            FilePath = filePath;
            PlaceholderImageKey = placeholderImageKey;
            Resize = false;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>
        /// <param name="placeholderImageKey">The key of the placeholder image to use when image data is null.</param>   
        /// <param name="hoverImage">The image to display when hovering over the image.</param>             
        public FileSpriteImage(FileInfo filePath, string placeholderImageKey, IHoverImage hoverImage)
        {
            FilePath = filePath;
            PlaceholderImageKey = placeholderImageKey;
            HoverImage = hoverImage;
            Resize = false;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>        
        public FileSpriteImage(FileInfo filePath)
        {
            FilePath = filePath;
            Resize = false;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>        
        /// <param name="hoverImage">The image to display when hovering over the image.</param>        
        public FileSpriteImage(FileInfo filePath, IHoverImage hoverImage)
        {            
            Resize = false;
            FilePath = filePath;
            HoverImage = hoverImage;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>
        /// <param name="placeholderImageKey">The key of the placeholder image to use when image data is null.</param>        
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>
        public FileSpriteImage(FileInfo filePath, string placeholderImageKey, int resizeToHeight, int resizeToWidth)
        {        
            Resize = true;
            FilePath = filePath;
            PlaceholderImageKey = placeholderImageKey;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>
        /// <param name="placeholderImageKey">The key of the placeholder image to use when image data is null.</param>        
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>
        /// <param name="hoverImage">The image to display when hovering over the image.</param>        
        public FileSpriteImage(FileInfo filePath, string placeholderImageKey, int resizeToHeight, int resizeToWidth, IHoverImage hoverImage)
        {
            Resize = true;
            FilePath = filePath;
            PlaceholderImageKey = placeholderImageKey;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
            HoverImage = hoverImage;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>        
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>
        public FileSpriteImage(FileInfo filePath, int resizeToHeight, int resizeToWidth)
        {            
            Resize = true;
            FilePath = filePath;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>
        /// <param name="filePath">The path of the image file.</param>        
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>
        /// <param name="hoverImage">The image to display when hovering over the image.</param>        
        public FileSpriteImage(FileInfo filePath, int resizeToHeight, int resizeToWidth, IHoverImage hoverImage)
        {
            Resize = true;
            FilePath = filePath;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
            HoverImage = hoverImage;
        }

        public string Key => Path.GetFileName(FilePath.FullName).Replace(".", "-");        

        public bool Resize { get; }

        public int ResizeToHeight { get; }

        public int ResizeToWidth { get; }

        public IHoverImage HoverImage { get; }

        public string PlaceholderImageKey { get; }

        public FileInfo FilePath { get; set; }
    }
}