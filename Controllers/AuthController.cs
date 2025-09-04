using LoginApplication.Data;
using LoginApplication.DTOs;
using LoginApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
       private readonly IUserService _userService;  
        private readonly AppDbContext _context;
        public AuthController(IUserService userService, AppDbContext context)
         {
              _userService = userService;
              _context= context;
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
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==dto.Email);
                if(user==null )
                {
                    return Unauthorized();
                }
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
