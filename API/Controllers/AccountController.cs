using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController: ControllerAPIBase
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(user => user.Username == loginDTO.Username!.ToLower());

            if(user is null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt!);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password!));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash![i]) return Unauthorized("Invalid password.");

            return new UserDTO
            {
                Username = loginDTO.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username!)) return BadRequest("Username is taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                Username = registerDTO.Username!.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password!)),
                PasswordSalt = hmac.Key
            };

            _dataContext.Add(user); 
            await _dataContext.SaveChangesAsync();

            return new UserDTO
            {
                Username = registerDTO.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private Task<bool> UserExists(string username)
        {
            return _dataContext.Users.AnyAsync(user => user.Username == username.ToLower());
        }
    }
}
