using API.Data;
using API.Extensions;
using API.Middleware;
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
	await dataContext.Database.MigrateAsync();
	await UserSeed.SeedUsers(dataContext);
}
catch (Exception ex)
{
	var logger = app.Services.GetRequiredService<ILogger<Program>>();
	logger.LogError(ex, "An error occurred during migration and seeding data");
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
