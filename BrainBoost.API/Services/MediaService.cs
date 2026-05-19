using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Media;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class MediaService : IMediaService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MediaService(
        AppDbContext context,
        IWebHostEnvironment env,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<MediaFileDto> UploadAsync(IFormFile file, int userId)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Fayl seçilməyib.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Yalnız jpg, jpeg, png və webp faylları qəbul olunur.");

        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("Fayl maksimum 5MB ola bilər.");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var request = _httpContextAccessor.HttpContext!.Request;

        var fileUrl = $"{request.Scheme}://{request.Host}/uploads/{uniqueFileName}";

        var mediaFile = new MediaFile
        {
            FileName = uniqueFileName,
            OriginalFileName = file.FileName,
            FileUrl = fileUrl,
            FileType = "Image",
            MimeType = file.ContentType,
            Size = file.Length,
            UploadedByUserId = userId
        };

        _context.MediaFiles.Add(mediaFile);
        await _context.SaveChangesAsync();

        return ToDto(mediaFile);
    }

    public async Task<MediaFileDto> GetByIdAsync(int id)
    {
        var media = await _context.MediaFiles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (media == null)
            throw new Exception("Media tapılmadı.");

        return ToDto(media);
    }

    public async Task DeleteAsync(int id)
    {
        var media = await _context.MediaFiles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (media == null)
            throw new Exception("Media tapılmadı.");

        var filePath = Path.Combine(
            _env.WebRootPath,
            "uploads",
            media.FileName
        );

        if (File.Exists(filePath))
            File.Delete(filePath);

        _context.MediaFiles.Remove(media);
        await _context.SaveChangesAsync();
    }

    private static MediaFileDto ToDto(MediaFile media)
    {
        return new MediaFileDto
        {
            Id = media.Id,
            FileName = media.FileName,
            OriginalFileName = media.OriginalFileName,
            FileUrl = media.FileUrl,
            FileType = media.FileType,
            MimeType = media.MimeType,
            Size = media.Size
        };
    }
}