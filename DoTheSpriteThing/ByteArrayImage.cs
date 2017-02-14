namespace DoTheSpriteThing
{ 
    /// <summary>
    /// A byte array image.
    /// </summary>
    public class ByteArrayImage : IImage
    {
        /// <summary>
        /// Create the byte array image.
        /// </summary>
        /// <param name="key">The ID of the HTML element in which to display the image.</param>
        /// <param name="imageData">The image data.</param>        
        public ByteArrayImage(string key, byte[] imageData)
        {
            Key = key;
            ImageData = imageData;
            Resize = false;
        }

        /// <summary>
        /// Create the byte array image.
        /// </summary>
        /// <param name="key">The ID of the HTML element in which to display the image.</param>
        /// <param name="imageData">The image data.</param>
        /// <param name="resizeToHeight">The height in pixels to resize the image to.</param>
        /// <param name="resizeToWidth">The width in pixels to resize the image to.</param>
        public ByteArrayImage(string key, byte[] imageData, int resizeToHeight, int resizeToWidth)
        {
            Key = key;
            ImageData = imageData;
            Resize = true;
            ResizeToHeight = resizeToHeight;
            ResizeToWidth = resizeToWidth;
        }

        /// <summary>
        /// The ID of the HTML element in which to display the image.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The image data.
        /// </summary>
        public byte[] ImageData { get; }

        /// <summary>
        /// Should the image be resized in the sprite?
        /// </summary>
        public bool Resize { get; }

        /// <summary>
        /// The height in pixels to resize the image to.
        /// </summary>
        public int ResizeToHeight { get; }

        /// <summary>
        /// The width in pixels to resize the image to.
        /// </summary>
        public int ResizeToWidth { get; }

        /// <summary>
        /// The image to use when ImageData is null
        /// </summary>
        public string NoImageFilename { get; set; }
    }
}