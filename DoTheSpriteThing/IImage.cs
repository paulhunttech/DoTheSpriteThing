namespace DoTheSpriteThing
{
    public interface IImage
    {
        bool Resize { get; }

        int ResizeToHeight { get; }

        int ResizeToWidth { get; }
    }
}