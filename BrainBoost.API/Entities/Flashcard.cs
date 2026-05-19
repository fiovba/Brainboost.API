namespace BrainBoost.API.Entities;

public class Flashcard
{
    public int? FlashcardSetId { get; set; }

    public FlashcardSet? FlashcardSet { get; set; }
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public string Difficulty { get; set; } = "Easy";

    public string? ImageUrl { get; set; }

    public int CreatedByUserId { get; set; }

    public User CreatedByUser { get; set; } = null!;

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}