using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Questions;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _context;

    public QuestionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionDto>> GetByQuizIdAsync(int quizId)
    {
        return await _context.Questions
            .Include(x => x.Options)
            .Where(x => x.QuizId == quizId)
            .Select(x => new QuestionDto
            {
                Id = x.Id,
                QuestionText = x.QuestionText,
                QuestionType = x.QuestionType,
                Explanation = x.Explanation,
                Points = x.Points,
                OrderIndex = x.OrderIndex,

                Options = x.Options
                    .Select(o => new AnswerOptionDto
                    {
                        Id = o.Id,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect,
                        OrderIndex = o.OrderIndex
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<QuestionDto> CreateAsync(
        int quizId,
        CreateQuestionDto dto
    )
    {
        var quizExists = await _context.Quizzes
            .AnyAsync(x => x.Id == quizId);

        if (!quizExists)
            throw new Exception("Quiz tapılmadı.");

        var question = new Question
        {
            QuizId = quizId,
            QuestionText = dto.QuestionText,
            QuestionType = dto.QuestionType,
            Explanation = dto.Explanation,
            Points = dto.Points,
            OrderIndex = dto.OrderIndex
        };

        foreach (var optionDto in dto.Options)
        {
            question.Options.Add(new AnswerOption
            {
                OptionText = optionDto.OptionText,
                IsCorrect = optionDto.IsCorrect,
                OrderIndex = optionDto.OrderIndex
            });
        }

        _context.Questions.Add(question);

        await _context.SaveChangesAsync();

        return new QuestionDto
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            Explanation = question.Explanation,
            Points = question.Points,
            OrderIndex = question.OrderIndex,

            Options = question.Options
                .Select(x => new AnswerOptionDto
                {
                    Id = x.Id,
                    OptionText = x.OptionText,
                    IsCorrect = x.IsCorrect,
                    OrderIndex = x.OrderIndex
                })
                .ToList()
        };
    }

    public async Task DeleteAsync(int id)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (question == null)
            throw new Exception("Question tapılmadı.");

        _context.Questions.Remove(question);

        await _context.SaveChangesAsync();
    }
}