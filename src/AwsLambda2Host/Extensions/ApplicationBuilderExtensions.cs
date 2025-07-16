using AwsLambda2Host.Endpoints.WeatherForecast;

namespace AwsLambda2Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
