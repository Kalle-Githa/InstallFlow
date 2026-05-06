using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InstallFlow.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllJobs([FromQuery] int? assignmentId = null)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");
        var jobs = await _jobService.GetAllJobsAsync(assignmentId, userId, isAdmin);
        return Ok(jobs);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJob(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");
        var job = await _jobService.GetJobAsync(id, userId, isAdmin);

        return Ok(job);
    }

    [Authorize]
    [HttpPost]

    public async Task<IActionResult> CreateJob(CreateJobDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var job = await _jobService.CreateJobAsync(dto, userId);

        return CreatedAtAction(
            nameof(GetJob),
            new { id = job.Id },
            job
        );
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateJob(UpdateJobDto dto, int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");

        var job = await _jobService.UpdateJobAsync(dto, id, userId, isAdmin);

        return Ok(job);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");
        await _jobService.DeleteJobAsync(id, userId, isAdmin);

        return NoContent();
    }
}