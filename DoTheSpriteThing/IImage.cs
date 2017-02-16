namespace DoTheSpriteThing
{
    public interface IImage
    {
        string Key { get; }

        bool Resize { get; }

        int ResizeToHeight { get; }

        int ResizeToWidth { get; }
    }
}