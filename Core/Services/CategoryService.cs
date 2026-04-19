using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    public CategoryService(ICategoryRepo categoryRepo) => _categoryRepo = categoryRepo;

    // Privat hjälpmetod — återanvänds av alla GET-metoder
    private CategoryDto MapToDto(Category category) => new CategoryDto
    {
        Id = category.Id,
        Name = category.Name,
        Image = category.Image,
        UrlSlug = category.UrlSlug,
        Products = category.ProductCategories.Select(pc => new ProductDto
        {
            Id = pc.Product.Id,
            Name = pc.Product.Name,
            Description = pc.Product.Description,
            DefaultPrice = pc.Product.DefaultPrice,
            Unit = pc.Product.Unit,
            Image = pc.Product.Image,
            UrlSlug = pc.Product.UrlSlug
        }).ToList()
    };

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) return null;
        return MapToDto(category);
    }

    public async Task<List<CategoryDto>> GetCategoryBySlugAsync(string slug)
    {
        var category = await _categoryRepo.GetBySlugAsync(slug);
        if (category == null) return new List<CategoryDto>();
        return new List<CategoryDto> { MapToDto(category) };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Image = dto.Image,
            UrlSlug = dto.Name.ToLower().Replace(" ", "-"),
            CreatedAt = DateTime.UtcNow
        };
        await _categoryRepo.CreateAsync(category);
        return MapToDto(category);
    }



    public async Task<CategoryDto> UpdateCategoryAsync(JsonPatchDocument<UpdateCategoryDto> patchDoc, int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Kategorin hittades inte.");

        var dto = new UpdateCategoryDto
        {
            Name = category.Name,
            Image = category.Image

        };
        
        patchDoc.ApplyTo(dto);

        if (dto.Name != null) category.Name = dto.Name;
        if (dto.Image != null) category.Image = dto.Image;
        category.UrlSlug = category.Name.ToLower().Replace(" ", "-");
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepo.UpdateAsync(category);
        var updatedCategory = await _categoryRepo.GetByIdAsync(id);
        return MapToDto(updatedCategory!);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Kategorin hittades inte.");
        await _categoryRepo.DeleteAsync(id);
    }
}