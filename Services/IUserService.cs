using LoginApplication.DTOs;
using LoginApplication.Models;

namespace LoginApplication.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserLoginDto userDto);
        Task<string> LoginAsync(UserLoginDto userDto);
    }
}
