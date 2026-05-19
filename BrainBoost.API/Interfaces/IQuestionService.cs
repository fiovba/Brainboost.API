using BrainBoost.API.DTOs.Questions;

namespace BrainBoost.API.Interfaces;

public interface IQuestionService
{
    Task<List<QuestionDto>> GetByQuizIdAsync(int quizId);

    Task<QuestionDto> CreateAsync(
        int quizId,
        CreateQuestionDto dto
    );

    Task DeleteAsync(int id);
}