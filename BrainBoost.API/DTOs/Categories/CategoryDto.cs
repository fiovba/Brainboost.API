namespace BrainBoost.API.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? CoverImageUrl { get; set; }

    public bool IsActive { get; set; }
}