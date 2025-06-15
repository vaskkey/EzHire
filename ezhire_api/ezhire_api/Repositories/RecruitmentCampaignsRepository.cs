using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface IRecruitmentCampaignsRepository
{
    public Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation);
    public Task<RecruitmentCampaignGetDto?> GetById(CancellationToken cancellation, int id);

    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign);
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
        var campaign = await data.Campaigns.Select(campaign => new RecruitmentCampaignGetDto
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Priority = campaign.Priority,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt
            })
            .Where(campaign => campaign.Id == id)
            .FirstOrDefaultAsync(cancellation);

        return campaign;
    }

    public async Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign)
    {
        var entry = await data.Campaigns.AddAsync(new RecruitmentCampaign
        {
            Name = campaign.Name,
            Priority = campaign.Priority
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