namespace BrainBoost.API.Entities;

public class FlashcardProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int FlashcardId { get; set; }

    public Flashcard Flashcard { get; set; } = null!;

    public int KnownLevel { get; set; }

    public int ReviewCount { get; set; }

    public DateTime? LastReviewedAt { get; set; }

    public DateTime? NextReviewAt { get; set; }
}