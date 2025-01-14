using BookStore.Core.Model.Catalog;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Services;

public class ImageService : IImageService
{
    private readonly IYandexStorageService _yandexStorageService;

    public ImageService(IYandexStorageService yandexStorageService)
    {
        _yandexStorageService = yandexStorageService;
    }
    public async Task UploadImageAsync(IFormFile image, string imageName = "")
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Изображение не выбрано или пусто.", nameof(image));
        }

        if (!image.ContentType.StartsWith("image/"))
        {
            throw new ArgumentException("Недопустимый формат файла. Ожидается изображение.", nameof(image));
        }
        await _yandexStorageService.UploadFileAsync(image);
    }
    
}

public interface IImageService
{
    Task UploadImageAsync(IFormFile image, string imageName = "");
}