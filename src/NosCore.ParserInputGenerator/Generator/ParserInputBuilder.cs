//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NosCore.ParserInputGenerator.Downloader;
using NosCore.ParserInputGenerator.Extractor;

namespace NosCore.ParserInputGenerator.Generator
{
    /// <summary>
    /// Downloads the client archives the parser reads and unpacks them into the parser's layout.
    /// </summary>
    /// <param name="downloader">Fetches the client manifest and archives.</param>
    /// <param name="extractor">Unpacks the archives.</param>
    public class ParserInputBuilder(ClientDownloader downloader, IExtractor extractor)
    {
        /// <summary>
        /// The client archives the parser input is made from.
        /// </summary>
        public static readonly string[] Archives =
        [
            "NScliData_CZ.NOS", "NScliData_DE.NOS", "NScliData_ES.NOS", "NScliData_FR.NOS", "NScliData_IT.NOS",
            "NScliData_PL.NOS", "NScliData_RU.NOS", "NScliData_TR.NOS", "NScliData_UK.NOS",
            "NSlangData_CZ.NOS", "NSlangData_DE.NOS", "NSlangData_ES.NOS", "NSlangData_FR.NOS", "NSlangData_IT.NOS",
            "NSlangData_PL.NOS", "NSlangData_RU.NOS", "NSlangData_TR.NOS", "NSlangData_UK.NOS",
            "NStcData.NOS", "NSgtdData.NOS",
        ];

        /// <summary>
        /// Downloads the archives into <c>workFolder/client</c>, reusing those whose checksum still
        /// matches, and unpacks them into <c>workFolder/parser</c>.
        /// </summary>
        /// <param name="workFolder">The folder to download and unpack into.</param>
        /// <returns>The folder to give the parser.</returns>
        public async Task<string> BuildAsync(string workFolder)
        {
            var clientFolder = Path.Combine(workFolder, "client");
            var parserFolder = Path.Combine(workFolder, "parser");
            var mapFolder = Path.Combine(parserFolder, "map");
            Directory.CreateDirectory(mapFolder);

            var manifest = await downloader.DownloadManifest();
            var wanted = Archives.ToHashSet(StringComparer.OrdinalIgnoreCase);
            manifest.Entries = manifest.Entries.Where(e => wanted.Contains(ManifestPath.GetFileName(e.File))).ToArray();
            await downloader.DownloadClientAsync(manifest, clientFolder);

            foreach (var entry in manifest.Entries.Where(e => !e.Folder))
            {
                var fileName = ManifestPath.GetFileName(entry.File);
                var destination = (fileName.Contains("NStcData") ? mapFolder : parserFolder) + Path.DirectorySeparatorChar;
                await extractor.ExtractAsync(new FileInfo(ManifestPath.ToLocalPath(clientFolder, entry.File)), destination,
                    fileName.Contains("NScliData"));
            }

            return parserFolder;
        }
    }
}
