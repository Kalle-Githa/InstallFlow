using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetAllAssignments()
    {
        var assignments = await _assignmentService.GetAllAssignmentsAsync();
        return Ok(assignments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssignment(int id)
    {
        var assignment = await _assignmentService.GetAssignmentAsync(id);
        if (assignment == null) return NotFound();
        return Ok(assignment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment(CreateAssignmentDto dto)
    {
        var assignment = await _assignmentService.CreateAssignmentAsync(dto);
        if (assignment == null)
            return BadRequest(new { error = "Customer not found." });

        return CreatedAtAction(
            nameof(GetAssignment),
            new { id = assignment.Id },
            assignment
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAssignment(UpdateAssignmentDto dto, int id)
    {
        var assignment = await _assignmentService.UpdateAssignmentAsync(dto, id);
        if (assignment == null) return NotFound();
        return Ok(assignment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var deleted = await _assignmentService.DeleteAssignmentAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}