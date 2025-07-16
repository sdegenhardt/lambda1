using System.Reflection;

namespace AwsLambda2Host.Endpoints.Versions;

public class GetVersionsEndpoint : IEndpoint
{
    private const string Route = "versions";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Route, () =>
        {
            var assembly = Assembly.GetEntryAssembly();
            var informationalVersion = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;
            var segments = informationalVersion?.Split('+') ?? [];
            var version = segments.Length >= 1 ? segments[0] : string.Empty;
            var gitCommitHash = segments.Length >= 2 ? segments[1] : string.Empty;
            return TypedResults.Ok(new { version, gitCommitHash });
        }).WithName("GetVersion");
    }
}
