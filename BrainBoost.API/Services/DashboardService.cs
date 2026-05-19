using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Dashboard;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<TeacherDashboardDto> GetTeacherDashboardAsync(int userId)
    {
        var quizzes = await _context.Quizzes
            .Include(x => x.Questions)
            .Where(x => x.CreatedByUserId == userId)
            .ToListAsync();

        var quizIds = quizzes.Select(x => x.Id).ToList();

        var attempts = await _context.QuizAttempts
            .Include(x => x.Quiz)
            .Include(x => x.User)
            .Where(x =>
                quizIds.Contains(x.QuizId) &&
                x.Status == "Completed")
            .ToListAsync();

        return new TeacherDashboardDto
        {
            CreatedQuizzesCount = quizzes.Count,

            TotalQuestionsCount = quizzes.Sum(x => x.Questions.Count),

            TotalStudentAttempts = attempts.Count,

            AverageQuizScore = attempts.Count == 0
                ? 0
                : Math.Round(
                    attempts.Average(x =>
                        x.TotalPoints == 0
                            ? 0
                            : ((double)x.Score / x.TotalPoints) * 100),
                    2),

            PublishedQuizzesCount = quizzes.Count(x => x.IsPublished),

            DraftQuizzesCount = quizzes.Count(x => !x.IsPublished),

            RecentQuizzes = quizzes
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new TeacherRecentQuizDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Difficulty = x.Difficulty,
                    QuestionCount = x.Questions.Count,
                    IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                })
                .ToList(),

            RecentAttempts = attempts
                .OrderByDescending(x => x.SubmittedAt)
                .Take(5)
                .Select(x => new TeacherRecentAttemptDto
                {
                    Id = x.Id,
                    StudentName = x.User.FullName,
                    QuizTitle = x.Quiz.Title,
                    ScorePercent = x.TotalPoints == 0
                        ? 0
                        : Math.Round(((double)x.Score / x.TotalPoints) * 100, 2),
                    SubmittedAt = x.SubmittedAt
                })
                .ToList(),

            TopPerformingQuizzes = attempts
                .GroupBy(x => x.Quiz.Title)
                .OrderByDescending(g =>
                    g.Average(x =>
                        x.TotalPoints == 0
                            ? 0
                            : ((double)x.Score / x.TotalPoints) * 100))
                .Take(5)
                .Select(g => g.Key)
                .ToList()
        };
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync()
    {
        return new AdminDashboardDto
        {
            TotalUsers = await _context.Users.CountAsync(),

            TotalStudents = await _context.Users
                .Include(x => x.Role)
                .CountAsync(x => x.Role.Name == "Student"),

            TotalTeachers = await _context.Users
                .Include(x => x.Role)
                .CountAsync(x => x.Role.Name == "Teacher"),

            TotalQuizzes = await _context.Quizzes.CountAsync(),

            TotalFlashcards = await _context.Flashcards.CountAsync(),

            TotalAttempts = await _context.QuizAttempts.CountAsync(),

            PendingQuizzes = await _context.Quizzes
                .CountAsync(x => x.IsApproved == false)
        };
    }
    public async Task<StudentDashboardDto>
        GetStudentDashboardAsync(int userId)
    {
        var attempts = await _context.QuizAttempts
            .Include(x => x.Quiz)
            .Where(x =>
                x.UserId == userId &&
                x.Status == "Completed"
            )
            .ToListAsync();

        var flashcardReviews =
            await _context.FlashcardProgresses
                .CountAsync(x => x.UserId == userId);

        return new StudentDashboardDto
        {
            CompletedQuizzes = attempts.Count,

            AverageScore = attempts.Count == 0
                ? 0
                : Math.Round(
                    attempts.Average(x =>
                        x.TotalPoints == 0
                            ? 0
                            : ((double)x.Score /
                               x.TotalPoints) * 100
                    ),
                    2
                ),

            FlashcardsReviewed = flashcardReviews,

            RecentQuizTitles = attempts
                .OrderByDescending(x => x.SubmittedAt)
                .Take(5)
                .Select(x => x.Quiz.Title)
                .ToList()
        };

    }
}