using System.Text.Json;
using AwsLambda2Host.Endpoints.WeatherForecast;
using UseCases;

namespace AwsLambda2Host.IntegrationTests;

public class GetWeatherForecastFixture : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public GetWeatherForecastFixture(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/WeatherForecast", 5)]
    public async Task WeatherForcast_Get_ReturnsOk(string path, int expectedItems)
    {
        // Act
        var response = await _httpClient.GetAsync(path);

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetWeatherForecastResponse[]>(json);
        Assert.Equal(expectedItems, result?.Length ?? 0);
    }
}
