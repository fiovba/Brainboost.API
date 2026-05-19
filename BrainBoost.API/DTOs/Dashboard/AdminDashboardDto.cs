namespace BrainBoost.API.DTOs.Dashboard;

public class AdminDashboardDto
{
    public int TotalUsers { get; set; }

    public int TotalStudents { get; set; }

    public int TotalTeachers { get; set; }

    public int TotalQuizzes { get; set; }

    public int TotalFlashcards { get; set; }

    public int TotalAttempts { get; set; }

    public int PendingQuizzes { get; set; }
}