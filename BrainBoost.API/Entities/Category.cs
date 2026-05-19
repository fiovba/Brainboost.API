namespace BrainBoost.API.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? CoverImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
    public ICollection<Quiz> Quizzes { get; set; }
    = new List<Quiz>();
}