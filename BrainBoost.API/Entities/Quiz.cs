namespace BrainBoost.API.Entities;

public class Quiz
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public string Difficulty { get; set; } = "Easy";

    public int TimeLimitMinutes { get; set; }

    public int PassingScore { get; set; }

    public bool IsPublished { get; set; } = false;

    public bool IsApproved { get; set; } = false;

    public int CreatedByUserId { get; set; }

    public User CreatedByUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Question> Questions { get; set; }
        = new List<Question>();
}