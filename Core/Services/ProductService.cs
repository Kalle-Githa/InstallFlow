using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;

namespace InstallFlow.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }


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
                UrlSlug = dto.Name.ToLower().Replace(" ", "-"), // enkel slug-generering
                ProductCategories = dto.Categories.Select(categoryId => new ProductCategory
                {
                    CategoryId = categoryId
                }).ToList()
            };

            await _productRepo.CreateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                DefaultPrice = product.DefaultPrice,
                Unit = product.Unit,
                Image = product.Image,
                UrlSlug = product.UrlSlug
            };
        }




        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                DefaultPrice = product.DefaultPrice,
                Unit = product.Unit,
                Image = product.Image,
                UrlSlug = product.UrlSlug
            };
        }



        public async Task UpdateProductItemAsync(UpdateProductDto dto, int id)
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
}
