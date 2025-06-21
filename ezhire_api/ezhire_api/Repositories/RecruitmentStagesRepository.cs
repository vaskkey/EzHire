using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for managing recruitment stages for job postings, including retrieval and creation of specific meeting types.
/// </summary>
public interface IRecruitmentStagesRepository
{
    /// <summary>
    /// Retrieves all recruitment stages for a specific job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>A collection of recruitment stage DTOs for the posting.</returns>
    Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Retrieves a recruitment stage by its unique identifier.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="stageId">The unique identifier of the recruitment stage.</param>
    /// <returns>The recruitment stage DTO, or null if not found.</returns>
    Task<RecruitmentStageGetDto?> GetById(CancellationToken cancellation, int stageId);

    /// <summary>
    /// Creates a new technical meeting stage for a job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="meeting">The technical meeting creation data.</param>
    /// <returns>The created recruitment stage DTO.</returns>
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TechnicalMeetingCreateDto meeting);

    /// <summary>
    /// Creates a new team meeting stage for a job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="meeting">The team meeting creation data.</param>
    /// <returns>The created recruitment stage DTO.</returns>
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TeamMeetingCreateDto meeting);

    /// <summary>
    /// Creates a new culture meeting stage for a job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="meeting">The culture meeting creation data.</param>
    /// <returns>The created recruitment stage DTO.</returns>
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, CultureMeetingCreateDto meeting);

    /// <summary>
    /// Converts a RecruitmentStage entity to the correct DTO type based on its subtype.
    /// </summary>
    /// <param name="stage">The recruitment stage entity.</param>
    /// <returns>The corresponding recruitment stage DTO.</returns>
    RecruitmentStageGetDto GetCorrectStage(RecruitmentStage stage);
}

public class RecruitmentStagesRepository(EzHireContext data) : IRecruitmentStagesRepository
{
    /// <inheritdoc />
    public async Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId)
    {
        var filteredStages = await data.RecruitmentStages
            .Where(stage => stage.PostingId == postingId)
            .ToListAsync(cancellation);
        return filteredStages.Select(GetCorrectStage).ToList();
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageGetDto?> GetById(CancellationToken cancellation, int stageId)
    {
        var filteredStages = await data.RecruitmentStages
            .Include(stage => stage.Posting)
            .Where(stage => stage.Id == stageId)
            .ToListAsync(cancellation);

        return filteredStages.Select(GetCorrectStage).FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TechnicalMeetingCreateDto meeting)
    {
        var record = await data.AddAsync(new TechnicalMeeting
        {
            Description = meeting.Description,
            PostingId = meeting.PostingId,
            TechnologyName = meeting.TechnologyName
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return await GetById(cancellation, record.Entity.Id);
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TeamMeetingCreateDto meeting)
    {
        var record = await data.AddAsync(new TeamMeeting
        {
            Description = meeting.Description,
            PostingId = meeting.PostingId,
            TeamName = meeting.TeamName
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return await GetById(cancellation, record.Entity.Id);
    }

    /// <inheritdoc />
    public async Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, CultureMeetingCreateDto meeting)
    {
        var record = await data.AddAsync(new CultureMeeting
        {
            Description = meeting.Description,
            PostingId = meeting.PostingId,
            Values = meeting.Values
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return await GetById(cancellation, record.Entity.Id);
    }

    /// <inheritdoc />
    public RecruitmentStageGetDto GetCorrectStage(RecruitmentStage stage)
    {
        if (stage is TechnicalMeeting technical) return GetTechnicalDto(technical);

        if (stage is TeamMeeting team) return GetTeamDto(team);

        if (stage is CultureMeeting culture) return GetCultureDto(culture);

        throw new Exception("Unknown recruitment type");
    }

    private TeamMeetingGetDto GetTeamDto(TeamMeeting stage)
    {
        return new TeamMeetingGetDto
        {
            Id = stage.Id,
            CreatedAt = stage.CreatedAt,
            UpdatedAt = stage.UpdatedAt,
            Description = stage.Description,
            Posting = new CampaignPostingGetDto
            {
                Id = stage.Posting.Id,
                CreatedAt = stage.Posting.CreatedAt,
                UpdatedAt = stage.Posting.UpdatedAt,
                JobName = stage.Posting.JobName,
                DatePosted = stage.Posting.DatePosted,
                Description = stage.Posting.Description,
                Status = stage.Posting.Status
            },
            TeamName = stage.TeamName
        };
    }

    private CultureMeetingGetDto GetCultureDto(CultureMeeting stage)
    {
        return new CultureMeetingGetDto
        {
            Id = stage.Id,
            CreatedAt = stage.CreatedAt,
            UpdatedAt = stage.UpdatedAt,
            Description = stage.Description,
            Posting = new CampaignPostingGetDto
            {
                Id = stage.Posting.Id,
                CreatedAt = stage.Posting.CreatedAt,
                UpdatedAt = stage.Posting.UpdatedAt,
                JobName = stage.Posting.JobName,
                DatePosted = stage.Posting.DatePosted,
                Description = stage.Posting.Description,
                Status = stage.Posting.Status
            },
            Values = stage.Values
        };
    }

    private static RecruitmentStageGetDto GetTechnicalDto(TechnicalMeeting stage)
    {
        return new TechnicalMeetingGetDto
        {
            Id = stage.Id,
            Description = stage.Description,
            Posting = new CampaignPostingGetDto
            {
                Id = stage.Posting.Id,
                CreatedAt = stage.Posting.CreatedAt,
                UpdatedAt = stage.Posting.UpdatedAt,
                JobName = stage.Posting.JobName,
                DatePosted = stage.Posting.DatePosted,
                Description = stage.Posting.Description,
                Status = stage.Posting.Status
            },
            TechnologyName = stage.TechnologyName
        };
    }
}