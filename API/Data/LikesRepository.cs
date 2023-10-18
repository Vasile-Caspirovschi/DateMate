using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dataContext;

        public LikesRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            var userLike = await _dataContext.Likes.FindAsync(sourceUserId, likedUserId);
            return userLike!;
        }

        public async Task<IEnumerable<LikeDto>> GetUsersLikes(string predicate, int userId)
        {
            var users = _dataContext.Users.OrderBy(user => user.Username).AsQueryable();
            var likes = _dataContext.Likes.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.LikedUser)!;
            }

            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == userId);
                users = likes.Select(like => like.SourceUser)!;
            }

            return await users.Select(user => new LikeDto
            {
                Username = user.Username,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos!.FirstOrDefault(photo => photo.IsMain)!.Url,
                City = user.City,
                Id = user.Id
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            var user = await _dataContext.Users.Include(user => user.LikedUsers)
                .FirstOrDefaultAsync(user => user.Id == userId);
            return user!;
        }
    }
}