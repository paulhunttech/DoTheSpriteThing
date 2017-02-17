namespace DoTheSpriteThing
{
    internal class SpritePlaceholderImage
    {
        internal SpritePlaceholderImage(string key, int top, int height, int width)
        {
            Key = key;
            Top = top;
            Height = height;
            Width = width;
        }

        internal int Height { get; set; }

        internal string Key { get; set; }

        internal int Top { get; set; }

        internal int Width { get; set; }
    }
}