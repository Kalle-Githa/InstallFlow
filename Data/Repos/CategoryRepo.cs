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
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(c => c.UrlSlug == slug);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return;
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}