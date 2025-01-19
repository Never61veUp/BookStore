using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Abstractions;

public interface IYandexStorageService
{
    Task UploadFileAsync(IFormFile file);
    Task<byte[]> DownloadFileAsync(string fileName);
    Task<IEnumerable<string>> ListFilesAsync();
    Task<string> GetPreSignedUrlAsync(string fileName, TimeSpan expiration);
}