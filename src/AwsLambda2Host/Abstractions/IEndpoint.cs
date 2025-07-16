using Microsoft.AspNetCore.Routing;

namespace AwsLambda2Host.Abstractions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder routeBuilder);
}
