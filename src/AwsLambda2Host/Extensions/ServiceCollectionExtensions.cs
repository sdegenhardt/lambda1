using System.Reflection;
using AwsLambda2Host.Endpoints.WeatherForecast;
using UseCases.Extensions;

namespace AwsLambda2Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        services.AddTransientServices(assembly, typeof(IEndpoint), "Endpoint");
        return services;
    }
}
