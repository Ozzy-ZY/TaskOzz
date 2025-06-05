using APPLICATION.Services;
using APPLICATION.Validator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FluentValidation;
using FluentValidation.Internal;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;

namespace APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddScoped<AuthService>();
        services.AddScoped<PasswordService>();
        services.AddScoped<JwtService>();
        services.AddScoped<ImageService>();
        services.AddScoped<TaskService>();
        services.AddFluentValidationAutoValidation(autoValidationMvcConfiguration =>
        {
            autoValidationMvcConfiguration.ValidationStrategy = ValidationStrategy.All;
        });
        return services;
    }

}