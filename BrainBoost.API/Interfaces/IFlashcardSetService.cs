using BrainBoost.API.DTOs.FlashcardSets;

namespace BrainBoost.API.Interfaces;

public interface IFlashcardSetService
{
    Task<List<FlashcardSetDto>> GetAllAsync();

    Task<FlashcardSetDto?> GetByIdAsync(int id);

    Task<FlashcardSetDto> CreateAsync(
        CreateFlashcardSetDto dto,
        int userId
    );
    Task<FlashcardSetDto> UpdateAsync(
    int id,
    UpdateFlashcardSetDto dto,
    int userId
);

    Task DeleteAsync(int id, int userId);
}