using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[ApiController]
[Route("/api/applications")]
public class JobApplicationsController(IJobApplicationsService applications) : ControllerBase
{
    [ProducesResponseType(typeof(ICollection<JobApplicationGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ICollection<JobApplicationGetDto>), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetApplications(CancellationToken cancellation, [FromQuery] int postingId)
    {
        return Ok(await applications.GetAllForPosting(cancellation, postingId));
    }
    
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await applications.GetById(cancellation, id));
    }
    
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> RejectApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await applications.Reject(cancellation, id));
    }
    
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> AcceptApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await applications.Accept(cancellation, id));
    }
}