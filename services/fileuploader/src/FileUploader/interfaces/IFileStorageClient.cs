namespace interfaces
{
    public interface IFileStorageClient
    {
        Task<Models.UploadResponse> UploadFileAsync(IFormFile file);
    }
}