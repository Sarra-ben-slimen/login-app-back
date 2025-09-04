
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LoginApplication.DTOs;
using LoginApplication.Models;
using LoginApplication.Services;
using LoginApplication.Repositories;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly string _jwtKey;
    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtKey = configuration["Jwt:key"];
    }

    public async Task<User> RegisterAsync(UserLoginDto dto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(dto.Email);
        if (existingUser != null) throw new Exception("User already exists");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password)
        };

        await _userRepository.AddUserAsync(user);
        return user;
    }

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
      

       var user = await _userRepository.GetByUsernameAsync(dto.Email);
        string test = HashPassword(dto.Password);
        if (user == null || user.PasswordHash != HashPassword(dto.Password))
            throw new Exception("Invalid username or password");

        return GenerateJwtToken(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private string GenerateJwtToken(User user)
    {
        var Claims = new List<Claim> 
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.role)
        };
       var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: Claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
            );

         return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
