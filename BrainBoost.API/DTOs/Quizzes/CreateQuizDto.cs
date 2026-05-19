namespace BrainBoost.API.DTOs.Quizzes;

public class CreateQuizDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public string Difficulty { get; set; } = "Easy";

    public int TimeLimitMinutes { get; set; }

    public int PassingScore { get; set; }
}