using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    [Theory]
    [InlineData("/Versions")]
    public async Task Versions_Get_ReturnsOk(string path)
    {
        // Act
        var response = await _httpClient.GetAsync(path);

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        Assert.True(json?.Length > 0);
        var result = JsonSerializer.Deserialize<VersionDto>(json);
        Assert.NotNull(result);
    }
}

internal class VersionDto
{
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("gitCommitHash")]
    public string GitCommitHash { get; set; }
}
