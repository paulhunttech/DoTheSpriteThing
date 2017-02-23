namespace DoTheSpriteThing
{
    public interface IByteArrayImage
    {
        string Key { get; }

        byte[] ImageData { get; }
    }
}