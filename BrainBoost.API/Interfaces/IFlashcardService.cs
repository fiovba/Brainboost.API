using BrainBoost.API.DTOs.Flashcards;
using BrainBoost.API.DTOs.Common;
using BrainBoost.API.DTOs.Flashcards;

namespace BrainBoost.API.Interfaces;

public interface IFlashcardService
{
    Task<PagedResultDto<FlashcardDto>> GetAllAsync(FlashcardFilterDto filter);

    Task<FlashcardDto> GetByIdAsync(int id);

    Task<FlashcardDto> CreateAsync(CreateFlashcardDto dto, int userId);

    Task<FlashcardDto> UpdateAsync(int id, UpdateFlashcardDto dto);

    Task DeleteAsync(int id);

    Task MarkKnownAsync(int flashcardId, int userId);

    Task MarkUnknownAsync(int flashcardId, int userId);
}