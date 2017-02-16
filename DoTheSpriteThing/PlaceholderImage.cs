namespace DoTheSpriteThing
{
    internal class PlaceholderImage
    {
        internal PlaceholderImage(string key, int top, int height, int width)
        {
            Key = key;
            Top = top;
            Height = height;
            Width = width;
        }

        internal string Key { get; set; }

        internal int Top { get; set; }

        internal int Height { get; set; }

        internal int Width { get; set; }
    }
}