using BrainBoost.API.DTOs.Profile;

namespace BrainBoost.API.Interfaces;

public interface IProfileService
{
    Task<ProfileDto> GetMeAsync(int userId);
    Task DeleteAccountAsync(int userId);
    Task<ProfileDto> UpdateAsync(
        int userId,
        UpdateProfileDto dto
    );
}