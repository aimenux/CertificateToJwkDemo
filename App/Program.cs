using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

public static class Program
{
    public static void Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        var converters = host.Services.GetServices<ICertificateConverter>();
        foreach (var converter in converters)
        {
            ConsoleColor.Green.WriteLine($"Using {converter.GetType().Name}");
            var jwk = converter.FromCertificateToJwk();
            ConsoleColor.Yellow.WriteLine($"JWK = {jwk}\n");
        }

        Console.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.AddTransient<ICertificateConverter, CertificateConverterWayOne>();
                services.AddTransient<ICertificateConverter, CertificateConverterWayTwo>();
                services.Configure<CertificateConverterSettings>(hostingContext.Configuration.GetSection("Settings"));
            })
            .UseConsoleLifetime();
}