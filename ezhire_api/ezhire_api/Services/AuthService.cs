using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using Microsoft.AspNetCore.Identity;

namespace ezhire_api.Services;

public interface IAuthService
{
    Task<UserGetDto> Register(CancellationToken cancellation, UserCreateDto userData);
    Task Login(CancellationToken cancellation, UserLoginDto userData);
}

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
    : IAuthService
{
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

            if (user == null)
            {
                throw new BadRequest("Invalid user type");
            }
            
            var result = await userManager.CreateAsync(user, userData.Password);

            if (result.Succeeded)
            {
                return new UserGetDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
            }

            throw new BadRequest(result.Errors.First().Description);
    }

    public async Task Login(CancellationToken cancellation, UserLoginDto userData)
    {
        var result = await signInManager.PasswordSignInAsync(
            userData.Email,
            userData.Password,
            false, // Remember me.
            lockoutOnFailure: true);
        
        if (result.Succeeded) return;
        
        if (result.IsLockedOut)
        {
            throw new BadRequest("Account locked out");
        }
        
        throw new BadRequest("Invalid login attempt");
    }
}