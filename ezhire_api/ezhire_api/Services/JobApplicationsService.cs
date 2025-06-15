using ezhire_api.DTO;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IJobApplicationsService
{
    public Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application);
}

public class JobApplicationsService(IJobApplicationsRepository applications) : IJobApplicationsService
{
    public async Task<JobApplicationGetDto> Create(CancellationToken cancellation, JobApplicationCreateDto application)
    {
        return await applications.Create(cancellation, application);
    }
}