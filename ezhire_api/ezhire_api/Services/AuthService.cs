using System.Security.Principal;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ezhire_api.Services;

/// <summary>
/// Defines authentication and authorization operations for users, including registration, login,
/// user identity resolution, and role validation.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user (either Hiring Manager or Recruiter) with the given data.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="userData">User creation data, including type, names, email, department or recruiter type, and password.</param>
    /// <returns>User DTO representing the registered user.</returns>
    Task<UserGetDto> Register(CancellationToken cancellation, UserCreateDto userData);

    /// <summary>
    /// Attempts to log in a user with the provided credentials.
    /// Throws an exception if login fails or account is locked out.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="userData">Login data including email and password.</param>
    Task Login(CancellationToken cancellation, UserLoginDto userData);

    /// <summary>
    /// Resolves a user DTO by the identity present in the current context.
    /// Throws if not found.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="userIdentity">Identity (usually from HttpContext.User.Identity).</param>
    /// <returns>User DTO for the resolved user.</returns>
    Task<UserGetDto> GetUser(CancellationToken cancellation, IIdentity? userIdentity);

    /// <summary>
    /// Checks that the user associated with the given identity has the required role.
    /// Throws if the user does not have the valid role.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="userIdentity">Identity (usually from HttpContext.User.Identity).</param>
    /// <param name="validType">Required user type (role).</param>
    Task ValidateRole(CancellationToken cancellation, IIdentity? userIdentity, UserType validType);

    /// <summary>
    /// Checks that the given user DTO has the required role.
    /// Throws if the user does not have the valid role.
    /// </summary>
    /// <param name="user">User DTO.</param>
    /// <param name="validType">Required user type (role).</param>
    void ValidateRole(UserGetDto? user, UserType validType);
}

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IUsersRepository users)
    : IAuthService
{
    /// <inheritdoc/>
    public async Task<UserGetDto> Register(CancellationToken cancellation, UserCreateDto userData)
    {
        User? user = userData.UserType switch
        {
            UserType.HIRING_MANAGER => new HiringManager
            {
                UserName = userData.Email,
                Email = userData.Email,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Department = userData.Department
            },
            UserType.RECRUITER => new Recruiter
            {
                UserName = userData.Email,
                Email = userData.Email,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Type = userData.Type
            },
            _ => null
        };

        if (user == null) throw new BadRequest("Invalid user type");

        var result = await userManager.CreateAsync(user, userData.Password);

        if (result.Succeeded)
            return new UserGetDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

        throw new BadRequest(result.Errors.First().Description);
    }

    /// <inheritdoc />
    public async Task Login(CancellationToken cancellation, UserLoginDto userData)
    {
        var result = await signInManager.PasswordSignInAsync(
            userData.Email,
            userData.Password,
            false, // Remember me.
            true);

        if (result.Succeeded) return;

        if (result.IsLockedOut) throw new BadRequest("Account locked out");

        throw new BadRequest("Invalid login attempt");
    }

    /// <inheritdoc />
    public async Task<UserGetDto> GetUser(CancellationToken cancellation, IIdentity? userIdentity)
    {
        return await users.GetUserByEmail(cancellation, userIdentity?.Name);
    }

    /// <inheritdoc />
    public async Task ValidateRole(CancellationToken cancellation, IIdentity? userIdentity, UserType validType)
    {
        var user = await users.GetUserByEmail(cancellation, userIdentity?.Name);

        ValidateRole(user, validType);
    }

    /// <inheritdoc />
    public void ValidateRole(UserGetDto? user, UserType validType)
    {
        if (user?.UserType != validType) throw new BadRequest("Invalid permissions");
    }
}