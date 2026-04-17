using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;

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
        Image = product.Image,
        UrlSlug = product.UrlSlug,
        Categories = product.ProductCategories.Select(pc => new CategorySummaryDto
        {
            Id = pc.CategoryId,
            Name = pc.Category.Name,
            UrlSlug = pc.Category.UrlSlug
        }).ToList()
    };

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            DefaultPrice = dto.DefaultPrice,
            Unit = dto.Unit,
            Image = dto.Image,
            CreatedAt = DateTime.UtcNow,
            UrlSlug = dto.Name.ToLower().Replace(" ", "-"),
            ProductCategories = dto.Categories.Select(categoryId => new ProductCategory
            {
                CategoryId = categoryId
            }).ToList()
        };

        await _productRepo.CreateAsync(product);

        var created = await _productRepo.GetProductByIdAsync(product.Id);
        return MapToDto(created!);
    }

    public async Task<List<ProductDto>> GetProductBySlugAsync(string slug)
    {
        var product = await _productRepo.GetBySlugAsync(slug);
        if (product == null) return new List<ProductDto>();

        return new List<ProductDto> { MapToDto(product) };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);
        if (product == null) return null;

        return MapToDto(product);
    }

    public async Task<List<ProductDto>> GetAllProductAsync()
    {
        var products = await _productRepo.GetAllAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task UpdateProductAsync(UpdateProductDto dto, int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Produkten hittades inte.");

        if (dto.Name != null) product.Name = dto.Name;
        if (dto.Description != null) product.Description = dto.Description;
        if (dto.DefaultPrice.HasValue) product.DefaultPrice = dto.DefaultPrice.Value;
        if (dto.Image != null) product.Image = dto.Image;

        product.UpdatedAt = DateTime.UtcNow;
        await _productRepo.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetProductByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Produkten hittades inte.");

        await _productRepo.DeleteAsync(id);
    }
}