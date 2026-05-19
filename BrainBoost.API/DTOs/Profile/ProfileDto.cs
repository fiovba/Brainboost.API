namespace BrainBoost.API.DTOs.Profile;

public class ProfileDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    public int XP { get; set; }

    public int Level { get; set; }

    public int LearningStreak { get; set; }
}