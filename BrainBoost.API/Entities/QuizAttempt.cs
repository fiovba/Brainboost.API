namespace BrainBoost.API.Entities;

public class QuizAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? SubmittedAt { get; set; }

    public int Score { get; set; }

    public int TotalPoints { get; set; }

    public int CorrectAnswersCount { get; set; }

    public int WrongAnswersCount { get; set; }

    public string Status { get; set; } = "InProgress";

    public int TimeSpentSeconds { get; set; }

    public ICollection<UserAnswer> Answers { get; set; }
        = new List<UserAnswer>();
}