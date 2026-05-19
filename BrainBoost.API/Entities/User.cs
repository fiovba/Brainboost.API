namespace BrainBoost.API.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    public int XP { get; set; }

    public int Level { get; set; } = 1;

    public int LearningStreak { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Quiz> CreatedQuizzes { get; set; }
    = new List<Quiz>();
}