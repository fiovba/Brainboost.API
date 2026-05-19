using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Common;
using BrainBoost.API.DTOs.Quizzes;
using BrainBoost.API.Entities;
using BrainBoost.API.Helpers;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class QuizService : IQuizService
{
    private readonly AppDbContext _context;

    public QuizService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<QuizDto>> GetAllAsync(QuizFilterDto filter)
    {
        var query = _context.Quizzes
            .Include(x => x.Category)
            .Include(x => x.CreatedByUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.Title.Contains(filter.Search) ||
                (x.Description != null &&
                 x.Description.Contains(filter.Search)));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(x =>
                x.CategoryId == filter.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Difficulty))
        {
            query = query.Where(x =>
                x.Difficulty == filter.Difficulty);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new QuizDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                Description = x.Description,
                Difficulty = x.Difficulty,
                TimeLimitMinutes = x.TimeLimitMinutes,
                PassingScore = x.PassingScore,
                IsPublished = x.IsPublished,
                IsApproved = x.IsApproved,
                CategoryName = x.Category.Name,
                TeacherName = x.CreatedByUser.FullName
            })
            .ToListAsync();

        return new PagedResultDto<QuizDto>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)filter.PageSize
            )
        };
    }

    public async Task<QuizDto> GetBySlugAsync(string slug)
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Category)
            .Include(x => x.CreatedByUser)
            .FirstOrDefaultAsync(x => x.Slug == slug);

        if (quiz == null)
            throw new Exception("Quiz tapılmadı.");

        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Slug = quiz.Slug,
            Description = quiz.Description,
            Difficulty = quiz.Difficulty,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            PassingScore = quiz.PassingScore,
            IsPublished = quiz.IsPublished,
            IsApproved = quiz.IsApproved,
            CategoryName = quiz.Category.Name,
            TeacherName = quiz.CreatedByUser.FullName
        };
    }

    public async Task<QuizDto> CreateAsync(
        CreateQuizDto dto,
        int userId
    )
    {
        var categoryExists = await _context.Categories
            .AnyAsync(x => x.Id == dto.CategoryId);

        if (!categoryExists)
            throw new Exception("Category tapılmadı.");

        var quiz = new Quiz
        {
            Title = dto.Title,
            Slug = SlugHelper.Generate(dto.Title),
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Difficulty = dto.Difficulty,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            PassingScore = dto.PassingScore,
            CreatedByUserId = userId
        };

        _context.Quizzes.Add(quiz);

        await _context.SaveChangesAsync();

        await _context.Entry(quiz)
            .Reference(x => x.CreatedByUser)
            .LoadAsync();

        var category = await _context.Categories
            .FirstAsync(x => x.Id == dto.CategoryId);

        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Slug = quiz.Slug,
            Description = quiz.Description,
            Difficulty = quiz.Difficulty,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            PassingScore = quiz.PassingScore,
            IsPublished = quiz.IsPublished,
            IsApproved = quiz.IsApproved,
            CategoryName = category.Name,
            TeacherName = quiz.CreatedByUser.FullName
        };
    }

    public async Task<QuizDto> UpdateAsync(
        int id,
        UpdateQuizDto dto
    )
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Category)
            .Include(x => x.CreatedByUser)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (quiz == null)
            throw new Exception("Quiz tapılmadı.");

        quiz.Title = dto.Title;
        quiz.Slug = SlugHelper.Generate(dto.Title);
        quiz.Description = dto.Description;
        quiz.CategoryId = dto.CategoryId;
        quiz.Difficulty = dto.Difficulty;
        quiz.TimeLimitMinutes = dto.TimeLimitMinutes;
        quiz.PassingScore = dto.PassingScore;
        quiz.IsPublished = dto.IsPublished;
        quiz.IsApproved = dto.IsApproved;
        quiz.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Slug = quiz.Slug,
            Description = quiz.Description,
            Difficulty = quiz.Difficulty,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            PassingScore = quiz.PassingScore,
            IsPublished = quiz.IsPublished,
            IsApproved = quiz.IsApproved,
            CategoryName = quiz.Category.Name,
            TeacherName = quiz.CreatedByUser.FullName
        };
    }

    public async Task DeleteAsync(int id)
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (quiz == null)
            throw new Exception("Quiz tapılmadı.");

        var questionIds = quiz.Questions
            .Select(q => q.Id)
            .ToList();

        var optionIds = quiz.Questions
            .SelectMany(q => q.Options)
            .Select(o => o.Id)
            .ToList();

        var attemptIds = await _context.QuizAttempts
            .Where(x => x.QuizId == id)
            .Select(x => x.Id)
            .ToListAsync();

        var userAnswers = await _context.UserAnswers
            .Where(x =>
                attemptIds.Contains(x.AttemptId) ||
                questionIds.Contains(x.QuestionId) ||
                optionIds.Contains(x.SelectedOptionId)
            )
            .ToListAsync();

        _context.UserAnswers.RemoveRange(userAnswers);

        var attempts = await _context.QuizAttempts
            .Where(x => x.QuizId == id)
            .ToListAsync();

        _context.QuizAttempts.RemoveRange(attempts);

        var bookmarks = await _context.Bookmarks
            .Where(x => x.QuizId == id)
            .ToListAsync();

        _context.Bookmarks.RemoveRange(bookmarks);

        foreach (var question in quiz.Questions)
        {
            _context.AnswerOptions.RemoveRange(question.Options);
        }

        _context.Questions.RemoveRange(quiz.Questions);

        _context.Quizzes.Remove(quiz);

        await _context.SaveChangesAsync();
    }
}