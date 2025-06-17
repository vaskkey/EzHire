using System.ComponentModel;
using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface IJobApplicationsRepository
{
    Task<JobApplicationGetDto?> GetById(CancellationToken cancellation, int id);
    Task<JobApplicationGetDto?> GetByEmail(CancellationToken cancellation, string candidateEmail, int postingId);
    public Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application);

    Task<JobApplicationGetDto?> ChangeStatus(CancellationToken cancellation, int applicationId,
        ApplicationStatus status);

    Task<ICollection<PostingApplicationDto>> GetByPostingId(CancellationToken cancellation, int postingId);
}

public class JobApplicationsRepository(EzHireContext data) : IJobApplicationsRepository
{
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

    public async Task<JobApplicationGetDto?> ChangeStatus(CancellationToken cancellation, int applicationId,
        ApplicationStatus status)
    {
        var application = await data.JobApplications
            .Include(application => application.Posting)
            .Include(application => application.Applicant)
            .Where(application => application.Id == applicationId)
            .FirstOrDefaultAsync(cancellation);

        if (application == null)
        {
            return null;
        }

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
}