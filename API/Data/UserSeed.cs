using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data
{
    public class UserSeed
    {
        public static async Task SeedUsers(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach (var user in users!)
            {
                user.UserName = user.UserName!.ToLower();
                dataContext.Users.Add(user);
            }
            await dataContext.SaveChangesAsync();
        }
    }
}
