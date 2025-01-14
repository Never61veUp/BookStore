using Amazon.S3;
using Amazon.S3.Transfer;
using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IYandexStorageService _storageService;

    public FileUploadController(IYandexStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            await _storageService.UploadFileAsync(file);
            return Ok(new { Message = "Файл успешно загружен", FileName = file.FileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        try
        {
            var fileData = await _storageService.DownloadFileAsync(fileName);
            return File(fileData, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        try
        {
            var files = await _storageService.ListFilesAsync();
            return Ok(files);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("presigned-url/{fileName}")]
    public async Task<IActionResult> GetPreSignedUrl(string fileName)
    {
        try
        {
            var url = await _storageService.GetPreSignedUrlAsync(fileName, TimeSpan.FromMinutes(30));
            return Ok(new { Url = url });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}