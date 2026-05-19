using BrainBoost.API.DTOs.Attempts;

namespace BrainBoost.API.Interfaces;

public interface IAttemptService
{
    Task<AttemptDto> StartAsync(int quizId, int userId);

    Task<AttemptResultDto> SubmitAsync(
        int attemptId,
        int userId,
        SubmitAttemptDto dto
    );

    Task<List<AttemptDto>> GetMyAttemptsAsync(int userId);
    Task<AttemptReviewDto> GetReviewAsync(int attemptId, int userId);
}