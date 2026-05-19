namespace BrainBoost.API.DTOs.FlashcardSets;

public class UpdateFlashcardSetDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public string Difficulty { get; set; } = "Easy";

    public List<UpdateFlashcardInSetDto> Flashcards { get; set; } = new();
}