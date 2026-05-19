namespace BrainBoost.API.Entities;

public class MediaFile
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public long Size { get; set; }

    public int UploadedByUserId { get; set; }

    public User UploadedByUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}