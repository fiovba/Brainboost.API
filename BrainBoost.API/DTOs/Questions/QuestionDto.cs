namespace BrainBoost.API.DTOs.Questions;

public class QuestionDto
{
    public int Id { get; set; }

    public string QuestionText { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string? Explanation { get; set; }

    public int Points { get; set; }

    public int OrderIndex { get; set; }

    public List<AnswerOptionDto> Options { get; set; }
        = new();
}