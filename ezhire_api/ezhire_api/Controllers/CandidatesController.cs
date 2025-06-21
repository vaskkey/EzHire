using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[Authorize]
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

    [ProducesResponseType(typeof(OfferGetDto), StatusCodes.Status200OK)]
    [HttpPost("{id:int}")]
    public async Task<IActionResult> ExtendOffer(CancellationToken cancellation, [FromRoute] int id)
    {
        var candidate = await candidates.GetById(cancellation, id);
        return Ok(await candidates.ExtendOffer(cancellation, candidate));
    }
}