using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Bookmarks;
using BrainBoost.API.Entities;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class BookmarkService : IBookmarkService
{
    private readonly AppDbContext _context;

    public BookmarkService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddQuizBookmarkAsync(int quizId, int userId)
    {
        var quizExists = await _context.Quizzes.AnyAsync(x => x.Id == quizId);

        if (!quizExists)
            throw new Exception("Quiz tapılmadı.");

        var exists = await _context.Bookmarks.AnyAsync(x =>
            x.UserId == userId && x.QuizId == quizId);

        if (exists)
            throw new Exception("Bu quiz artıq bookmark edilib.");

        var bookmark = new Bookmark
        {
            UserId = userId,
            QuizId = quizId
        };

        _context.Bookmarks.Add(bookmark);
        await _context.SaveChangesAsync();
    }

    public async Task AddFlashcardBookmarkAsync(int flashcardId, int userId)
    {
        var flashcardExists = await _context.Flashcards.AnyAsync(x => x.Id == flashcardId);

        if (!flashcardExists)
            throw new Exception("Flashcard tapılmadı.");

        var exists = await _context.Bookmarks.AnyAsync(x =>
            x.UserId == userId && x.FlashcardId == flashcardId);

        if (exists)
            throw new Exception("Bu flashcard artıq bookmark edilib.");

        var bookmark = new Bookmark
        {
            UserId = userId,
            FlashcardId = flashcardId
        };

        _context.Bookmarks.Add(bookmark);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BookmarkDto>> GetMyBookmarksAsync(int userId)
    {
        return await _context.Bookmarks
            .Include(x => x.Quiz)
            .Include(x => x.Flashcard)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BookmarkDto
            {
                Id = x.Id,
                Type = x.QuizId != null ? "Quiz" : "Flashcard",
                ItemId = x.QuizId ?? x.FlashcardId!.Value,
                Title = x.QuizId != null ? x.Quiz!.Title : x.Flashcard!.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var bookmark = await _context.Bookmarks
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (bookmark == null)
            throw new Exception("Bookmark tapılmadı.");

        _context.Bookmarks.Remove(bookmark);
        await _context.SaveChangesAsync();
    }
}