using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for managing recruitment campaigns, including retrieval and creation.
/// </summary>
public interface IRecruitmentCampaignsRepository
{
    /// <summary>
    /// Retrieves all recruitment campaigns.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <returns>A collection of all recruitment campaign DTOs.</returns>
    Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Retrieves a recruitment campaign by its unique identifier.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The unique identifier of the recruitment campaign.</param>
    /// <returns>The recruitment campaign DTO, or null if not found.</returns>
    Task<RecruitmentCampaignGetDto?> GetById(CancellationToken cancellation, int id);

    /// <summary>
    /// Creates a new recruitment campaign.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="campaign">The recruitment campaign creation data.</param>
    /// <param name="user">The user (manager) creating the campaign.</param>
    /// <returns>The created recruitment campaign DTO.</returns>
    Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation, RecruitmentCampaignCreateDto campaign,
        UserGetDto user);
}

public class RecruitmentCampaignsRepository(EzHireContext data) : IRecruitmentCampaignsRepository
{
    public async Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.Campaigns.Select(campaign => new RecruitmentCampaignGetDto
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Priority = campaign.Priority,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt
            })
            .ToListAsync(cancellation);
    }

    public async Task<RecruitmentCampaignGetDto?> GetById(CancellationToken cancellation, int id)
    {
        var campaign = await data.Campaigns
            .Where(campaign => campaign.Id == id)
            .Select(campaign => new RecruitmentCampaignGetDto
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Priority = campaign.Priority,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt,
                Postings = campaign.JobPostings.Select(posting => new CampaignPostingGetDto
                {
                    Id = posting.Id,
                    CreatedAt = posting.CreatedAt,
                    UpdatedAt = posting.UpdatedAt,
                    JobName = posting.JobName,
                    DatePosted = posting.DatePosted,
                    Description = posting.Description,
                    Status = posting.Status
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellation);

        return campaign;
    }

    public async Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign, UserGetDto user)
    {
        var entry = await data.Campaigns.AddAsync(new RecruitmentCampaign
            {
                Name = campaign.Name,
                Priority = campaign.Priority,
                ManagerId = user.Id
            },
            cancellation);

        await data.SaveChangesAsync(cancellation);

        return new RecruitmentCampaignGetDto
        {
            Id = entry.Entity.Id,
            CreatedAt = entry.Entity.CreatedAt,
            UpdatedAt = entry.Entity.UpdatedAt,
            Name = entry.Entity.Name,
            Priority = entry.Entity.Priority
        };
    }
}