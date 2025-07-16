using System.Reflection;
using AwsLambda2Host.Extensions;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddQueryHandlers(Assembly.GetAssembly(typeof(IQueryHandler<,>))!);
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();
app.UseHttpsRedirection();
app.MapEndpoints();
app.Run();

public partial class Program
{ }
