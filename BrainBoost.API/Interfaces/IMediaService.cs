using BrainBoost.API.DTOs.Media;

namespace BrainBoost.API.Interfaces;

public interface IMediaService
{
    Task<MediaFileDto> UploadAsync(IFormFile file, int userId);

    Task<MediaFileDto> GetByIdAsync(int id);

    Task DeleteAsync(int id);
}