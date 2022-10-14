using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using whatapp_ride_joiner;
using Log = Serilog.Log;

namespace DependecyInj
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IRideManger, RideManger>();


                })
                .UseSerilog()
                .Build();

            var ride_manger = ActivatorUtilities.CreateInstance<RideManger>(host.Services);

            int wait_sec = builder.Build().GetValue<int>("wait_sec");
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(wait_sec));
            Log.Logger.Information("wating for next tik in {sec}", wait_sec);
            while (await timer.WaitForNextTickAsync())
            {
                try
                {
                    bool found_ride = ride_manger.LoadMassages()
                    .ChoseMassage()
                    .ReplayToChosenMassage();


                    if (found_ride)
                        timer.Dispose();
                }
                catch (Exception e)
                {
                    Log.Logger.Error("failed to run " + e.Message + "\n" + e.StackTrace);
                }


            }
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

        }
    }
}


