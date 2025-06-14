using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[Controller]
[Route("/api/[controller]")]
public class CandidatesController(ICandidatesService candidates) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCandidates(CancellationToken cancellation)
    {
        return Ok(await candidates.GetAll(cancellation));
    }
}