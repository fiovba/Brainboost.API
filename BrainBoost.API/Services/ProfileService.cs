using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Profile;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _context;

    public ProfileService(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAccountAsync(int userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("User tapılmadı.");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }
    public async Task<ProfileDto> GetMeAsync(int userId)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("User tapılmadı.");

        return new ProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            Bio = user.Bio,
            ProfileImageUrl = user.ProfileImageUrl,
            XP = user.XP,
            Level = user.Level,
            LearningStreak = user.LearningStreak
        };
    }

    public async Task<ProfileDto> UpdateAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("User tapılmadı.");

        user.FullName = dto.FullName;
        user.Bio = dto.Bio;
        user.ProfileImageUrl = dto.ProfileImageUrl;

        await _context.SaveChangesAsync();

        return await GetMeAsync(userId);
    }
}