namespace BrainBoost.API.DTOs.Bookmarks;

public class BookmarkDto
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public int ItemId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}