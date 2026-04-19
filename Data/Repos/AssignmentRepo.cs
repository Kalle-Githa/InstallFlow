
using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data.Repos;

public class AssignmentRepo : IAssignmentRepo
{
    private readonly InstallFlowDbContext _context;

    public AssignmentRepo(InstallFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Assignment>> GetAllAsync()
    {
        return await _context.Assignments
            .Include(a => a.Customer)      // ← hämta kunden med, för CustomerName
            .ToListAsync();
    }
    
    public async Task<List<Assignment>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Assignments
            .Include(a => a.Customer)
            .Where(a => a.CreatedByUserId == userId)
            .ToListAsync();
    }

    public async Task<Assignment?> GetByIdAsync(int id)
    {
        return await _context.Assignments
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Assignment> CreateAsync(Assignment assignment)
    {
        await _context.Assignments.AddAsync(assignment);
        return assignment;
    }

    public async Task DeleteAsync(int id)
    {
        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null) return;
        _context.Assignments.Remove(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}