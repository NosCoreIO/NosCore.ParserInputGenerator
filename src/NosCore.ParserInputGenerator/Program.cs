﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NosCore.ParserInputGenerator.Configuration;
using NosCore.Shared.Configuration;
using Serilog;

namespace NosCore.ParserInputGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ParserInputGeneratorConfiguration();
            ConfiguratorBuilder.InitializeConfiguration(args, new[] { "logger.yml", "parser-input-generator.yml" }, configuration);
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
                    services.AddHostedService<Worker>();
                });
        }
    }
}
