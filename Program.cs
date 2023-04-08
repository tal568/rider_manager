using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rider_manager;
using rider_manager.Interfaces;
using rider_manager.servises;
using Serilog;
using whatsapp_ride_joiner;
using WorkerService1;

var configurationBuilder = new ConfigurationBuilder();
BuildConfig(configurationBuilder);

Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configurationBuilder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
Log.Logger.Information("Application Starting");

//after create the builder - UseSerilog



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<INlpMessages, NlpMessages>();
        services.AddSingleton<IRideManger, RideManger>();
        services.AddSingleton<IMyWebDriver, MyWebDriver>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Prod"}.json", optional: true)
        .AddEnvironmentVariables();
}










