namespace BrainBoost.API.DTOs.Attempts;

public class AttemptReviewDto
{
    public int AttemptId { get; set; }
    public string QuizTitle { get; set; } = null!;

    public int Score { get; set; }

    public int TotalPoints { get; set; }

    public double Percentage { get; set; }

    public List<QuestionReviewDto> Questions { get; set; } = new();
}