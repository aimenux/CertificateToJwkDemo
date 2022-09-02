using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;

namespace App
{
    public static class Extensions
    {
        public static void WriteLine(this ConsoleColor color, object value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static string GetSecurityAlgorithm(this X509Certificate2 certificate)
        {
            var algorithm = certificate.SignatureAlgorithm;
            return algorithm.Value switch
            {
                "1.2.840.113549.1.1.11" => SecurityAlgorithms.RsaSha256,
                "1.2.840.113549.1.1.12" => SecurityAlgorithms.RsaSha384,
                "1.2.840.113549.1.1.13" => SecurityAlgorithms.RsaSha512,
                _ => throw new InvalidOperationException($"Unsupported algorithm {algorithm.Value} ({algorithm.FriendlyName})"),
            };
        }

        public static void AddJsonFile(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(GetDirectoryPath());
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
        }

        public static void AddUserSecrets(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddUserSecrets(typeof(Program).Assembly);
        }

        public static void AddConsoleLogger(this ILoggingBuilder loggingBuilder)
        {
            if (File.Exists(GetSettingFilePath()))
            {
                loggingBuilder.AddConsole();
            }
            else
            {
                loggingBuilder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = true;
                    options.UseUtcTimestamp = true;
                    options.TimestampFormat = "[HH:mm:ss:fff] ";
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                });
            }
        }

        private static string GetSettingFilePath() => Path.GetFullPath(Path.Combine(GetDirectoryPath(), @"appsettings.json"));

        private static string GetDirectoryPath()
        {
            try
            {
                return Path.GetDirectoryName(GetAssemblyLocation())!;
            }
            catch
            {
                return Directory.GetCurrentDirectory();
            }
        }

        private static string GetAssemblyLocation() => Assembly.GetExecutingAssembly().Location;
    }
}
