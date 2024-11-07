using AuthProviderRika_V2.Contexts;
using AuthProviderRika_V2.Entities;
using AuthProviderRika_V2.Handlers;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("USER_IDENTITY_SQL")));


builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = true;
    x.SignIn.RequireConfirmedEmail = true;
    x.Password.RequiredLength = 8;
    x.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<ServiceBusSender>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
    var queueName = builder.Configuration["ServiceBus:SenderQueue"];
    var client = new ServiceBusClient(connectionString);
    return client.CreateSender(queueName);
});

//om vi vill att hantera role

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    string[] roles = ["Admin", "superAdmin", "Manager", "User"];
//    foreach (string role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }

//    }

//}




builder.Services.AddSingleton<ServiceBusHandler>();
builder.Services.AddHostedService(x => x.GetRequiredService<ServiceBusHandler>());

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

