using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddIdentityServices(builder.Configuration);

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

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
