namespace BrainBoost.API.Entities;

public class Question
{
    public int Id { get; set; }

    public int QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public string QuestionType { get; set; } = "SingleChoice";

    public string? Explanation { get; set; }

    public int Points { get; set; }

    public int OrderIndex { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AnswerOption> Options { get; set; }
        = new List<AnswerOption>();
}