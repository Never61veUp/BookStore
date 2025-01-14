using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Services;

public class YandexStorageService : IYandexStorageService
{
    private readonly string _bucketName;
    private readonly string _serviceUrl;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly AmazonS3Client _client;

    public YandexStorageService(IConfiguration configuration)
    {
        var yandexConfig = configuration.GetSection("YandexCloud");
        _bucketName = yandexConfig["BucketName"];
        _serviceUrl = yandexConfig["ServiceUrl"];
        _accessKey = yandexConfig["AccessKey"];
        _secretKey = yandexConfig["SecretKey"];

        var s3Config = new AmazonS3Config
        {
            ServiceURL = _serviceUrl
        };

        _client = new AmazonS3Client(_accessKey, _secretKey, s3Config);
    }

    public async Task UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Файл не выбран или пуст.", nameof(file));
        }

        using var transferUtility = new TransferUtility(_client);
        using var stream = file.OpenReadStream();

        var request = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            BucketName = _bucketName,
            Key = file.FileName,
            ContentType = file.ContentType
        };

        await transferUtility.UploadAsync(request);
    }

    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        var response = await _client.GetObjectAsync(_bucketName, fileName);

        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    public async Task<IEnumerable<string>> ListFilesAsync()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName
        };

        var response = await _client.ListObjectsV2Async(request);
        return response.S3Objects.Select(o => o.Key);
    }

    public async Task<string> GetPreSignedUrlAsync(string fileName, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.Add(expiration)
        };

        return await _client.GetPreSignedURLAsync(request);
    }
}
public interface IYandexStorageService
{
    Task UploadFileAsync(IFormFile file);
    Task<byte[]> DownloadFileAsync(string fileName);
    Task<IEnumerable<string>> ListFilesAsync();
    Task<string> GetPreSignedUrlAsync(string fileName, TimeSpan expiration);
}