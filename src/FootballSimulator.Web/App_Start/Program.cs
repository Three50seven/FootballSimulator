using FootballSimulator.Web;

var builder = WebApplication.CreateBuilder(args);

// configure NLog, when using Windows OS
if (OperatingSystem.IsWindows())
{
    builder.Logging.Configure(builder.Configuration);
}

builder.Services.Register(builder.Configuration, builder.Environment);

var app = builder.Build();

// register all request pipeline events and middleware
app.ConfigureAppPipeline();

await app.RunAsync();