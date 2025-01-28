using BookStore.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IYandexStorageService _storageService;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IYandexStorageService storageService, ILogger<FileUploadController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            _logger.LogWarning("Попытка загрузить пустой файл.");
            return BadRequest(new { Error = "Файл не предоставлен или пустой." });
        }
        
        try
        {
            await _storageService.UploadFileAsync(file);
            _logger.LogInformation($"Файл {file.FileName} успешно загружен.");
            return CreatedAtAction(nameof(UploadFile), new { fileName = file.FileName }, 
                new { Message = "Файл успешно загружен", file.FileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при загрузке файла {file?.FileName}.");
            return StatusCode(500, new { Error = "Произошла ошибка при загрузке файла." });
        }
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _logger.LogWarning("Попытка скачать файл с пустым именем.");
            return BadRequest(new { Error = "Имя файла не предоставлено." });
        }
        
        try
        {
            var fileData = await _storageService.DownloadFileAsync(fileName);
            if (fileData.Length == 0)
            {
                _logger.LogWarning($"Файл {fileName} не найден.");
                return NotFound(new { Error = $"Файл {fileName} не найден." });
            }

            _logger.LogInformation($"Файл {fileName} успешно скачан.");
            return File(fileData, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при скачивании файла {fileName}.");
            return StatusCode(500, new { Error = "Произошла ошибка при скачивании файла." });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        try
        {
            var files = await _storageService.ListFilesAsync();
            _logger.LogInformation($"Получен список файлов ({files.Count()} шт.).");
            return Ok(files);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка файлов.");
            return StatusCode(500, new { Error = "Произошла ошибка при получении списка файлов." });
        }
    }

    [HttpGet("presigned-url/{fileName}")]
    public async Task<IActionResult> GetPreSignedUrl(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _logger.LogWarning("Попытка получить pre-signed URL с пустым именем файла.");
            return BadRequest(new { Error = "Имя файла не предоставлено." });
        }
        
        try
        {
            var url = await _storageService.GetPreSignedUrlAsync(fileName, TimeSpan.FromMinutes(30));
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogWarning($"Не удалось получить pre-signed URL для файла {fileName}.");
                return NotFound(new { Error = $"Не удалось получить ссылку для файла {fileName}." });
            }
            
            _logger.LogInformation($"Получен pre-signed URL для файла {fileName}.");
            return Ok(new { Url = url });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении pre-signed URL для файла {fileName}.");
            return StatusCode(500, new { Error = "Произошла ошибка при получении pre-signed URL." });
        }
    }
}