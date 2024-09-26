namespace Fantasy.Backend.Helpers
{
    public interface IFilesStorage
    {
        Task<string> SaveFileAsync(Stream imageStream, string fileName);

        Task<bool> RemoveFileAsync(string publicId);
    }
}