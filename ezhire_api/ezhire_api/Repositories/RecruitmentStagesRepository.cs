using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface IRecruitmentStagesRepository
{
    Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId);
    Task<RecruitmentStageGetDto?> GetById(CancellationToken cancellation, int stageId);
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TechnicalMeetingCreateDto meeting);
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, TeamMeetingCreateDto meeting);
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, CultureMeetingCreateDto meeting);
}

public class RecruitmentStagesRepository(EzHireContext data) : IRecruitmentStagesRepository
{
    public async Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId)
    {
        var filteredStages = await data.RecruitmentStages
            .Where(stage => stage.PostingId == postingId)
            .ToListAsync(cancellation);
        return filteredStages.Select(GetCorrectStage).ToList();
    }

    public async Task<RecruitmentStageGetDto?> GetById(CancellationToken cancellation, int stageId)
    {
        var filteredStages = await data.RecruitmentStages
            .Include(stage => stage.Posting)
            .Where(stage => stage.Id == stageId)
            .ToListAsync(cancellation);
        
        return filteredStages.Select(GetCorrectStage).FirstOrDefault();
    }

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

    private RecruitmentStageGetDto GetCorrectStage(RecruitmentStage stage)
    {
        if (stage is TechnicalMeeting technical)
        {
            return GetTechnicalDto(technical);
        }
        
        if (stage is TeamMeeting team)
        {
            return GetTeamDto(team);
        }
        
        if (stage is CultureMeeting culture)
        {
            return GetCultureDto(culture);
        }

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
                Status = stage.Posting.Status,
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
                Status = stage.Posting.Status,
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
                Status = stage.Posting.Status,
            },
            TechnologyName = stage.TechnologyName,
        };
    }
}