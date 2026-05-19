namespace BrainBoost.API.DTOs.Attempts;

public class AttemptDto
{
    public int Id { get; set; }

    public string QuizTitle { get; set; } = null!;

    public int Score { get; set; }

    public int TotalPoints { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }
}