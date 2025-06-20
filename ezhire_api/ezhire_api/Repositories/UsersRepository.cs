using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface IUsersRepository
{
    Task<UserGetDto?> GetUserByEmail(CancellationToken cancellation, string? email);
}

public class UsersRepository(EzHireContext data) : IUsersRepository
{
    public async Task<UserGetDto?> GetUserByEmail(CancellationToken cancellation, string? email)
    {
        if (email == null)
        {
            return null;
        }

        var record = await data.Users.FirstOrDefaultAsync(user => user.Email == email, cancellation);

        if (record == null)
        {
            return null;
        }

        return GetUserDto(record);
    }

    private UserGetDto GetUserDto(User record)
    {
        if (record is Recruiter recruiter)
        {
            return new RecruiterGetDto
            {
                Id = recruiter.Id,
                FirstName = recruiter.FirstName,
                LastName = recruiter.LastName,
                Email = recruiter.Email,
                Type = recruiter.Type
            };
        }

        if (record is HiringManager manager)
        {
            return new HiringManagerGetDto
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Email = manager.Email,
                Department = manager.Department
            };
        }
        
        throw new Exception("Unknown user type");
    }
}