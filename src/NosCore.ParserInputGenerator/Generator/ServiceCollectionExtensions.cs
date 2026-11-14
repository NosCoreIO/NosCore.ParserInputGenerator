//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using Microsoft.Extensions.DependencyInjection;
using NosCore.ParserInputGenerator.Downloader;
using NosCore.ParserInputGenerator.Extractor;

namespace NosCore.ParserInputGenerator.Generator
{
    /// <summary>
    /// Registers the parser input generator.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="ParserInputBuilder"/> and what it needs.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddParserInputGenerator(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<ClientDownloader>();
            services.AddTransient<IClientDownloader>(sp => sp.GetRequiredService<ClientDownloader>());
            services.AddTransient<IExtractor, Extractor.Extractor>();
            services.AddTransient<ParserInputBuilder>();
            return services;
        }
    }
}
