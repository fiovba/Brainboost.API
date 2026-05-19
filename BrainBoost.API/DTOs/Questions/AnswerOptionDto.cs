namespace BrainBoost.API.DTOs.Questions;

public class AnswerOptionDto
{
    public int Id { get; set; }

    public string OptionText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int OrderIndex { get; set; }
}