using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for retrieving user information by email address.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Retrieves a user DTO by their email address.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="email">The email address of the user to retrieve. If null, returns null.</param>
    /// <returns>The user DTO (RecruiterGetDto or HiringManagerGetDto), or null if not found or email is null.</returns>
    Task<UserGetDto?> GetUserByEmail(CancellationToken cancellation, string? email);
}

public class UsersRepository(EzHireContext data) : IUsersRepository
{
    /// <inheritdoc />
    public async Task<UserGetDto?> GetUserByEmail(CancellationToken cancellation, string? email)
    {
        if (email == null) return null;

        var record = await data.Users.FirstOrDefaultAsync(user => user.Email == email, cancellation);

        if (record == null) return null;

        return GetUserDto(record);
    }

    private UserGetDto GetUserDto(User record)
    {
        if (record is Recruiter recruiter)
            return new RecruiterGetDto
            {
                Id = recruiter.Id,
                FirstName = recruiter.FirstName,
                LastName = recruiter.LastName,
                Email = recruiter.Email,
                Type = recruiter.Type
            };

        if (record is HiringManager manager)
            return new HiringManagerGetDto
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Email = manager.Email,
                Department = manager.Department
            };

        throw new Exception("Unknown user type");
    }
}