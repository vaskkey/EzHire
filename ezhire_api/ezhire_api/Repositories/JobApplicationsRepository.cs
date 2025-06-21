using System.ComponentModel;
using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for managing job applications and their related meetings.
/// </summary>
public interface IJobApplicationsRepository
{
    /// <summary>
    /// Retrieves all job applications for a specific job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>A collection of job application DTOs for the posting.</returns>
    Task<ICollection<PostingApplicationDto>> GetByPostingId(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Retrieves a job application by its unique identifier.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the job application.</param>
    /// <returns>The job application DTO, or null if not found.</returns>
    Task<JobApplicationGetDto?> GetById(CancellationToken cancellation, int applicationId);

    /// <summary>
    /// Creates a new job application.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="application">The job application creation data.</param>
    /// <returns>The created job application DTO.</returns>
    Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application);

    /// <summary>
    /// Changes the status of a job application.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the job application.</param>
    /// <param name="newStatus">The new status to set for the application.</param>
    /// <returns>The updated job application DTO, or null if not found.</returns>
    Task<JobApplicationGetDto?> ChangeStatus(CancellationToken cancellation, int applicationId,
        ApplicationStatus newStatus);

    /// <summary>
    /// Retrieves a meeting associated with a job application and recruitment stage.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the job application.</param>
    /// <param name="recruitmentStageId">The unique identifier of the recruitment stage.</param>
    /// <returns>The recruitment stage meeting DTO, or null if not found.</returns>
    Task<RecruitmentStageMeetingGetDto?> GetMeeting(CancellationToken cancellation, int applicationId,
        int recruitmentStageId);

    /// <summary>
    /// Retrieves a meeting by its id
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="meetingId">The unique identifier of the meeting.</param>
    /// <returns></returns>
    Task<RecruitmentStageMeetingGetDto?> GetMeeting(CancellationToken cancellation, int meetingId);

    /// <summary>
    /// Plans a new meeting for a job application in a given recruitment stage.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the job application.</param>
    /// <param name="meeting">The details of the meeting to plan.</param>
    /// <returns>The created recruitment stage meeting DTO.</returns>
    Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int applicationId,
        ApplicationMeetingPlanDto meeting);

    /// <summary>
    /// Logs details for a meeting of a job application and recruitment stage.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="applicationId">The unique identifier of the job application.</param>
    /// <param name="meeting">The meeting log details (held date, notes, etc).</param>
    /// <returns>The updated recruitment stage meeting DTO, or null if not found.</returns>
    Task<RecruitmentStageMeetingGetDto?> LogMeeting(CancellationToken cancellation, int applicationId,
        ApplicationMeetingLogDto meeting);

    /// <summary>
    /// Retrieves a job application by candidate email and posting ID. Used to check for duplicate applications.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="email">The email address of the candidate.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>The job application DTO, or null if not found.</returns>
    Task<JobApplicationGetDto?> GetByEmail(CancellationToken cancellation, string email, int postingId);
}

