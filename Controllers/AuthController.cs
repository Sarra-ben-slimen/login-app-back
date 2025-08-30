using LoginApplication.DTOs;
using LoginApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginApplication.Controllers
{
    public class AuthController : Controller
    {
       private readonly IUserService _userService;  

        public AuthController(IUserService userService)
         {
              _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserLoginDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new {user.Email, user.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        [HttpPost("login")] 
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            try
            {
                var token = await _userService.LoginAsync(dto);
                return Ok(new {token});
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}
