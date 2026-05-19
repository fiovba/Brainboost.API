namespace BrainBoost.API.DTOs.Quizzes;

public class QuizDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string Difficulty { get; set; } = null!;

    public int TimeLimitMinutes { get; set; }

    public int PassingScore { get; set; }

    public bool IsPublished { get; set; }
    public string TeacherName { get; set; } = null!;
    public bool IsApproved { get; set; }

    public string CategoryName { get; set; } = null!;
}