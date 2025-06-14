using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CandidatesController(ICandidatesService candidates) : ControllerBase
{
    [ProducesResponseType(typeof(CandidateGetDto), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllCandidates(CancellationToken cancellation)
    {
        return Ok(await candidates.GetAll(cancellation));
    }
}