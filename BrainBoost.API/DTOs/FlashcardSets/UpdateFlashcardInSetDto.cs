namespace BrainBoost.API.DTOs.FlashcardSets;

public class UpdateFlashcardInSetDto
{
    public int? Id { get; set; }

    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public string? ImageUrl { get; set; }
}