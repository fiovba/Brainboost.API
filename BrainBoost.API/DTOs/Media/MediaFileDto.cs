namespace BrainBoost.API.DTOs.Media;

public class MediaFileDto
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public long Size { get; set; }
}