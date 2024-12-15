using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationPractice.Api.Controllers;

[ApiController]
[Authorize] // If commented out, anyone can access this endpoint; otherwise, a valid token is needed.
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    // [Authorize(Policy = IdentityPolicyConstants.AdminUserPolicyName)] // Uncomment this if you want admin-only access
    public IEnumerable<WeatherForecast> Get()
    {
        
            var userName = User.Identity.Name ?? "NoNameClaim";
            _logger.LogInformation("User is authenticated. Username: {UserName}", userName);

            // Print all claims
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation("ClaimType: {Type}, ClaimValue: {Value}", claim.Type, claim.Value);
            }
        

        // Return the weather as before
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}