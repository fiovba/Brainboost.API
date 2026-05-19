namespace BrainBoost.API.DTOs.FlashcardSets;

public class CreateFlashcardInSetDto
{
    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public string? ImageUrl { get; set; }
}