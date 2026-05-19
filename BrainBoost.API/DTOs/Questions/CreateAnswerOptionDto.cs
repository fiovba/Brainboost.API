namespace BrainBoost.API.DTOs.Questions;

public class CreateAnswerOptionDto
{
    public string OptionText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int OrderIndex { get; set; }
}