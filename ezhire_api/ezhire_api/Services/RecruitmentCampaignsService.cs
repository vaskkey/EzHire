using ezhire_api.DTO;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IRecruitmentCampaignsService
{
    public Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation);
    public Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id);
    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign);
}

public class RecruitmentCampaignsService(IRecruitmentCampaignsRepository data) : IRecruitmentCampaignsService
{
    public async Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.GetAll(cancellation);
    }

    public async Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id)
    {
        return await data.GetById(cancellation, id);
    }

    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation, RecruitmentCampaignCreateDto campaign)
    {
        return data.Create(cancellation, campaign);
    }
}