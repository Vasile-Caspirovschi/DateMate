using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext dataContext, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _dataContext.Users.Include(p => p.Photos)
                .SingleOrDefaultAsync(user => user.UserName == loginDTO.Username!.ToLower());

            return new UserDTO
            {
                Username = loginDTO.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                Gender = user.Gender,
                KnownAs = user.KnownAs 
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username!)) return BadRequest("Username is taken.");

            var user = _mapper.Map<AppUser>(registerDTO);
            using var hmac = new HMACSHA512();

            user.UserName = registerDTO.Username!.ToLower();

            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();

            return new UserDTO
            {
                Username = registerDTO.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        private Task<bool> UserExists(string username)
        {
            return _dataContext.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}
