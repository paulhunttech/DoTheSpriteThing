namespace DoTheSpriteThing
{
    public interface ISpriteImage : IImage
    {
        string Key { get; }

        string PlaceholderImageKey { get; }

        bool Resize { get; }

        int ResizeToHeight { get; }

        int ResizeToWidth { get; }

        IHoverImage HoverImage { get; }
    }
}