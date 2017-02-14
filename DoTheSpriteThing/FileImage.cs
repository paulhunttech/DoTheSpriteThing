using System.IO;

namespace DoTheSpriteThing
{
    /// <summary>
    /// A file image.
    /// </summary>
    public class FileImage : IImage
    {
        /// <summary>
        /// Create the file image.
        /// </summary>        
        /// <param name="filePath">The path of the image file.</param>        
        public FileImage(FileInfo filePath)
        {
            FilePath = filePath;
            Resize = false;
        }

        /// <summary>
        /// Create the file image.
        /// </summary>        
        /// <param name="filePath">The path of the image file.</param>    
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>    
        public FileImage(FileInfo filePath, int resizeToHeight, int resizeToWidth)
        {
            FilePath = filePath;
            Resize = true;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
        }

        public FileInfo FilePath { get; }

        public bool Resize { get; }

        public int ResizeToHeight { get; }

        public int ResizeToWidth { get; }
    }
}