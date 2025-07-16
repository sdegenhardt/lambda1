namespace UseCases;

public class GetForecastHandler : IQueryHandler<NoQuery, GetWeatherForecastResponse[]>
{
    private readonly string[] _summaries = [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task<GetWeatherForecastResponse[]> Handle(NoQuery _, CancellationToken cancellationToken)
    {
        var forecast =  Enumerable.Range(1, 5).Select(index =>
                new GetWeatherForecastResponse
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray();

        return await Task.FromResult(forecast);
    }
}

public record GetWeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
