namespace BrainBoost.API.DTOs.Categories;

public class CreateCategoryDto
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? CoverImageUrl { get; set; }
}