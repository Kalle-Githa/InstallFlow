using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Categories;
using InstallFlow.Data.DTO.Products;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
using InstallFlow.Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InstallFlow.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;

    public ProductService(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }

    private ProductDto MapToDto(Product product) => new ProductDto
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        DefaultPrice = product.DefaultPrice,
        Unit = product.Unit,
        Type = product.Type,
        Image = product.Image,
        UrlSlug = product.UrlSlug,
        Categories = product.ProductCategories.Select(pc => new CategorySummaryDto
        {
            Id = pc.CategoryId,
            Name = pc.Category.Name

        }).ToList()
    };

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, int userId)
    {

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            DefaultPrice = dto.DefaultPrice,
            Unit = dto.Unit,
            Type = dto.Type,
            Image = dto.Image,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = userId,
            UrlSlug = dto.Name.ToLower().Replace(" ", "-"),
            ProductCategories = dto.Categories.Select(categoryId => new ProductCategory
            {
                CategoryId = categoryId
            }).ToList()
        };

        await _productRepo.CreateAsync(product);
        await _productRepo.SaveChangesAsync();
        var created = await _productRepo.GetProductByIdAsync(product.Id);

        return MapToDto(created!);
    }

    public async Task<List<ProductDto>> GetProductBySlugAsync(string slug)
    {

        var product = await _productRepo.GetBySlugAsync(slug);
        if (product == null) return new List<ProductDto>();

        return new List<ProductDto> { MapToDto(product) };
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);
        if (product == null) throw new KeyNotFoundException($"Produkt med id {id} hittades inte.");


        return MapToDto(product);
    }

    public async Task<List<ProductDto>> GetAllProductAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;



        var products = await _productRepo.GetAllAsync(page, pageSize);
        return products.Select(MapToDto).ToList();
    }

    public async Task<List<ProductDto>> GetProductsByTypeAsync(ProductType type)
    {
        var products = await _productRepo.GetByTypeAsync(type);
        return products.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm, int page, int pageSize)
    {
        var products = await _productRepo.SearchAsync(searchTerm, page, pageSize);
        return products.Select(MapToDto);
    }


    public async Task<ProductDto> UpdateProductAsync(JsonPatchDocument<UpdateProductDto> patchDoc, int id, int userId, bool isAdmin)
    {
        var product = await _productRepo.GetProductByIdAsync(id)
          ?? throw new KeyNotFoundException($"Produkt med id {id} hittades inte.");

        if (product.CreatedByUserId != userId && !isAdmin)
            throw new UnauthorizedAccessException("Du får inte ändra andras produkter.");


        //  Mappa entitet → DTO
        var dto = new UpdateProductDto
        {
            Name = product.Name,
            Description = product.Description,
            DefaultPrice = product.DefaultPrice,
            Image = product.Image
        };

        // Applicera patch-operationerna på DTO:n
        patchDoc.ApplyTo(dto);

        // Mappa tillbaka DTO → entitet
        if (dto.Name != null) product.Name = dto.Name;
        if (dto.Description != null) product.Description = dto.Description;
        if (dto.DefaultPrice.HasValue) product.DefaultPrice = dto.DefaultPrice.Value;
        if (dto.Image != null) product.Image = dto.Image;

        // Uppdatera slug om namnet ändrades
        product.UrlSlug = product.Name.ToLower().Replace(" ", "-");
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepo.UpdateAsync(product);
        await _productRepo.SaveChangesAsync();

        var updated = await _productRepo.GetProductByIdAsync(id);
        return MapToDto(updated!);
    }


    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id)
            ?? throw new KeyNotFoundException($"Produkt med id {id} hittades inte.");

        await _productRepo.DeleteAsync(id);
        await _productRepo.SaveChangesAsync();
    }
}