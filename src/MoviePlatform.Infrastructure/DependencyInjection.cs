using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MoviePlatform.Application.Common.Authentication;
using MoviePlatform.Infrastructure.Authentication;
using MoviePlatform.Infrastructure.Persistence;

namespace MoviePlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddSingleton<IJwtProvider, JwtProvider>();
		services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

		IConfigurationSection jwtConfigurationSection = configuration.GetSection("JwtSettings");
		JwtOptions jwtOptions = jwtConfigurationSection.Get<JwtOptions>()
			?? throw new InvalidOperationException("JWT configuration is missing or invalid.");

		services.Configure<JwtOptions>(jwtConfigurationSection);
		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
				};
			});

		services.AddAuthorization();

		services.AddDbContext<MoviePlatformDbContext>(
			options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")
					?? throw new InvalidOperationException("Database connection string is missing or invalid.")));

		return services;
    }
}
