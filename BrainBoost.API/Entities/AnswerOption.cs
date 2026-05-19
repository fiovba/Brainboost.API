namespace BrainBoost.API.Entities;

public class AnswerOption
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public string OptionText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int OrderIndex { get; set; }
}