using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IRecruitmentCampaignsService
{
    public Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation);
    public Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id);
    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign);
}

public class RecruitmentCampaignsService(IRecruitmentCampaignsRepository campaigns) : IRecruitmentCampaignsService
{
    public async Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation)
    {
        return await campaigns.GetAll(cancellation);
    }

    public async Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id)
    {
        var campaign = await campaigns.GetById(cancellation, id);
        
        if (campaign == null)
        {
            throw new NotFound();
        }

        return campaign;
    }

    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation, RecruitmentCampaignCreateDto campaign)
    {
        return campaigns.Create(cancellation, campaign);
    }
}