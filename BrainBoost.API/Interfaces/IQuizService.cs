using BrainBoost.API.DTOs.Common;
using BrainBoost.API.DTOs.Quizzes;

namespace BrainBoost.API.Interfaces;

public interface IQuizService
{
    Task<PagedResultDto<QuizDto>> GetAllAsync(QuizFilterDto filter);

    Task<QuizDto> GetBySlugAsync(string slug);

    Task<QuizDto> CreateAsync(CreateQuizDto dto, int userId);

    Task<QuizDto> UpdateAsync(int id, UpdateQuizDto dto);

    Task DeleteAsync(int id);
}