public class JobApplicationsRepository(EzHireContext data, IRecruitmentStagesRepository stages)
    : IJobApplicationsRepository
{
    /// <inheritdoc />
    public async Task<JobApplicationGetDto?> GetById(CancellationToken cancellation, int id)
    {
        return await data.JobApplications
            .Where(application => application.Id == id)
            .Select(application => new JobApplicationGetDto
            {
                Id = application.Id,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt,
                DateApplied = application.DateApplied,
                Status = application.Status,
                PostingId = application.PostingId,
                ApplicantId = application.ApplicantId,
                Posting = new CampaignPostingGetDto
                {
                    Id = application.Posting.Id,
                    CreatedAt = application.Posting.CreatedAt,
                    UpdatedAt = application.Posting.UpdatedAt,
                    JobName = application.Posting.JobName,
                    DatePosted = application.Posting.DatePosted,
                    Description = application.Posting.Description,
                    Status = application.Posting.Status
                },
                Applicant = new ApplicantDto
                {
                    Id = application.Applicant.Id,
                    CreatedAt = application.Applicant.CreatedAt,
                    UpdatedAt = application.Applicant.UpdatedAt,
                    FirstName = application.Applicant.FirstName,
                    LastName = application.Applicant.LastName,
                    Email = application.Applicant.Email
                }
            })
            .FirstOrDefaultAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<JobApplicationGetDto?> GetByEmail(CancellationToken cancellation, string candidateEmail,
        int postingId)
    {
        return await data.JobApplications
            .Where(application => application.PostingId == postingId && application.Applicant.Email == candidateEmail)
            .Select(application => new JobApplicationGetDto
            {
                Id = application.Id,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt,
                DateApplied = application.DateApplied,
                Status = application.Status,
                PostingId = application.PostingId,
                ApplicantId = application.ApplicantId,
                Posting = new CampaignPostingGetDto
                {
                    Id = application.Posting.Id,
                    CreatedAt = application.Posting.CreatedAt,
                    UpdatedAt = application.Posting.UpdatedAt,
                    JobName = application.Posting.JobName,
                    DatePosted = application.Posting.DatePosted,
                    Description = application.Posting.Description,
                    Status = application.Posting.Status
                },
                Applicant = new ApplicantDto
                {
                    Id = application.Applicant.Id,
                    CreatedAt = application.Applicant.CreatedAt,
                    UpdatedAt = application.Applicant.UpdatedAt,
                    FirstName = application.Applicant.FirstName,
                    LastName = application.Applicant.LastName,
                    Email = application.Applicant.Email
                }
            })
            .FirstOrDefaultAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application)
    {
        var entity = await data.JobApplications.AddAsync(new JobApplication
        {
            DateApplied = application.DateApplied,
            Status = application.Status,
            PostingId = application.PostingId,
            ApplicantId = application.ApplicantId
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return await GetById(cancellation, entity.Entity.Id);
    }

    /// <inheritdoc />
    public async Task<JobApplicationGetDto?> ChangeStatus(CancellationToken cancellation, int applicationId,
        ApplicationStatus status)
    {
        var application = await data.JobApplications
            .Include(application => application.Posting)
            .Include(application => application.Applicant)
            .Where(application => application.Id == applicationId)
            .FirstOrDefaultAsync(cancellation);

        if (application == null) return null;

        data.JobApplications.Update(application);

        application.Status = status;

        await data.SaveChangesAsync(cancellation);

        return new JobApplicationGetDto
        {
            Id = application.Id,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt,
            DateApplied = application.DateApplied,
            Status = application.Status,
            PostingId = application.PostingId,
            ApplicantId = application.ApplicantId,
            Posting = new CampaignPostingGetDto
            {
                Id = application.Posting.Id,
                CreatedAt = application.Posting.CreatedAt,
                UpdatedAt = application.Posting.UpdatedAt,
                JobName = application.Posting.JobName,
                DatePosted = application.Posting.DatePosted,
                Description = application.Posting.Description,
                Status = application.Posting.Status
            },
            Applicant = new ApplicantDto
            {
                Id = application.Applicant.Id,
                CreatedAt = application.Applicant.CreatedAt,
                UpdatedAt = application.Applicant.UpdatedAt,
                FirstName = application.Applicant.FirstName,
                LastName = application.Applicant.LastName,
                Email = application.Applicant.Email
            }
        };
    }

    /// <inheritdoc />
    public async Task<ICollection<PostingApplicationDto>> GetByPostingId(CancellationToken cancellation, int postingId)
    {
        return await data.JobApplications
            .Where(application => application.Id == postingId)
            .Select(application => new PostingApplicationDto
                {
                    Id = application.Id,
                    CreatedAt = application.CreatedAt,
                    UpdatedAt = application.UpdatedAt,
                    DateApplied = application.DateApplied,
                    Status = application.Status,
                    PostingId = application.PostingId,
                    ApplicantId = application.ApplicantId,
                    Applicant = new ApplicantDto
                    {
                        Id = application.Applicant.Id,
                        CreatedAt = application.Applicant.CreatedAt,
                        UpdatedAt = application.Applicant.UpdatedAt,
                        FirstName = application.Applicant.FirstName,
                        LastName = application.Applicant.LastName,
                        Email = application.Applicant.Email
                    }
                }
            )
            .ToListAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageMeetingGetDto?> GetMeeting(CancellationToken cancellation, int applicationId,
        int stageId)
    {
        return await data.RecruitmentStageMeetings
            .Include(meeting => meeting.Stage)
            .ThenInclude(stage => stage.Posting)
            .Where(meeting => meeting.ApplicationId == applicationId && meeting.RecruitmentStageId == stageId)
            .Select(meeting => new RecruitmentStageMeetingGetDto
            {
                Id = meeting.Id,
                CreatedAt = meeting.CreatedAt,
                UpdatedAt = meeting.UpdatedAt,
                Date = meeting.Date,
                Grade = meeting.Grade,
                Comment = meeting.Comment,
                Stage = stages.GetCorrectStage(meeting.Stage),
                Application = new JobApplicationGetDto
                {
                    Id = meeting.Application.Id,
                    CreatedAt = meeting.Application.CreatedAt,
                    UpdatedAt = meeting.Application.UpdatedAt,
                    DateApplied = meeting.Application.DateApplied,
                    Status = meeting.Application.Status,
                    PostingId = meeting.Application.PostingId,
                    ApplicantId = meeting.Application.ApplicantId
                }
            })
            .FirstOrDefaultAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageMeetingGetDto?> GetMeeting(CancellationToken cancellation, int meetingId)
    {
        return await data.RecruitmentStageMeetings
            .Where(meeting => meeting.Id == meetingId)
            .Select(meeting => new RecruitmentStageMeetingGetDto
            {
                Id = meeting.Id,
                CreatedAt = meeting.CreatedAt,
                UpdatedAt = meeting.UpdatedAt,
                Date = meeting.Date,
                Grade = meeting.Grade,
                Comment = meeting.Comment,
                Stage = stages.GetCorrectStage(meeting.Stage),
                Application = new JobApplicationGetDto
                {
                    Id = meeting.Application.Id,
                    CreatedAt = meeting.Application.CreatedAt,
                    UpdatedAt = meeting.Application.UpdatedAt,
                    DateApplied = meeting.Application.DateApplied,
                    Status = meeting.Application.Status,
                    PostingId = meeting.Application.PostingId,
                    ApplicantId = meeting.Application.ApplicantId
                }
            })
            .FirstOrDefaultAsync(cancellation);
    }

    public async Task<RecruitmentStageMeetingGetDto> PlanMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingPlanDto plannedMeeting)
    {
        await data.RecruitmentStageMeetings.AddAsync(new RecruitmentStageMeeting
        {
            Date = plannedMeeting.Date,
            ApplicationId = plannedMeeting.ApplicationId,
            RecruitmentStageId = plannedMeeting.RecruitmentStageId
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return await GetMeeting(cancellation, plannedMeeting.ApplicationId, plannedMeeting.RecruitmentStageId);
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageMeetingGetDto> LogMeeting(CancellationToken cancellation, int id,
        ApplicationMeetingLogDto meetingLog)
    {
        var meeting = await data.RecruitmentStageMeetings
            .FirstOrDefaultAsync(meeting => meeting.Id == meetingLog.MeetingId);

        if (meeting == null) return null;

        data.RecruitmentStageMeetings.Update(meeting);

        meeting.Comment = meetingLog.Comment;
        meeting.Grade = meetingLog.Grade;

        await data.SaveChangesAsync(cancellation);

        return await GetMeeting(cancellation, id);
    }
}