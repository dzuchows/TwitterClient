using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitterClient;
using TwitterClient.TweetReporter;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ITweetRepository, TweetRepository>();
        services.AddHttpClient<ITweetCollectorService, TweetCollectorService>();

        services.AddHostedService<TweetCollectorBackgroundService>();
        services.AddScoped<ITweetCollectorService, TweetCollectorService>();
        
        services.AddHostedService<TweetReporterBackgroundService>();
        services.AddScoped<ITweetReporterService, ConsoleTweetReporterService>();

    })
    .Build();

await host.RunAsync();


