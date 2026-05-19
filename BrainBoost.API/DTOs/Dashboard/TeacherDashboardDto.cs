namespace BrainBoost.API.DTOs.Dashboard;

public class TeacherDashboardDto
{
    public int CreatedQuizzesCount { get; set; }

    public int TotalQuestionsCount { get; set; }

    public int TotalStudentAttempts { get; set; }

    public double AverageQuizScore { get; set; }

    public int PublishedQuizzesCount { get; set; }

    public int DraftQuizzesCount { get; set; }

    public List<TeacherRecentQuizDto> RecentQuizzes { get; set; } = new();

    public List<TeacherRecentAttemptDto> RecentAttempts { get; set; } = new();

    public List<string> TopPerformingQuizzes { get; set; } = new();
}

public class TeacherRecentQuizDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public int QuestionCount { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class TeacherRecentAttemptDto
{
    public int Id { get; set; }

    public string StudentName { get; set; } = null!;

    public string QuizTitle { get; set; } = null!;

    public double ScorePercent { get; set; }

    public DateTime? SubmittedAt { get; set; }
}
