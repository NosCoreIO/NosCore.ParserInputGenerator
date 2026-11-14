using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NosCore.ParserInputGenerator.Generator;
using NosCore.ParserInputGenerator.I18N;
using NosCore.Shared.I18N;

namespace NosCore.ParserInputGenerator.Launcher
{
    /// <summary>
    /// Background service worker that downloads and processes parser input files.
    /// </summary>
    public class Worker : BackgroundService
    {
        private const string ConsoleText = "PARSER INPUT GENERATOR - NosCoreIO";

        private readonly ILogger<Worker> _logger;
        private readonly ParserInputBuilder _builder;
        private readonly IHostApplicationLifetime _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="builder">Downloads and unpacks the parser input.</param>
        public Worker(ILogger<Worker> logger, ParserInputBuilder builder, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _builder = builder;
            _lifetime = lifetime;
        }

        /// <summary>
        /// Executes the worker task to download, extract, and package parser input files.
        /// </summary>
        /// <param name="stoppingToken">Cancellation token to stop the service.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.PrintHeader(ConsoleText);
            await _builder.BuildAsync(Path.Combine(".", "output"));
            var directoryOfFilesToBeTarred = new DirectoryInfo($".{Path.DirectorySeparatorChar}output{Path.DirectorySeparatorChar}parser");
            var filesInDirectory = directoryOfFilesToBeTarred.GetFiles("*.*", SearchOption.AllDirectories);
            var tarArchiveName = $".{Path.DirectorySeparatorChar}output{Path.DirectorySeparatorChar}parser-input-files.tar.bz2";
            if (File.Exists(tarArchiveName))
            {
                File.Delete(tarArchiveName);
            }
            await using Stream targetStream = new BZip2OutputStream(File.Create(tarArchiveName));
            using var tarArchive = TarArchive.CreateOutputTarArchive(targetStream, TarBuffer.DefaultBlockFactor);
            foreach (var fileToBeTarred in filesInDirectory)
            {
                var entry = TarEntry.CreateEntryFromFile(fileToBeTarred.FullName);
                tarArchive.WriteEntry(entry, true);
            }
            tarArchive.Dispose();
            _logger.LogInformation(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.PARSER_INPUT_GENERATED));
            _lifetime.StopApplication();
        }
    }
}
