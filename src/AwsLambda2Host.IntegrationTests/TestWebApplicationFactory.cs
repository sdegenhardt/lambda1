using Microsoft.AspNetCore.Mvc.Testing;

namespace AwsLambda2Host.IntegrationTests;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
        });

        builder.ConfigureServices(services =>
        {
        });

        return base.CreateHost(builder);
    }
}
