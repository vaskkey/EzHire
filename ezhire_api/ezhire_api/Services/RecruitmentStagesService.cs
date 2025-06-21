using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

/// <summary>
/// Defines operations related to recruitment stages within a campaign, including retrieval and creation.
/// </summary>
public interface IRecruitmentStagesService
{
    /// <summary>
    /// Retrieves all recruitment stages for a specific job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>A collection of recruitment stage DTOs.</returns>
    Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Retrieves a specific recruitment stage by its unique identifier.
    /// Throws NotFound if the stage does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="stageId">The recruitment stage's unique identifier.</param>
    /// <returns>The recruitment stage DTO.</returns>
    Task<RecruitmentStageGetDto> GetById(CancellationToken cancellation, int stageId);

    /// <summary>
    /// Creates a new recruitment stage (of type technical, team, or culture meeting) for a job posting.
    /// Throws NotFound if the posting does not exist or BadRequest if the meeting type is invalid.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="stage">The generic recruitment stage creation data.</param>
    /// <param name="user">The recruiter creating the stage.</param>
    /// <returns>The created recruitment stage DTO.</returns>
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, GenericRecruitmentStageCreateDto stage,
        UserGetDto user);
}

public class RecruitmentStagesService(IRecruitmentStagesRepository stages, IJobPostingRepository postings)
    : IRecruitmentStagesService
{
    /// <inheritdoc/>
    public async Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId)
    {
        return await stages.GetAllForId(cancellation, postingId);
    }

    /// <inheritdoc/>
    public async Task<RecruitmentStageGetDto> GetById(CancellationToken cancellation, int stageId)
    {
        var stage = await stages.GetById(cancellation, stageId);
        if (stage == null) throw new NotFound();

        return stage;
    }

    /// <inheritdoc/>
    public async Task<RecruitmentStageGetDto> Create(CancellationToken cancellation,
        GenericRecruitmentStageCreateDto stage, UserGetDto user)
    {
        var posting = await postings.GetById(cancellation, stage.PostingId);

        if (posting == null) throw new NotFound("Posting not found");

        if (stage.TechnologyName != null)
            return await stages.Create(cancellation, new TechnicalMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                TechnologyName = stage.TechnologyName,
                RecruiterId = user.Id
            });

        if (stage.TeamName != null)
            return await stages.Create(cancellation, new TeamMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                TeamName = stage.TeamName,
                RecruiterId = user.Id
            });

        if (stage.Values != null)
            return await stages.Create(cancellation, new CultureMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                Values = stage.Values,
                RecruiterId = user.Id
            });

        throw new BadRequest("Invalid meeting type");
    }
}