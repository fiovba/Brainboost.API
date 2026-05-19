namespace BrainBoost.API.Entities;

public class Bookmark
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int? QuizId { get; set; }

    public Quiz? Quiz { get; set; }

    public int? FlashcardId { get; set; }

    public Flashcard? Flashcard { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}