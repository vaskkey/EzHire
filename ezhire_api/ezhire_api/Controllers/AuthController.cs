using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ezhire_api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(CancellationToken cancellation, [FromBody] UserLoginDto userData)
    {
        await auth.Login(cancellation, userData);
        return Ok(new { Message = "Success." });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CancellationToken cancellation, [FromBody] UserCreateDto userData)
    {
        return Ok(await auth.Register(cancellation, userData));
    }
}