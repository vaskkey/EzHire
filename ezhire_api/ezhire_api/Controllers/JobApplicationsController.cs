using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[Authorize]
[ApiController]
[Route("/api/applications")]
public class JobApplicationsController(IJobApplicationsService applications, IAuthService auth) : ControllerBase
{
    [ProducesResponseType(typeof(ICollection<JobApplicationGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetApplications(CancellationToken cancellation, [FromQuery] int postingId)
    {
        return Ok(await applications.GetAllForPosting(cancellation, postingId));
    }

    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        return Ok(await applications.GetById(cancellation, id));
    }

    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> RejectApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.RECRUITER);
        return Ok(await applications.Reject(cancellation, id));
    }

    [ProducesResponseType(typeof(JobApplicationGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> AcceptApplication(CancellationToken cancellation, [FromRoute] int id)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.RECRUITER);
        return Ok(await applications.Accept(cancellation, id));
    }

    [ProducesResponseType(typeof(RecruitmentStageMeetingGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/plan-meeting")]
    public async Task<IActionResult> PlanMeeting(CancellationToken cancellation, [FromRoute] int id,
        [FromBody] ApplicationMeetingPlanDto plannedMeeting)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.RECRUITER);
        return Ok(await applications.PlanMeeting(cancellation, id, plannedMeeting));
    }

    [ProducesResponseType(typeof(RecruitmentStageMeetingGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPost("{id:int}/log-meeting")]
    public async Task<IActionResult> LogMeeting(CancellationToken cancellation, [FromRoute] int id,
        [FromBody] ApplicationMeetingLogDto log)
    {
        await auth.ValidateRole(cancellation, User.Identity, UserType.RECRUITER);
        return Ok(await applications.LogMeeting(cancellation, id, log));
    }
}