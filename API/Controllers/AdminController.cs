using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public ActionResult GetUsersAndRoles()
        {
            var users = _userManager.Users.Include(user => user.UserRoles)
                .ThenInclude(role => role.Role)
                .OrderBy(user => user.UserName)
                .Select(user => new
                {
                    user.Id,
                    Username = user.UserName,
                    Roles = user.UserRoles.Select(role => role.Role.Name).ToList()
                }).ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(',').ToArray();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Failed to configure user roles");
            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!result.Succeeded) return BadRequest("Failed to configure user roles");

            result = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!result.Succeeded) return BadRequest("Failed to configure user roles");
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}