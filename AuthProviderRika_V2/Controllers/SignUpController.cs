using AuthProviderRika_V2.Entities;
using AuthProviderRika_V2.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthProviderRika_V2.Controllers;
[Route("api/auth/[controller]")]
[ApiController]
public class SignUpController(UserManager<UserEntity> userManager, ServiceBusSender sender) : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly ServiceBusSender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUp model)
    {
        if (ModelState.IsValid)
        {
            if (!await _userManager.Users.AnyAsync(x => x.Email == model.Email))
            {
                var identityUser = new UserEntity
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,                   
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    return await SendVerificationCodeAsync(model.Email);
                }

            }

            return Conflict();
        }
        return BadRequest();
    }


    [HttpPost("verificationCode")]
    public async Task<IActionResult> SendVerificationCodeAsync(string email)
    {
        try
        {
            var verificationRequest = new VerificationRequest
            {
                Email = email
            };

            var jsonMessage = JsonConvert.SerializeObject(verificationRequest);

            var serviceBusMessage = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(serviceBusMessage);

            return Ok();

        }
        catch (ServiceBusException ex)
        {
            Console.WriteLine($"Service Bus error: {ex.Message}");
            return StatusCode(500, "Error sending verification code-servicebus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending verification code: {ex.Message}");
            return StatusCode(500, "Error sending verification code");
        }
    }
}
