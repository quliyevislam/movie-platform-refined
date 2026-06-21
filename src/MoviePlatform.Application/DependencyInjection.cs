using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MoviePlatform.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
		services.AddSingleton(TimeProvider.System);

		return services;
	}
}
