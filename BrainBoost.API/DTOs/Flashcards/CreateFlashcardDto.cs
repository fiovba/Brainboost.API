namespace BrainBoost.API.DTOs.Flashcards;

public class CreateFlashcardDto
{
    public string Title { get; set; } = null!;

    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Difficulty { get; set; } = "Easy";

    public string? ImageUrl { get; set; }
}