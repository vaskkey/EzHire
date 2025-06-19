using System.ComponentModel;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IJobApplicationsService
{
    public Task<ICollection<PostingApplicationDto>> GetAllForPosting(CancellationToken cancellation, int postingId);
    public Task<JobApplicationGetDto> GetById(CancellationToken cancellation, int applicationId);
    public Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application);
    public Task<JobApplicationGetDto> Reject(CancellationToken cancellation, int applicationId);
    public Task<JobApplicationGetDto> Accept(CancellationToken cancellation, int applicationId);

    Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingPlanDto plannedMeeting);

    Task<RecruitmentStageMeetingGetDto> LogMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingLogDto plannedMeeting);
}

public class JobApplicationsService(IJobApplicationsRepository applications) : IJobApplicationsService
{
    public async Task<ICollection<PostingApplicationDto>> GetAllForPosting(CancellationToken cancellation,
        int postingId)
    {
        return await applications.GetByPostingId(cancellation, postingId);
    }

    public async Task<JobApplicationGetDto> GetById(CancellationToken cancellation, int applicationId)
    {
        var application = await applications.GetById(cancellation, applicationId);

        if (application == null) throw new NotFound();

        return application;
    }

    public async Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application)
    {
        return await applications.Create(cancellation, application);
    }

    public async Task<JobApplicationGetDto> Reject(CancellationToken cancellation, int applicationId)
    {
        var record = await applications.ChangeStatus(cancellation, applicationId, ApplicationStatus.REJECTED);

        if (record == null) throw new NotFound();

        return record;
    }

    public async Task<JobApplicationGetDto> Accept(CancellationToken cancellation, int applicationId)
    {
        var record = await applications.ChangeStatus(cancellation, applicationId, ApplicationStatus.ACCEPTED);

        if (record == null) throw new NotFound();

        return record;
    }

    public async Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingPlanDto plannedMeeting)
    {
        var record = await applications.GetMeeting(cancellation, id, plannedMeeting.RecruitmentStageId);
        if (record != null) throw new BadRequest("Meeting of this type has already taken place");

        return await applications.PlanMeeting(cancellation, id, plannedMeeting);
    }

    public async Task<RecruitmentStageMeetingGetDto> LogMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingLogDto meetingLog)
    {
        var record = await applications.LogMeeting(cancellation, id, meetingLog);

        if (record == null) throw new NotFound();


        return record;
    }
}