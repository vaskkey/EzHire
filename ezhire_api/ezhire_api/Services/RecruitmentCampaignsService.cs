using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

/// <summary>
/// Defines operations related to recruitment campaigns, including retrieval and creation.
/// </summary>
public interface IRecruitmentCampaignsService
{
    /// <summary>
    /// Retrieves all recruitment campaigns in the system.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <returns>A collection of recruitment campaign DTOs.</returns>
    Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Retrieves a specific recruitment campaign by its unique identifier.
    /// Throws NotFound if the campaign does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The campaign's unique identifier.</param>
    /// <returns>The recruitment campaign DTO.</returns>
    Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id);

    /// <summary>
    /// Creates a new recruitment campaign.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="campaign">The recruitment campaign creation data.</param>
    /// <param name="createdBy">The user creating the campaign.</param>
    /// <returns>The created recruitment campaign DTO.</returns>
    Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation,
        RecruitmentCampaignCreateDto campaign, UserGetDto createdBy);
}

public class RecruitmentCampaignsService(IRecruitmentCampaignsRepository campaigns) : IRecruitmentCampaignsService
{
    /// <inheritdoc/>
    public async Task<ICollection<RecruitmentCampaignGetDto>> GetAll(CancellationToken cancellation)
    {
        return await campaigns.GetAll(cancellation);
    }

    /// <inheritdoc/>
    public async Task<RecruitmentCampaignGetDto> GetById(CancellationToken cancellation, int id)
    {
        var campaign = await campaigns.GetById(cancellation, id);

        if (campaign == null) throw new NotFound();

        return campaign;
    }

    /// <inheritdoc/>
    public Task<RecruitmentCampaignGetDto> Create(CancellationToken cancellation, RecruitmentCampaignCreateDto campaign,
        UserGetDto user)
    {
        return campaigns.Create(cancellation, campaign, user);
    }
}