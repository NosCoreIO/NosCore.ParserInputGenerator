using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NosCore.ParserInputGenerator.Downloader;
using NosCore.ParserInputGenerator.Extractor;
using NosCore.ParserInputGenerator.Launcher.Configuration;
using NosCore.Shared.Configuration;
using Serilog;

namespace NosCore.ParserInputGenerator.Launcher
{
    /// <summary>
    /// Main program entry point for the Parser Input Generator launcher.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates and configures the host builder.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The configured host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ParserInputGeneratorConfiguration();
            ConfiguratorBuilder.InitializeConfiguration(args, new[] { "logger.yml", "parser-input-generator.yml" }).Bind(configuration);
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(
                    loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog(dispose: true);
                    }
                )
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(configuration);
                    services.AddTransient<IExtractor, Extractor.Extractor>();
                    services.AddTransient<IClientDownloader, ClientDownloader>();
                    services.AddHttpClient();
                    services.AddHostedService<Worker>();
                }); 
        }
    }
}
