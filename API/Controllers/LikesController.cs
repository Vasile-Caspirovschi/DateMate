using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser is null) return NotFound();
            if (username == sourceUser.UserName) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike is not null) return BadRequest("You already liked this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers?.Add(userLike);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var likedUsers = await _likesRepository.GetUsersLikes(likesParams);

            Response.AddPaginationHeader(likedUsers.CurrentPage, likedUsers.PageSize, likedUsers.TotalCount, likedUsers.TotalPages);
            return Ok(likedUsers);
        }
    }
}