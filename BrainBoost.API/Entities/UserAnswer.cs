namespace BrainBoost.API.Entities;

public class UserAnswer
{
    public int Id { get; set; }

    public int AttemptId { get; set; }

    public QuizAttempt Attempt { get; set; } = null!;

    public int QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public int SelectedOptionId { get; set; }

    public AnswerOption SelectedOption { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int PointsEarned { get; set; }
}