namespace BrainBoost.API.DTOs.Questions;

public class CreateQuestionDto
{
    public string QuestionText { get; set; } = null!;

    public string QuestionType { get; set; } = "SingleChoice";

    public string? Explanation { get; set; }

    public int Points { get; set; }

    public int OrderIndex { get; set; }

    public List<CreateAnswerOptionDto> Options { get; set; }
        = new();
}