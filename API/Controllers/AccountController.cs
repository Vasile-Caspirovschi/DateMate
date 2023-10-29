using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.Include(p => p.Photos)
                .SingleOrDefaultAsync(user => user.UserName == loginDTO.Username!.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password!, false);
            if (!result.Succeeded) return Unauthorized("Invalid password");

            var users = new UserDTO
            {
                Username = loginDTO.Username,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                Gender = user.Gender,
                KnownAs = user.KnownAs
            };
            return users;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username!)) return BadRequest("Username is taken.");

            var user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username!.ToLower();

            var result = await _userManager.CreateAsync(user, registerDTO.Password!);
            if (!result.Succeeded) return BadRequest(result.Errors);
            result = await _userManager.AddToRoleAsync(user, "Member");
            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = registerDTO.Username,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        private Task<bool> UserExists(string username)
        {
            return _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}
