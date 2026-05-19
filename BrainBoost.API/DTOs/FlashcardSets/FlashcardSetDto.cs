namespace BrainBoost.API.DTOs.FlashcardSets;

public class FlashcardSetDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public int CardCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<FlashcardInSetDto> Flashcards { get; set; } = new();
}

public class FlashcardInSetDto
{
    public int Id { get; set; }

    public string FrontText { get; set; } = null!;

    public string BackText { get; set; } = null!;

    public string? ImageUrl { get; set; }

  
}