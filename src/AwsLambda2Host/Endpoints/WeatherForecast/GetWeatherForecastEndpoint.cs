namespace AwsLambda2Host.Endpoints.WeatherForecast;

public class GetWeatherForecastEndpoint : IEndpoint
{
    private const string Route = "weatherforecast";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(Route, async Task<Ok<GetWeatherForecastResponse[]>> (
            [FromServices] IQueryHandler<NoQuery, GetWeatherForecastResponse[]> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(NoQuery.Instance, cancellationToken);
            return TypedResults.Ok(result);
        }).WithName("GetWeatherForecast");
    }
}
