namespace AwsLambda2.Host.Tests;

public class GetForecastHandlerFixture
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var x = new GetWeatherForecastResponse(new DateOnly(2025, 7, 11), 36, "Chicago - Balmy");

        // Assert
        Assert.Equal(96, x.TemperatureF);
    }
}
