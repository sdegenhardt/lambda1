namespace AwsLambda2Host.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var x = new WeatherForecast(new DateOnly(2025, 7, 11), 36, "Chicago - Balmy");

        // Assert
        Assert.Equal(96, x.TemperatureF);
    }
}
