using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[ApiController]
[Route("/api/campaigns")]
public class RecruitmentCampaignsController(IRecruitmentCampaignsService campaigns) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCampaigns(CancellationToken cancellation)
    {
        return Ok(await campaigns.GetAll(cancellation));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaign(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await campaigns.GetById(cancellation, id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCampaign(CancellationToken cancellation,
        [FromBody] RecruitmentCampaignCreateDto campaign)
    {
        var response = await campaigns.Create(cancellation, campaign);
        return CreatedAtAction(nameof(GetCampaign), new { id = response.Id }, response);
    }
}