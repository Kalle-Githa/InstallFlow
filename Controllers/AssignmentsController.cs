using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Assignments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InstallFlow.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [Authorize]
    [HttpGet]

    public async Task<IActionResult> GetAllAssignments()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var assignments = await _assignmentService.GetAllByUserIdAsync(userId);
        return Ok(assignments);
    }

    [Authorize]
    [HttpGet("{id}")]

    public async Task<IActionResult> GetAssignment(int id)
    {
        var assignment = await _assignmentService.GetAssignmentAsync(id);
        if (assignment == null) return NotFound();
        return Ok(assignment);
    }

    [Authorize]
    [HttpPost]

    public async Task<IActionResult> CreateAssignment(CreateAssignmentDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var assignment = await _assignmentService.CreateAssignmentAsync(dto, userId);
        if (assignment == null)
            return BadRequest(new { error = "Customer not found." });

        return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, assignment);
    }

    [Authorize]
    [HttpPatch("{id}")]

    public async Task<IActionResult> UpdateAssignment(UpdateAssignmentDto dto, int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");
        var assignment = await _assignmentService.UpdateAssignmentAsync(dto, id, userId, isAdmin);

        return Ok(assignment);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");
        await _assignmentService.DeleteAssignmentAsync(id, userId, isAdmin);

        return NoContent();
    }
}