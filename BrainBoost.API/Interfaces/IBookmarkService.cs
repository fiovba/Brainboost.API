using BrainBoost.API.DTOs.Bookmarks;

namespace BrainBoost.API.Interfaces;

public interface IBookmarkService
{
    Task AddQuizBookmarkAsync(int quizId, int userId);

    Task AddFlashcardBookmarkAsync(int flashcardId, int userId);

    Task<List<BookmarkDto>> GetMyBookmarksAsync(int userId);

    Task DeleteAsync(int id, int userId);
}