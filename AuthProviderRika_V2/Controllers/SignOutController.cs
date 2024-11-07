using AuthProviderRika_V2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthProviderRika_V2.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class SignOutController(SignInManager<UserEntity> signInManager) : ControllerBase
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
        Response.Cookies.Delete("AccessToken");
        await _signInManager.SignOutAsync();
        return Ok();
    }
}
