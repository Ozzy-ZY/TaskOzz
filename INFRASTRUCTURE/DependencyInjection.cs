using System.Text;
using INFRASTRUCTURE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace INFRASTRUCTURE;

// public class JwtEvents
// {
//     private readonly ILogger<JwtEvents> _logger;
//
//     public JwtEvents(ILogger<JwtEvents> logger)
//     {
//         _logger = logger;
//     }
//
//     public Task OnAuthenticationFailed(AuthenticationFailedContext context)
//     {
//         _logger.LogError("Authentication failed: {Exception}", context.Exception);
//         return Task.CompletedTask;
//     }
//
//     public Task OnTokenValidated(TokenValidatedContext context)
//     {
//         _logger.LogInformation("Token validated successfully");
//         return Task.CompletedTask;
//     }
//
//     public Task OnMessageReceived(MessageReceivedContext context)
//     {
//         _logger.LogInformation("Received token: {Token}", context.Token);
//         return Task.CompletedTask;
//     }
//
//     public Task OnChallenge(JwtBearerChallengeContext context)
//     {
//         _logger.LogWarning("Challenge issued: {Error}, {ErrorDescription}", 
//             context.Error, context.ErrorDescription);
//         return Task.CompletedTask;
//     }
// }

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:ValidIssuer"],
            ValidAudience = configuration["Jwt:ValidAudience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
        };

        //services.AddScoped<JwtEvents>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
                // var serviceProvider = services.BuildServiceProvider();
                // var jwtEvents = serviceProvider.GetRequiredService<JwtEvents>();
                //
                // options.Events = new JwtBearerEvents
                // {
                //     OnAuthenticationFailed = jwtEvents.OnAuthenticationFailed,
                //     OnTokenValidated = jwtEvents.OnTokenValidated,
                //     OnMessageReceived = jwtEvents.OnMessageReceived,
                //     OnChallenge = jwtEvents.OnChallenge
                // };
            });

        services.AddAuthorization();
        services.AddSingleton(tokenValidationParameters);
        
        return services;
    }
}