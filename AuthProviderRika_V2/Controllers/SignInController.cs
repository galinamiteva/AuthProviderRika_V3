using AuthProviderRika_V2.Entities;
using AuthProviderRika_V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthProviderRika_V2.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class SignInController(SignInManager<UserEntity> signInManager) : ControllerBase
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    [HttpPost]
    public async Task<IActionResult> SignIn(SignIn model)
    {
        if (ModelState.IsValid)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

            if (user != null && !user.EmailConfirmed)
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }
        return BadRequest();
    }
}