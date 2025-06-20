using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[Authorize]
[ApiController]
[Route("/api/campaigns")]
public class RecruitmentCampaignsController(
    IRecruitmentCampaignsService campaigns,
    IJobPostingsService postings,
    IAuthService auth)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<RecruitmentCampaignGetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCampaigns(CancellationToken cancellation)
    {
        return Ok(await campaigns.GetAll(cancellation));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RecruitmentCampaignGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCampaign(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await campaigns.GetById(cancellation, id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecruitmentCampaignGetDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCampaign(CancellationToken cancellation,
        [FromBody] RecruitmentCampaignCreateDto campaign)
    {
        var user = await auth.GetUser(cancellation, User.Identity);
        auth.ValidateRole(user, UserType.HIRING_MANAGER);

        var response = await campaigns.Create(cancellation, campaign, user);
        return CreatedAtAction(nameof(GetCampaign), new { id = response.Id }, response);
    }

    [HttpPost("{id:int}/create-posting")]
    [ProducesResponseType(typeof(JobPostingGetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddPosting(CancellationToken cancellation, [FromRoute] int id,
        [FromBody] JobPostingCreateDto postingData)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.HIRING_MANAGER);
        var campaign = await campaigns.GetById(cancellation, id);
        var response = await postings.AddPosting(cancellation, campaign.Id, postingData);
        return Created($"/api/postings/{response.Id}", response);
    }
}