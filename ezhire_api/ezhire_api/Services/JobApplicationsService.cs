using System.ComponentModel;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

/// <summary>
/// Defines operations for managing job applications, including retrieval, creation,
/// status changes (accept/reject), and interview/meeting management.
/// </summary>
public interface IJobApplicationsService
{
    /// <summary>
    /// Retrieves all applications associated with a specific job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>Collection of posting application DTOs.</returns>
    Task<ICollection<PostingApplicationDto>> GetAllForPosting(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Retrieves a job application by its unique identifier.
    /// Throws NotFound if the application does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the application.</param>
    /// <returns>The job application DTO.</returns>
    Task<JobApplicationGetDto> GetById(CancellationToken cancellation, int applicationId);

    /// <summary>
    /// Creates a new job application.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="application">The job application creation data.</param>
    /// <returns>The created job application DTO.</returns>
    Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application);

    /// <summary>
    /// Rejects an application by changing its status to REJECTED.
    /// Throws NotFound if the application does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the application to reject.</param>
    /// <returns>The updated job application DTO.</returns>
    Task<JobApplicationGetDto> Reject(CancellationToken cancellation, int applicationId);

    /// <summary>
    /// Accepts an application by changing its status to ACCEPTED.
    /// Throws NotFound if the application does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the application to accept.</param>
    /// <returns>The updated job application DTO.</returns>
    Task<JobApplicationGetDto> Accept(CancellationToken cancellation, int applicationId);

    /// <summary>
    /// Schedules a meeting/interview for a specific application and recruitment stage.
    /// Throws BadRequest if a meeting of this type has already taken place.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The unique identifier of the application.</param>
    /// <param name="plannedMeeting">Details of the planned meeting.</param>
    /// <returns>The DTO representing the planned meeting.</returns>
    Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingPlanDto plannedMeeting);

    /// <summary>
    /// Logs a meeting/interview for a specific application and recruitment stage.
    /// Throws NotFound if the application or meeting does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The unique identifier of the application.</param>
    /// <param name="plannedMeeting">Details of the meeting to log.</param>
    /// <returns>The DTO representing the logged meeting.</returns>
    Task<RecruitmentStageMeetingGetDto> LogMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingLogDto plannedMeeting);
}

public class JobApplicationsService(IJobApplicationsRepository applications) : IJobApplicationsService
{
    /// <inheritdoc/>
    public async Task<ICollection<PostingApplicationDto>> GetAllForPosting(CancellationToken cancellation,
        int postingId)
    {
        return await applications.GetByPostingId(cancellation, postingId);
    }

    /// <inheritdoc/>
    public async Task<JobApplicationGetDto> GetById(CancellationToken cancellation, int applicationId)
    {
        var application = await applications.GetById(cancellation, applicationId);

        if (application == null) throw new NotFound();

        return application;
    }

    /// <inheritdoc/>
    public async Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application)
    {
        return await applications.Create(cancellation, application);
    }

    /// <inheritdoc/>
    public async Task<JobApplicationGetDto> Reject(CancellationToken cancellation, int applicationId)
    {
        var record = await applications.ChangeStatus(cancellation, applicationId, ApplicationStatus.REJECTED);

        if (record == null) throw new NotFound();

        return record;
    }

    /// <inheritdoc/>
    public async Task<JobApplicationGetDto> Accept(CancellationToken cancellation, int applicationId)
    {
        var record = await applications.ChangeStatus(cancellation, applicationId, ApplicationStatus.ACCEPTED);

        if (record == null) throw new NotFound();

        return record;
    }

    /// <inheritdoc/>
    public async Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingPlanDto plannedMeeting)
    {
        var record = await applications.GetMeeting(cancellation, id, plannedMeeting.RecruitmentStageId);
        if (record != null) throw new BadRequest("Meeting of this type has already taken place");

        return await applications.PlanMeeting(cancellation, id, plannedMeeting);
    }

    /// <inheritdoc/>
    public async Task<RecruitmentStageMeetingGetDto> LogMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingLogDto meetingLog)
    {
        var record = await applications.LogMeeting(cancellation, id, meetingLog);

        if (record == null) throw new NotFound();


        return record;
    }
}