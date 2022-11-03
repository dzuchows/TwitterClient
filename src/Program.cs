using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterClient;
using TwitterClient.Reporter;
using TwitterClient.TweetCollector;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IReportWriter, ConsoleReportWriter>();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ITweetRepository, TweetRepository>();
        services.AddHttpClient<ITweetCollectorService, TweetCollectorService>();

        services.AddHostedService<TweetCollectorBackgroundService>();
        services.AddScoped<ITweetCollectorService, TweetCollectorService>();

        services.AddHostedService<ReporterBackgroundService>();
        services.AddScoped<IReporterService, TopNHashTagReporterService>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();