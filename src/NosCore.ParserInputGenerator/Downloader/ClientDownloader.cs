//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NosCore.ParserInputGenerator.I18N;
using NosCore.Shared.Enumerations;

namespace NosCore.ParserInputGenerator.Downloader
{
    /// <summary>
    /// Implements client file downloading functionality.
    /// </summary>
    public class ClientDownloader : IClientDownloader
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ClientDownloader> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDownloader"/> class.
        /// </summary>
        /// <param name="clientFactory">The HTTP client factory.</param>
        /// <param name="logger">The logger instance.</param>
        public ClientDownloader(IHttpClientFactory clientFactory, ILogger<ClientDownloader> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        private const int DownloadAttempts = 3;

        private static async Task<bool> HasChecksumAsync(string file, string? sha1)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            await using var stream = File.OpenRead(file);
            var checksum = Convert.ToHexString(await System.Security.Cryptography.SHA1.HashDataAsync(stream)).ToLowerInvariant();
            return checksum == sha1;
        }

        /// <inheritdoc/>
        public Task<ClientManifest> DownloadManifest() => DownloadManifestAsync(RegionType.EN);

        /// <inheritdoc/>
        public async Task<ClientManifest> DownloadManifestAsync(RegionType region)
        {
            var client = _clientFactory.CreateClient();
            using var result = await client
                .GetAsync($"https://spark.gameforge.com/api/v1/patching/download/latest/nostale/default?locale={region}&architecture=x64&branchToken")
                .ConfigureAwait(false);
            return JsonSerializer.Deserialize<ClientManifest>(await result.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            }) ?? throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public async Task DownloadClientAsync() => await DownloadClientAsync(await DownloadManifest());

        /// <inheritdoc/>
        public Task DownloadClientAsync(ClientManifest manifest) => DownloadClientAsync(manifest, Path.Combine(".", "output"));

        /// <summary>
        /// Downloads the manifest's entries under <paramref name="root"/>, skipping files whose checksum already matches.
        /// </summary>
        /// <param name="manifest">The entries to download.</param>
        /// <param name="root">The folder to download into.</param>
        /// <returns>A task that completes once every entry is downloaded.</returns>
        public async Task DownloadClientAsync(ClientManifest manifest, string root)
        {
            async Task Download(Entry entry)
            {
                var client = _clientFactory.CreateClient();

                if (entry.Folder)
                {
                    Directory.CreateDirectory(ManifestPath.ToLocalPath(root, entry.File));
                    return;
                }
                var file = ManifestPath.ToLocalPath(root, entry.File);
                if (await HasChecksumAsync(file, entry.Sha1))
                {
                    return;
                }

                var fileInfo = new FileInfo(file);
                if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }

                // A transfer cut short, or a file that arrives with the wrong checksum, is fetched again.
                for (var attempt = 1; ; attempt++)
                {
                    _logger.LogInformation(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.DOWNLOADING), entry.Path);
                    try
                    {
                        using var response = await client.GetAsync($"http://patches.gameforge.com/" + entry.Path,
                            HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();
                        await using (var fileStream = File.Create(file))
                        {
                            await using var stream = await response.Content.ReadAsStreamAsync();
                            await stream.CopyToAsync(fileStream);
                        }

                        if (await HasChecksumAsync(file, entry.Sha1))
                        {
                            break;
                        }

                        if (attempt == DownloadAttempts)
                        {
                            throw new InvalidDataException($"{entry.Path} does not match its checksum after {DownloadAttempts} downloads.");
                        }
                    }
                    catch (Exception e) when (e is HttpRequestException or IOException && attempt < DownloadAttempts)
                    {
                        _logger.LogWarning(e, LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.ERROR));
                    }
                }

                _logger.LogInformation(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.DOWNLOAD_SUCCESSFULL), file);
            }

            await Task.WhenAll(manifest.Entries.Select(Download));
        }

    }
}
