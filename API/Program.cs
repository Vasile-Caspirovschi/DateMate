using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSignalR();

var clientOrigin = "DatingAppAngularClient";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: clientOrigin,
	policy =>
	{
		policy.AllowCredentials();
		policy.AllowAnyHeader();
		policy.WithOrigins("http://localhost:4200");
	});
});

var app = builder.Build();

try
{
	var scope = app.Services.CreateScope();
	var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
	await dataContext.Database.MigrateAsync();
	await UserSeed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
	var logger = app.Services.GetRequiredService<ILogger<Program>>();
	logger.LogError(ex, "An error occurred during migration and seeding data");
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(clientOrigin);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<PresenceHub>("hubs/message");

app.Run();
