using InstallFlow.Data;
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CategoryRepo : ICategoryRepo
{
    private readonly InstallFlowDbContext _context;
    public CategoryRepo(InstallFlowDbContext context) => _context = context;

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.UrlSlug == slug);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);

    }

    public async Task RemoveProductAsync(int categoryId, int productId)
    {
        var link = await _context.ProductCategories
            .FirstOrDefaultAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);

        if (link == null) return;
        _context.ProductCategories.Remove(link);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return;
        _context.Categories.Remove(category);

    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}