using BrainBoost.API.Data;
using BrainBoost.API.DTOs.FlashcardSets;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class FlashcardSetService : IFlashcardSetService
{
    private readonly AppDbContext _context;

    public FlashcardSetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FlashcardSetDto>> GetAllAsync()
    {
        return await _context.FlashcardSets
            .Include(x => x.Category)
            .Include(x => x.Flashcards)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new FlashcardSetDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                Difficulty = x.Difficulty,
                CardCount = x.Flashcards.Count,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<FlashcardSetDto?> GetByIdAsync(int id)
    {
        var set = await _context.FlashcardSets
            .Include(x => x.Category)
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (set == null)
            return null;

        return new FlashcardSetDto
        {
            Id = set.Id,
            Title = set.Title,
            Description = set.Description,
            CategoryId = set.CategoryId,
            CategoryName = set.Category.Name,
            Difficulty = set.Difficulty,
            CardCount = set.Flashcards.Count,
            CreatedAt = set.CreatedAt,

            Flashcards = set.Flashcards.Select(card =>
     new FlashcardInSetDto
     {
         Id = card.Id,
         FrontText = card.FrontText,
         BackText = card.BackText,
         ImageUrl = card.ImageUrl
     }
).ToList()
        };
    }

    public async Task<FlashcardSetDto> CreateAsync(
        CreateFlashcardSetDto dto,
        int userId
    )
    {
        var set = new FlashcardSet
        {
            Title = dto.Title,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Difficulty = dto.Difficulty,
            CreatedByUserId = userId
        };

        _context.FlashcardSets.Add(set);

        await _context.SaveChangesAsync();

        var flashcards = dto.Flashcards.Select((card, index) =>
      new Flashcard
      {
          Title = $"{dto.Title} - Card {index + 1}",
          FrontText = card.FrontText,
          BackText = card.BackText,
          ImageUrl = card.ImageUrl,
          CategoryId = dto.CategoryId,
          Difficulty = dto.Difficulty,
          FlashcardSetId = set.Id,
          CreatedByUserId = userId,
      }
  ).ToList();

        _context.Flashcards.AddRange(flashcards);

        await _context.SaveChangesAsync();

        return await GetByIdAsync(set.Id)
            ?? throw new Exception("Set tapılmadı.");
    }
    public async Task<FlashcardSetDto> UpdateAsync(
    int id,
    UpdateFlashcardSetDto dto,
    int userId
)
    {
        var set = await _context.FlashcardSets
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.CreatedByUserId == userId
            );

        if (set == null)
            throw new Exception("Flashcard set tapılmadı.");

        set.Title = dto.Title;
        set.Description = dto.Description;
        set.CategoryId = dto.CategoryId;
        set.Difficulty = dto.Difficulty;

        var incomingIds = dto.Flashcards
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToList();

        var cardsToRemove = set.Flashcards
            .Where(x => !incomingIds.Contains(x.Id))
            .ToList();

        _context.Flashcards.RemoveRange(cardsToRemove);

        foreach (var cardDto in dto.Flashcards)
        {
            if (cardDto.Id.HasValue)
            {
                var existingCard = set.Flashcards
                    .FirstOrDefault(x => x.Id == cardDto.Id.Value);

                if (existingCard == null)
                    continue;

                existingCard.Title = $"{dto.Title} - Card {set.Flashcards.IndexOf(existingCard) + 1}";
                existingCard.FrontText = cardDto.FrontText;
                existingCard.BackText = cardDto.BackText;
                existingCard.ImageUrl = cardDto.ImageUrl;
                existingCard.CategoryId = dto.CategoryId;
                existingCard.Difficulty = dto.Difficulty;
            }
            else
            {
                var newCardIndex = set.Flashcards.Count + 1;

                var newCard = new Flashcard
                {
                    Title = $"{dto.Title} - Card {newCardIndex}",
                    FrontText = cardDto.FrontText,
                    BackText = cardDto.BackText,
                    ImageUrl = cardDto.ImageUrl,
                    CategoryId = dto.CategoryId,
                    Difficulty = dto.Difficulty,
                    FlashcardSetId = set.Id,
                    CreatedByUserId = userId,
                };

                _context.Flashcards.Add(newCard);
            }
        }

        await _context.SaveChangesAsync();

        return await GetByIdAsync(set.Id)
            ?? throw new Exception("Flashcard set tapılmadı.");
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var set = await _context.FlashcardSets
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.CreatedByUserId == userId
            );

        if (set == null)
            throw new Exception("Flashcard set tapılmadı.");

        _context.FlashcardSets.Remove(set);

        await _context.SaveChangesAsync();
    }
}