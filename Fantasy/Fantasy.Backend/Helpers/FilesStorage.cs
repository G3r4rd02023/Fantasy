using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace Fantasy.Backend.Helpers
{
    public class FilesStorage : IFilesStorage
    {
        private readonly Cloudinary _cloudinary;

        public FilesStorage(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> SaveFileAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                PublicId = fileName
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }

        public async Task<bool> RemoveFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            return deletionResult.Result == "ok";
        }
    }
}