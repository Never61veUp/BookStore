using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Abstractions;

public interface IImageService
{
    Task UploadImageAsync(IFormFile image, string imageName = "");
}