using AuthProviderRika_V2.Contexts;
using AuthProviderRika_V2.Entities;
using AuthProviderRika_V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AuthProviderRika_V2.Controllers;

//[Route("api/[controller]")]
[Route("api/auth/[controller]")]
[ApiController]
public class VerificationController(DataContext context, UserManager<UserEntity> userManager, HttpClient httpClient) : ControllerBase
{
    private readonly DataContext _context = context;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly HttpClient _httpClient = httpClient;


    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail(VerificationMessage model)
     {
        var apiUrl = "https://verificationprovider.azurewebsites.net/api/verify?code=jLihlJW42fADXRAA8zX7q0vMtutF8ZRACEp9AqhUwoO6AzFu91kZ6A%3D%3D";


        var response = await _httpClient.PostAsJsonAsync(apiUrl, model);

        if (response.IsSuccessStatusCode)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user != null && !user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return StatusCode((int)response.StatusCode);
        }
        else
        {
            return StatusCode((int)response.StatusCode);
        }
    }



}
