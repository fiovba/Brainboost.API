namespace BrainBoost.API.DTOs.Flashcards;

public class FlashcardFilterDto
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public string? Difficulty { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}