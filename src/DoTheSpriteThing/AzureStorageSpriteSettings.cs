using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ImageMagick;

namespace DoTheSpriteThing
{
    /// <summary>
    /// The sprite settings.
    /// </summary>
    public class AzureStorageSpriteSettings : ISpriteSettings
    {
        private readonly BlobContainerClient _cssBlobContainerClient;
        private readonly BlobContainerClient _spriteBlobContainerClient;

        /// <summary>
        /// Create the sprite settings.
        /// </summary>
        /// <param name="spriteFilename">The name of the sprite file that will be created or overwritten.</param>
        /// <param name="spriteUrl">The URL of the sprite file.</param>
        /// <param name="cssFilename">The name of the CSS file that will be created or overwritten.</param>
        /// <param name="connectionString"></param>
        /// <param name="cssContainerName"></param>
        /// <param name="spriteContainerName"></param>                
        public AzureStorageSpriteSettings(string spriteFilename, string spriteUrl, string cssFilename, string connectionString, string cssContainerName, string spriteContainerName)
        {
            SpriteFilename = spriteFilename;
            SpriteUrl = spriteUrl;
            CssFilename = cssFilename;
            ConnectionString = connectionString;
            CssContainerName = cssContainerName;
            SpriteContainerName = spriteContainerName;

            var blobServiceClient = new BlobServiceClient(ConnectionString);

            var cssBlobContainer = blobServiceClient.GetBlobContainers().FirstOrDefault(x => x.Name == CssContainerName);
            _cssBlobContainerClient = cssBlobContainer == null
                ? blobServiceClient.CreateBlobContainer(CssContainerName)
                : blobServiceClient.GetBlobContainerClient(CssContainerName);

            var spriteBlobContainer = blobServiceClient.GetBlobContainers().FirstOrDefault(x => x.Name == SpriteContainerName);
            _spriteBlobContainerClient = spriteBlobContainer == null
                ? blobServiceClient.CreateBlobContainer(SpriteContainerName)
                : blobServiceClient.GetBlobContainerClient(SpriteContainerName);
        }

        /// <summary>
        /// The name of the CSS file that will be created or overwritten.
        /// </summary>
        public string CssFilename { get; }

        /// <summary>
        /// The name of the sprite file that will be created or overwritten.
        /// </summary>
        public string SpriteFilename { get; }

        /// <summary>
        /// The URL of the sprite file.
        /// </summary>
        public string SpriteUrl { get; }

        public string ConnectionString { get; }

        public string CssContainerName { get; set; }

        public string SpriteContainerName { get; set; }

        public async Task SaveSpriteAsync(IMagickImage sprite)
        {
            await using var spriteStream = new MemoryStream();
            sprite.Write(spriteStream);
            spriteStream.Position = 0;
            await UploadFileAsync(_spriteBlobContainerClient, spriteStream, SpriteFilename);
        }

        public async Task SaveCssAsync(string css)
        {
            await using var stream = new MemoryStream();
            await using var writer = new StreamWriter(stream);
            await writer.WriteAsync(css);
            await writer.FlushAsync();
            stream.Position = 0;
            await UploadFileAsync(_cssBlobContainerClient, stream, CssFilename);
        }

        private static async Task UploadFileAsync(BlobContainerClient blobContainerClient, Stream fileStream, string blobName)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, true);
        }
    }
}