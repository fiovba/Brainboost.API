namespace BrainBoost.API.DTOs.Profile;

public class UpdateProfileDto
{
    public string FullName { get; set; } = null!;

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }
}