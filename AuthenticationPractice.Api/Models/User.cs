using System.Security.Claims;

namespace AuthenticationPractice.Api.Models;

public record User(
    string Username,
    string Password,
    IEnumerable<Claim> Claims
);
