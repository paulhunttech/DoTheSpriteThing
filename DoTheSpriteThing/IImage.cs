namespace DoTheSpriteThing
{
    public interface IImage
    {
        string Key { get; }

        string PlaceholderImageKey { get; }

        bool Resize { get; }

        int ResizeToHeight { get; }

        int ResizeToWidth { get; }
    }
}