using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Identity.Client;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models.Common;
using TaskManagerAPI.Models.DTOs;
using TaskManagerAPI.Models.Entities;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Users> _signinManager;
        public UserController(ApplicationDBContext context, UserManager<Users> userManager, ITokenService tokenService, SignInManager<Users> signinManager)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _signinManager = signinManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid email");
                }
                var result = await _signinManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
                if (result.Succeeded)
                {
                    return Ok(new NewUserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Token = _tokenService.CreateToken(user)
                    });
                }
                else
                {
                    return Unauthorized("Invalid email or password.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdUser = new Users
                {
                    UserName = registerUserDto.Username,
                    Email = registerUserDto.Email
                };
                var createUserResult = await _userManager.CreateAsync(createdUser, registerUserDto.Password);
                if (createUserResult.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(createdUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            Id = createdUser.Id,
                            Email = createdUser.Email,
                            UserName = createdUser.UserName,
                            Token = _tokenService.CreateToken(createdUser)
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else {
                    return StatusCode(500, createUserResult.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}
