namespace BrainBoost.API.DTOs.Flashcards;

public class FlashcardDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsPublished { get; set; }
}