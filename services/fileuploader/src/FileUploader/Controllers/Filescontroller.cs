
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

namespace Controllers
{
    


[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IAmazonS3 _s3;
    private readonly IConfiguration _config;

    public FilesController(IAmazonS3 s3, IConfiguration config)
    {
        _s3 = s3;
        _config = config;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        var bucket = _config["R2:Bucket"];
        var accountId = _config["R2:AccountId"];

        var key = $"uploads/{Guid.NewGuid()}_{file.FileName}";

        using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await _s3.PutObjectAsync(request);

        var url = $"https://{accountId}.r2.dev/{key}";

        return Ok(new { key, url });
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest("Invalid key");

        var bucket = _config["R2:Bucket"];

        var request = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = key
        };

        await _s3.DeleteObjectAsync(request);

        return Ok(new { message = "Deleted", key });
    }
}}