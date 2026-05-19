namespace BrainBoost.API.DTOs.Attempts;

public class AttemptResultDto
{
    public int AttemptId { get; set; }

    public int Score { get; set; }

    public int TotalPoints { get; set; }

    public double Percentage { get; set; }

    public bool Passed { get; set; }

    public int CorrectAnswersCount { get; set; }

    public int WrongAnswersCount { get; set; }

    public int TimeSpentSeconds { get; set; }
}