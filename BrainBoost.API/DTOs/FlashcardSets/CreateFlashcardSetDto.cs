namespace BrainBoost.API.DTOs.FlashcardSets;

public class CreateFlashcardSetDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public string Difficulty { get; set; } = "Easy";

    public List<CreateFlashcardInSetDto> Flashcards { get; set; } = new();
}