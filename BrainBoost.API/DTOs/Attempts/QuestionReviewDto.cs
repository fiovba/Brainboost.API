namespace BrainBoost.API.DTOs.Attempts;

public class QuestionReviewDto
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string? Explanation { get; set; }

    public string SelectedAnswer { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int PointsEarned { get; set; }

    public int QuestionPoints { get; set; }
}