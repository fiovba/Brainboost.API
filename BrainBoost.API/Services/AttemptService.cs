using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Attempts;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class AttemptService : IAttemptService
{
    private readonly AppDbContext _context;

    public AttemptService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<AttemptReviewDto> GetReviewAsync(int attemptId, int userId)
    {
        var attempt = await _context.QuizAttempts
            .Include(x => x.Quiz)
            .Include(x => x.Answers)
                .ThenInclude(a => a.Question)
            .Include(x => x.Answers)
                .ThenInclude(a => a.SelectedOption)
            .FirstOrDefaultAsync(x =>
                x.Id == attemptId &&
                x.UserId == userId);

        if (attempt == null)
            throw new Exception("Attempt tapılmadı.");

        if (attempt.Status != "Completed")
            throw new Exception("Attempt hələ tamamlanmayıb.");

        var reviews = new List<QuestionReviewDto>();

        foreach (var answer in attempt.Answers)
        {
            var correctOption = await _context.AnswerOptions
                .FirstOrDefaultAsync(x =>
                    x.QuestionId == answer.QuestionId &&
                    x.IsCorrect);

            reviews.Add(new QuestionReviewDto
            {
                QuestionId = answer.QuestionId,
                QuestionText = answer.Question.QuestionText,
                Explanation = answer.Question.Explanation,
                SelectedAnswer = answer.SelectedOption.OptionText,
                CorrectAnswer = correctOption?.OptionText ?? "Correct answer tapılmadı",
                IsCorrect = answer.IsCorrect,
                PointsEarned = answer.PointsEarned,
                QuestionPoints = answer.Question.Points
            });
        }

        var percentage = attempt.TotalPoints == 0
            ? 0
            : ((double)attempt.Score / attempt.TotalPoints) * 100;

        return new AttemptReviewDto
        {
            AttemptId = attempt.Id,
            QuizTitle = attempt.Quiz.Title,
            Score = attempt.Score,
            TotalPoints = attempt.TotalPoints,
            Percentage = Math.Round(percentage, 2),
            Questions = reviews
        };
    }
    public async Task<AttemptDto> StartAsync(int quizId, int userId)
    {
        var quiz = await _context.Quizzes
            .FirstOrDefaultAsync(x => x.Id == quizId);

        if (quiz == null)
            throw new Exception("Quiz tapılmadı.");

        var alreadyCompleted = await _context.QuizAttempts
            .AnyAsync(x =>
                x.QuizId == quizId &&
                x.UserId == userId &&
                x.Status == "Completed"
            );

        if (alreadyCompleted)
            throw new Exception("Bu quiz artıq tamamlanıb.");

        var attempt = new QuizAttempt
        {
            QuizId = quizId,
            UserId = userId,
            Status = "InProgress"
        };

        _context.QuizAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return new AttemptDto
        {
            Id = attempt.Id,
            QuizTitle = quiz.Title,
            Score = 0,
            TotalPoints = 0,
            Status = attempt.Status,
            StartedAt = attempt.StartedAt,
            SubmittedAt = null
        };
    }
    public async Task<AttemptResultDto> SubmitAsync(
        int attemptId,
        int userId,
        SubmitAttemptDto dto
    )
    {
        var attempt = await _context.QuizAttempts
            .Include(x => x.Quiz)
            .FirstOrDefaultAsync(x =>
                x.Id == attemptId &&
                x.UserId == userId
            );

        if (attempt == null)
            throw new Exception("Attempt tapılmadı.");

        if (attempt.Status == "Completed")
            throw new Exception("Attempt artıq submit edilib.");

        int totalScore = 0;
        int totalPoints = 0;
        int correctCount = 0;
        int wrongCount = 0;

        foreach (var submittedAnswer in dto.Answers)
        {
            var question = await _context.Questions
                .Include(x => x.Options)
                .FirstOrDefaultAsync(x =>
                    x.Id == submittedAnswer.QuestionId
                );

            if (question == null)
                continue;

            totalPoints += question.Points;

            var correctOption = question.Options
                .FirstOrDefault(x => x.IsCorrect);

            bool isCorrect =
                correctOption != null &&
                correctOption.Id == submittedAnswer.SelectedOptionId;

            int earnedPoints = isCorrect
                ? question.Points
                : 0;

            if (isCorrect)
            {
                totalScore += question.Points;
                correctCount++;
            }
            else
            {
                wrongCount++;
            }

            var userAnswer = new UserAnswer
            {
                AttemptId = attempt.Id,
                QuestionId = question.Id,
                SelectedOptionId =
                    submittedAnswer.SelectedOptionId,
                IsCorrect = isCorrect,
                PointsEarned = earnedPoints
            };

            _context.UserAnswers.Add(userAnswer);
        }

        attempt.Score = totalScore;
        attempt.TotalPoints = totalPoints;
        attempt.CorrectAnswersCount = correctCount;
        attempt.WrongAnswersCount = wrongCount;
        attempt.Status = "Completed";
        attempt.SubmittedAt = DateTime.UtcNow;

        attempt.TimeSpentSeconds =
            (int)(attempt.SubmittedAt.Value - attempt.StartedAt)
            .TotalSeconds;
        var user = await _context.Users
    .FirstOrDefaultAsync(x => x.Id == userId);

        if (user != null)
        {
            var earnedXp = 20 + totalScore;

            user.XP += earnedXp;

            user.Level = CalculateLevel(user.XP);

            user.LearningStreak += 1;
        }
        await _context.SaveChangesAsync();

        double percentage = totalPoints == 0
            ? 0
            : ((double)totalScore / totalPoints) * 100;

        bool passed =
            percentage >= attempt.Quiz.PassingScore;

        return new AttemptResultDto
        {
            AttemptId = attempt.Id,
            Score = totalScore,
            TotalPoints = totalPoints,
            Percentage = Math.Round(percentage, 2),
            Passed = passed,
            CorrectAnswersCount = correctCount,
            WrongAnswersCount = wrongCount,
            TimeSpentSeconds = attempt.TimeSpentSeconds
        };
    }
    private static int CalculateLevel(int xp)
    {
        if (xp >= 1000) return 6;
        if (xp >= 700) return 5;
        if (xp >= 500) return 4;
        if (xp >= 250) return 3;
        if (xp >= 100) return 2;

        return 1;
    }
    public async Task<List<AttemptDto>> GetMyAttemptsAsync(
        int userId
    )
    {
        return await _context.QuizAttempts
            .Include(x => x.Quiz)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.StartedAt)
            .Select(x => new AttemptDto
            {
                Id = x.Id,
                QuizTitle = x.Quiz.Title,
                Score = x.Score,
                TotalPoints = x.TotalPoints,
                Status = x.Status,
                StartedAt = x.StartedAt,
                SubmittedAt = x.SubmittedAt
            })
            .ToListAsync();
    }
}