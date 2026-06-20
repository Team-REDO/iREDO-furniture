
using Amazon.S3;
using Amazon.S3.Model;
using interfaces;

namespace service
{
    public class R2StorageClient : IFileStorageClient
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public R2StorageClient(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        public async Task<Models.UploadResponse> UploadFileAsync(IFormFile file)
        {
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using var stream = file.OpenReadStream();
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            var url = $"https://{_bucketName}.r2.cloudflarestorage.com/{key}";
            return new Models.UploadResponse { FileName = file.FileName, Url = url };
        }
    }
}