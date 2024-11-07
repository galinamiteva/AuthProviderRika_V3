

using AuthProviderRika_V2.Controllers;
using AuthProviderRika_V2.Entities;
using AuthProviderRika_V2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuthProvider.Tests;

public class SignInControllerTests
{
    private readonly Mock<UserManager<UserEntity>> mockUserManager;
    private readonly Mock<SignInManager<UserEntity>> mockSignInManager;
    private readonly SignInController controller;

    public SignInControllerTests()
    {
        mockUserManager = new Mock<UserManager<UserEntity>>(
            new Mock<IUserStore<UserEntity>>().Object,
            null, null, null, null, null, null, null, null, null, null);

        mockSignInManager = new Mock<SignInManager<UserEntity>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<UserEntity>>().Object,
            null, null, null, null);

        controller = new SignInController(mockUserManager.Object, mockSignInManager.Object);
    }

    [Fact]
    public async Task IndexPost_InvalidModelState_ReturnsViewWithModel()
    {
        var model = new SignIn
        {
            Email = "invalid-email",
            Password = "12345",
            
        };
        controller.ModelState.AddModelError("Email", "Invalid email format");

        var result = await controller.SignIn(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
       // Assert.IsType<SignIn>(viewResult.Model);
    }

   

    [Fact]
    public async Task IndexPost_InvalidCredentials_ReturnsViewWithModelError()
    {
        var model = new SignIn
        {
            Email = "user@example.com",
            Password = "WrongPassword!",
            
        };

        var user = new UserEntity { UserName = model.Email, Email = model.Email };
        mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        mockSignInManager.Setup(sim => sim.CheckPasswordSignInAsync(user, model.Password, false))
                         .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var result = await controller.SignIn(model);

       //var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
        Assert.Equal("You have entered an invalid username or password!", controller.ModelState[""]!.Errors[0].ErrorMessage);
        //Assert.IsType<SignIn>(viewResult.Model);
    }

   
}
