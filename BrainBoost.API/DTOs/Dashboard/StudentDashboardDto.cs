namespace BrainBoost.API.DTOs.Dashboard;

public class StudentDashboardDto
{
    public int CompletedQuizzes { get; set; }

    public double AverageScore { get; set; }

    public int FlashcardsReviewed { get; set; }

    public List<string> RecentQuizTitles { get; set; }
        = new();
}