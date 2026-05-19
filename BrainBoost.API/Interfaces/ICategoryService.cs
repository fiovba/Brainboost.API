using BrainBoost.API.DTOs.Categories;

namespace BrainBoost.API.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();

    Task<CategoryDto> GetBySlugAsync(string slug);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

    Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);

    Task DeleteAsync(int id);
}