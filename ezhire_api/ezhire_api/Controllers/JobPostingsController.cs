using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[Authorize]
[ApiController]
[Route("/api/postings")]
public class JobPostingsController(IJobPostingsService postings, IAuthService auth) : ControllerBase
{
    [ProducesResponseType(typeof(ICollection<CampaignPostingGetDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllPostings(CancellationToken cancellation, [FromQuery] int? campaignId)
    {
        return Ok(await postings.GetAllForId(cancellation, campaignId));
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(JobPostingGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPosting(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await postings.GetById(cancellation, id));
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(JobPostingGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}")]
    public async Task<IActionResult> ApplyToPosting(CancellationToken cancellation,
        [FromRoute] int id,
        [FromBody] CandidateCreateDto candidateApplication)
    {
        return Ok(await postings.Apply(cancellation, id, candidateApplication));
    }

    [ProducesResponseType(typeof(JobPostingGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> ClosePosting(CancellationToken cancellation,
        [FromRoute] int id)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.HIRING_MANAGER);
        return Ok(await postings.Close(cancellation, id));
    }
}