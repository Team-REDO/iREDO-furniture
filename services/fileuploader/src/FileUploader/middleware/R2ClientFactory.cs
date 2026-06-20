using Amazon.S3;
namespace middleware
{
    public class R2ClientFactory
    {
            public static IAmazonS3 Create(string accountId, string accessKey, string secretKey)
        {
        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true
        };

        return new AmazonS3Client(accessKey, secretKey, config);
    }
    }
}