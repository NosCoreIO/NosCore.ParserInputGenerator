using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NosCore.ParserInputGenerator.Downloader;
using NosCore.ParserInputGenerator.Extractor;
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
        private readonly IClientDownloader _client;
        private readonly IExtractor _extractor;
        private readonly IHostApplicationLifetime _lifetime;

        private readonly string[] _parserInputFiles = {
            "NScliData_CZ.NOS",
            "NScliData_DE.NOS",
            "NScliData_ES.NOS",
            "NScliData_FR.NOS",
            "NScliData_IT.NOS",
            "NScliData_PL.NOS",
            "NScliData_RU.NOS",
            "NScliData_TR.NOS",
            "NScliData_UK.NOS",
            "NSlangData_CZ.NOS",
            "NSlangData_DE.NOS",
            "NSlangData_ES.NOS",
            "NSlangData_FR.NOS",
            "NSlangData_IT.NOS",
            "NSlangData_PL.NOS",
            "NSlangData_RU.NOS",
            "NSlangData_TR.NOS",
            "NSlangData_UK.NOS",
            "NStcData.NOS",
            "NSgtdData.NOS"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="client">The client downloader.</param>
        /// <param name="extractor">The file extractor.</param>
        public Worker(ILogger<Worker> logger, IClientDownloader client, IExtractor extractor,
            IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _client = client;
            _extractor = extractor;
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
            var manifest = await _client.DownloadManifest();

            var requestedFiles = _parserInputFiles.ToHashSet(System.StringComparer.OrdinalIgnoreCase);

            manifest.Entries = manifest.Entries
                .Where(entry =>
                {
                    return requestedFiles.Contains(ManifestPath.GetFileName(entry.File));
                })
                .ToArray();

            await _client.DownloadClientAsync(manifest);

            foreach (var entry in manifest.Entries)
            {
                var fileName = ManifestPath.GetFileName(entry.File);

                var rename = fileName.Contains("NScliData");

                var dest = fileName.Contains("NStcData")
                    ? Path.Combine(".", "output", "parser", "map") + Path.DirectorySeparatorChar
                    : $".{Path.DirectorySeparatorChar}output{Path.DirectorySeparatorChar}parser{Path.DirectorySeparatorChar}";

                var localPath = ManifestPath.ToLocalPath(Path.Combine(".", "output"), entry.File);

                var fileInfo = new FileInfo(localPath);

                await _extractor.ExtractAsync(fileInfo, dest, rename);
            }
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
