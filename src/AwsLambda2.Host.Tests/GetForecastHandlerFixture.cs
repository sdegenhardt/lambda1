using UseCases.Abstractions;

namespace AwsLambda2.Host.Tests;

public class GetForecastHandlerFixture
{
    [Fact]
    public async Task GetForcasts()
    {
        // Arrange
        var sut = new GetForecastHandler();
        var result = await sut.Handle(NoQuery.Instance, CancellationToken.None);

        // Assert
        Assert.Equal(5, result.Length);
    }

    [Fact]
    public void ResponseCovertsToF()
    {
        // Arrange
        var x = new GetWeatherForecastResponse(new DateOnly(2025, 7, 11), 36, "Chicago - Balmy");

        // Assert
        Assert.Equal(96, x.TemperatureF);
    }
}
