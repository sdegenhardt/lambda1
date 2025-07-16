using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Extensions;

namespace AwsLambda2Host.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        services.AddTransientServices(assembly, typeof(IEndpoint), "Endpoint");
        return services;
    }
}
