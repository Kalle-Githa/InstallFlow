using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data.Repos;

public class JobRepo : IJobRepo
{
    private readonly InstallFlowDbContext _context;

    public JobRepo(InstallFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetAllAsync(int? assignmentId = null)
    {
        var query = _context.Jobs
            .AsNoTracking()
            .Include(j => j.Assignment)
            .Include(j => j.LaborRows)
            .Include(j => j.MaterialRows)
            .AsSplitQuery()
            .AsQueryable();

        if (assignmentId.HasValue)
        {
            query = query.Where(j => j.AssignmentId == assignmentId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _context.Jobs
            .Include(j => j.Assignment)
            .Include(j => j.LaborRows)
            .Include(j => j.MaterialRows)
            .AsSplitQuery()
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<Job> CreateAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
        return job;
    }

    public async Task DeleteAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return;
        _context.Jobs.Remove(job);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}