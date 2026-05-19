namespace BrainBoost.API.Entities;

public class FlashcardSet
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public string Difficulty { get; set; } = "Easy";

    public int CreatedByUserId { get; set; }

    public User CreatedByUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Flashcard> Flashcards { get; set; } = new();
}