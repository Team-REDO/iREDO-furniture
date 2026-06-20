using Amazon.S3;
using Amazon.S3.Model;
using interfaces;

namespace service
{
    public class HttpFileStorageClient : IFileStorageClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _bucket;
        private readonly IAmazonS3 _s3;

        public HttpFileStorageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Models.UploadResponse> UploadFileAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            content.Add(new StreamContent(fileStream), "file", file.FileName);

            var response = await _httpClient.PostAsync("/upload", content);
            response.EnsureSuccessStatusCode();

            var uploadResponse = await response.Content.ReadFromJsonAsync<Models.UploadResponse>();
            return uploadResponse;
        }

        public async Task DeleteFileAsync(string key)
        {
            var request = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = key
            };

    await _s3.DeleteObjectAsync(request);
}
    }
}