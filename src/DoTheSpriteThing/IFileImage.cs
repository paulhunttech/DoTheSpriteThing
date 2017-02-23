using System.IO;

namespace DoTheSpriteThing
{
    public interface IFileImage
    {
        FileInfo FilePath { get; }

        string Key { get; }
    }
}