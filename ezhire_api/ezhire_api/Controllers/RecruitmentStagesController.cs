using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[ApiController]
[Route("/api/stages")]
public class RecruitmentStagesController(IRecruitmentStagesService stages) : ControllerBase
{
    [ProducesResponseType(typeof(ICollection<RecruitmentStageGetDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllStages(CancellationToken cancellation, [FromQuery] int stageId)
    {
        return Ok(await stages.GetAllForId(cancellation, stageId));
    }

    [ProducesResponseType(typeof(RecruitmentStageGetDto), StatusCodes.Status200OK)]
    [HttpGet("{stageId:int}")]
    public async Task<IActionResult> GetStage(CancellationToken cancellation, [FromRoute] int stageId)
    {
        return Ok(await stages.GetById(cancellation, stageId));
    }

    [ProducesResponseType(typeof(RecruitmentStageGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> AddStage(CancellationToken cancellation,
        [FromBody] GenericRecruitmentStageCreateDto stage)
    {
        return Ok(await stages.Create(cancellation, stage));
    }
}