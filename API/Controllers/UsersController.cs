using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
       private readonly IUserRepository _userRepository;
       private readonly ITokenService _tokenService;
       public UsersController(IUserRepository userRepository, ITokenService tokenService) 
       {
          _userRepository = userRepository;
          _tokenService = tokenService;
       }
       // [AllowAnonymous] 
       [Authorize]
       [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
       {
         return Ok(await _userRepository.GetUsersAsync());
       }

       [Authorize]
       [HttpGet("{username}")]
       public async Task<ActionResult<AppUser>> GetUser(string username)
       {
          return await _userRepository.GetUserByUsernameAsync(username);
       }

       [HttpPost("login")]
       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       {
          var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
          if (user == null) return Unauthorized("Invalid username");
          if (loginDto.Username != loginDto.Password) return Unauthorized("Invalid password");
          return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
        };
       }

    }        
}