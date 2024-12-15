using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthenticationPractice.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationPractice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly User _inMemoryUser;
    private readonly IConfiguration _configuration;

    public AuthController(User inMemoryUser, IConfiguration configuration)
    {
        _inMemoryUser = inMemoryUser;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username != _inMemoryUser.Username || request.Password != _inMemoryUser.Password)
            return Unauthorized("Invalid credentials.");

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var key = jwtSettings["Key"];

        // Check if any JWT setting is null or empty
        if (string.IsNullOrWhiteSpace(issuer))
        {
            Console.WriteLine("Issuer is null or empty");
        }
        if (string.IsNullOrWhiteSpace(audience))
        {
            Console.WriteLine("Audience is null or empty");
        }
        if (string.IsNullOrWhiteSpace(key))
        {
            Console.WriteLine("Key is null or empty");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: _inMemoryUser.Claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = jwt });
    }

    public record LoginRequest(string Username, string Password);
}
