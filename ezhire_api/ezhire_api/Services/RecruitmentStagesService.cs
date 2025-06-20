using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IRecruitmentStagesService
{
    Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId);
    Task<RecruitmentStageGetDto> GetById(CancellationToken cancellation, int stageId);
    Task<RecruitmentStageGetDto> Create(CancellationToken cancellation, GenericRecruitmentStageCreateDto stage);
}

public class RecruitmentStagesService(IRecruitmentStagesRepository stages, IJobPostingRepository postings) : IRecruitmentStagesService
{
    public async Task<ICollection<RecruitmentStageGetDto>> GetAllForId(CancellationToken cancellation, int postingId)
    {
        return await stages.GetAllForId(cancellation, postingId);
    }

    public async Task<RecruitmentStageGetDto> GetById(CancellationToken cancellation, int stageId)
    {
        var stage = await stages.GetById(cancellation, stageId);
        if (stage == null) throw new NotFound();

        return stage;
    }

    public async Task<RecruitmentStageGetDto> Create(CancellationToken cancellation,
        GenericRecruitmentStageCreateDto stage)
    {
        var posting = await  postings.GetById(cancellation, stage.PostingId);

        if (posting == null)
        {
            throw new NotFound("Posting not found");
        }
        
        if (stage.TechnologyName != null)
            return await stages.Create(cancellation, new TechnicalMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                TechnologyName = stage.TechnologyName
            });

        if (stage.TeamName != null)
            return await stages.Create(cancellation, new TeamMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                TeamName = stage.TeamName
            });

        if (stage.Values != null)
            return await stages.Create(cancellation, new CultureMeetingCreateDto
            {
                Description = stage.Description,
                PostingId = stage.PostingId,
                Values = stage.Values
            });

        throw new BadRequest("Invalid meeting type");
    }
}