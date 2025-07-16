using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UseCases.Abstractions;

namespace UseCases.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var t = typeof(IQueryHandler<,>);
        return services.AddScopedServices2(assembly, t, "Handler");
    }

    private static IServiceCollection AddScopedServices2(this IServiceCollection services, Assembly assembly, Type interfaceType, string classNameEndsWith)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        ArgumentNullException.ThrowIfNull(interfaceType, nameof(interfaceType));
        ArgumentNullException.ThrowIfNull(classNameEndsWith, nameof(classNameEndsWith));

        var serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsClass: true, IsInterface: false, FullName: not null }
                           && type.FullName.EndsWith(classNameEndsWith, StringComparison.OrdinalIgnoreCase)
                           && type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == interfaceType))
            .ToArray();

        foreach (var item in serviceDescriptors)
        {
            foreach (var handler in item.GetInterfaces())
            {
                if (handler.IsGenericType && handler.GetGenericTypeDefinition() == interfaceType)
                {
                    services.AddScoped(handler, item);
                }
            }
        }

        return services;
    }

    public static IServiceCollection AddScopedServices(this IServiceCollection services, Assembly assembly, Type interfaceType, string classNameEndsWith)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        ArgumentNullException.ThrowIfNull(interfaceType, nameof(interfaceType));
        ArgumentNullException.ThrowIfNull(classNameEndsWith, nameof(classNameEndsWith));

        var types = assembly.GetTypes()
            .Where(interfaceType.IsAssignableFrom)
            .Where(type =>
                type.FullName != null &&
                type.FullName != interfaceType.FullName &&
                type.FullName.EndsWith(classNameEndsWith, StringComparison.OrdinalIgnoreCase));

        foreach (var t in types)
        {
            services.AddScoped(interfaceType, t);
        }

        return services;
    }

    public static IServiceCollection AddTransientServices(this IServiceCollection services, Assembly  assembly, Type interfaceType, string classNameEndsWith)
    {
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        ArgumentNullException.ThrowIfNull(interfaceType, nameof(interfaceType));
        ArgumentNullException.ThrowIfNull(classNameEndsWith, nameof(classNameEndsWith));

        var serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false, FullName: not null }
                           && type.FullName.EndsWith(classNameEndsWith, StringComparison.OrdinalIgnoreCase)
                           && type.IsAssignableTo(interfaceType))
            .Select(type => ServiceDescriptor.Transient(interfaceType, type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
}
