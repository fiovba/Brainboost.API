using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Categories;
using BrainBoost.API.Entities;
using BrainBoost.API.Helpers;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                IconUrl = x.IconUrl,
                ImageUrl = x.ImageUrl,
                CoverImageUrl = x.CoverImageUrl,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<CategoryDto> GetBySlugAsync(string slug)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Slug == slug);

        if (category == null)
            throw new Exception("Category tapılmadı.");

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IconUrl = category.IconUrl,
            ImageUrl = category.ImageUrl,
            CoverImageUrl = category.CoverImageUrl,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var slug = SlugHelper.Generate(dto.Name);

        var exists = await _context.Categories
            .AnyAsync(x => x.Name == dto.Name);

        if (exists)
            throw new Exception("Bu category artıq mövcuddur.");

        var category = new Category
        {
            Name = dto.Name,
            Slug = slug,
            Description = dto.Description,
            IconUrl = dto.IconUrl,
            ImageUrl = dto.ImageUrl,
            CoverImageUrl = dto.CoverImageUrl
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IconUrl = category.IconUrl,
            ImageUrl = category.ImageUrl,
            CoverImageUrl = category.CoverImageUrl,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            throw new Exception("Category tapılmadı.");

        category.Name = dto.Name;
        category.Slug = SlugHelper.Generate(dto.Name);
        category.Description = dto.Description;
        category.IconUrl = dto.IconUrl;
        category.ImageUrl = dto.ImageUrl;
        category.CoverImageUrl = dto.CoverImageUrl;
        category.IsActive = dto.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IconUrl = category.IconUrl,
            ImageUrl = category.ImageUrl,
            CoverImageUrl = category.CoverImageUrl,
            IsActive = category.IsActive
        };
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            throw new Exception("Category tapılmadı.");

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();
    }
}