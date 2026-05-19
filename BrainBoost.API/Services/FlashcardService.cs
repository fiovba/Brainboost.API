using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Common;
using BrainBoost.API.DTOs.Flashcards;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class FlashcardService : IFlashcardService
{
    private readonly AppDbContext _context;

    public FlashcardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<FlashcardDto>> GetAllAsync(FlashcardFilterDto filter)
    {
        var query = _context.Flashcards
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.Title.Contains(filter.Search) ||
                x.FrontText.Contains(filter.Search) ||
                x.BackText.Contains(filter.Search));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Difficulty))
        {
            query = query.Where(x => x.Difficulty == filter.Difficulty);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new FlashcardDto
            {
                Id = x.Id,
                Title = x.Title,
                FrontText = x.FrontText,
                BackText = x.BackText,
                CategoryName = x.Category.Name,
                Difficulty = x.Difficulty,
                ImageUrl = x.ImageUrl,
                IsPublished = x.IsPublished
            })
            .ToListAsync();

        return new PagedResultDto<FlashcardDto>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        };
    }

    public async Task<FlashcardDto> GetByIdAsync(int id)
    {
        var flashcard = await _context.Flashcards
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (flashcard == null)
            throw new Exception("Flashcard tapılmadı.");

        return new FlashcardDto
        {
            Id = flashcard.Id,
            Title = flashcard.Title,
            FrontText = flashcard.FrontText,
            BackText = flashcard.BackText,
            CategoryName = flashcard.Category.Name,
            Difficulty = flashcard.Difficulty,
            ImageUrl = flashcard.ImageUrl,
            IsPublished = flashcard.IsPublished
        };
    }

    public async Task<FlashcardDto> CreateAsync(CreateFlashcardDto dto, int userId)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(x => x.Id == dto.CategoryId);

        if (!categoryExists)
            throw new Exception("Category tapılmadı.");

        var flashcard = new Flashcard
        {
            Title = dto.Title,
            FrontText = dto.FrontText,
            BackText = dto.BackText,
            CategoryId = dto.CategoryId,
            Difficulty = dto.Difficulty,
            ImageUrl = dto.ImageUrl,
            CreatedByUserId = userId,
            IsPublished = true
        };

        _context.Flashcards.Add(flashcard);
        await _context.SaveChangesAsync();

        var category = await _context.Categories
            .FirstAsync(x => x.Id == dto.CategoryId);

        return new FlashcardDto
        {
            Id = flashcard.Id,
            Title = flashcard.Title,
            FrontText = flashcard.FrontText,
            BackText = flashcard.BackText,
            CategoryName = category.Name,
            Difficulty = flashcard.Difficulty,
            ImageUrl = flashcard.ImageUrl,
            IsPublished = flashcard.IsPublished
        };
    }

    public async Task<FlashcardDto> UpdateAsync(int id, UpdateFlashcardDto dto)
    {
        var flashcard = await _context.Flashcards
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (flashcard == null)
            throw new Exception("Flashcard tapılmadı.");

        flashcard.Title = dto.Title;
        flashcard.FrontText = dto.FrontText;
        flashcard.BackText = dto.BackText;
        flashcard.CategoryId = dto.CategoryId;
        flashcard.Difficulty = dto.Difficulty;
        flashcard.ImageUrl = dto.ImageUrl;
        flashcard.IsPublished = dto.IsPublished;
        flashcard.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var category = await _context.Categories
            .FirstAsync(x => x.Id == dto.CategoryId);

        return new FlashcardDto
        {
            Id = flashcard.Id,
            Title = flashcard.Title,
            FrontText = flashcard.FrontText,
            BackText = flashcard.BackText,
            CategoryName = category.Name,
            Difficulty = flashcard.Difficulty,
            ImageUrl = flashcard.ImageUrl,
            IsPublished = flashcard.IsPublished
        };
    }

    public async Task DeleteAsync(int id)
    {
        var flashcard = await _context.Flashcards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (flashcard == null)
            throw new Exception("Flashcard tapılmadı.");

        _context.Flashcards.Remove(flashcard);
        await _context.SaveChangesAsync();
    }

    public async Task MarkKnownAsync(int flashcardId, int userId)
    {
        var flashcardExists = await _context.Flashcards
            .AnyAsync(x => x.Id == flashcardId);

        if (!flashcardExists)
            throw new Exception("Flashcard tapılmadı.");

        var progress = await _context.FlashcardProgresses
            .FirstOrDefaultAsync(x =>
                x.FlashcardId == flashcardId &&
                x.UserId == userId);

        if (progress == null)
        {
            progress = new FlashcardProgress
            {
                FlashcardId = flashcardId,
                UserId = userId,
                KnownLevel = 1,
                ReviewCount = 1,
                LastReviewedAt = DateTime.UtcNow,
                NextReviewAt = DateTime.UtcNow.AddDays(1)
            };

            _context.FlashcardProgresses.Add(progress);
        }
        else
        {
            progress.KnownLevel++;
            progress.ReviewCount++;
            progress.LastReviewedAt = DateTime.UtcNow;
            progress.NextReviewAt = DateTime.UtcNow.AddDays(progress.KnownLevel);
        }

        await _context.SaveChangesAsync();
    }

    public async Task MarkUnknownAsync(int flashcardId, int userId)
    {
        var flashcardExists = await _context.Flashcards
            .AnyAsync(x => x.Id == flashcardId);

        if (!flashcardExists)
            throw new Exception("Flashcard tapılmadı.");

        var progress = await _context.FlashcardProgresses
            .FirstOrDefaultAsync(x =>
                x.FlashcardId == flashcardId &&
                x.UserId == userId);

        if (progress == null)
        {
            progress = new FlashcardProgress
            {
                FlashcardId = flashcardId,
                UserId = userId,
                KnownLevel = 0,
                ReviewCount = 1,
                LastReviewedAt = DateTime.UtcNow,
                NextReviewAt = DateTime.UtcNow.AddHours(6)
            };

            _context.FlashcardProgresses.Add(progress);
        }
        else
        {
            progress.KnownLevel = 0;
            progress.ReviewCount++;
            progress.LastReviewedAt = DateTime.UtcNow;
            progress.NextReviewAt = DateTime.UtcNow.AddHours(6);
        }

        await _context.SaveChangesAsync();
    }
}