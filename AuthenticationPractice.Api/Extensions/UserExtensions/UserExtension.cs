using System.Security.Claims;
using AuthenticationPractice.Api.Identity;
using AuthenticationPractice.Api.Models;

namespace AuthenticationPractice.Api.Extensions.UserExtensions;

public static class UserServiceExtensions
{
    public static IServiceCollection AddInMemoryUser(this IServiceCollection services)
    {
        var inMemoryUser = new User(
            Username: "EN",
            Password: "any12345",
            Claims:
            [
                new Claim(IdentityPolicyConstants.AdminUserClaimName, "true")
            ]
        );

        services.AddSingleton(inMemoryUser);

        return services;
    }
}
