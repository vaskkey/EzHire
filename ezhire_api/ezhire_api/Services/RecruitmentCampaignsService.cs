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
    public Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, int id,
        CampaignPostingCreateDto posting);
}

public class RecruitmentCampaignsService(IRecruitmentCampaignsRepository campaigns, IJobPostingRepository jobPosting) : IRecruitmentCampaignsService
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

    public async Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, int id, CampaignPostingCreateDto posting)
    {
        var campaign = await campaigns.GetById(cancellation, id);
        
        if (campaign == null)
        {
            throw new NotFound();
        }
        
        return await jobPosting.AddPosting(cancellation, new JobPostingCreateDto
        {
            JobName = posting.JobName,
            Description = posting.Description,
            Status = PostingStatus.OPEN,
            DatePosted = DateTime.Now,
            CampaignId = campaign.Id
        });
    }
}