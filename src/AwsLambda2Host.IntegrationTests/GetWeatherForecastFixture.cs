using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UseCases;
using Xunit;

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
