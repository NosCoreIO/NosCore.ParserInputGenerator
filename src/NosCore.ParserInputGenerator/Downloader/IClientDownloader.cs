using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NosCore.Shared.Enumerations;

namespace NosCore.ParserInputGenerator.Downloader
{
    /// <summary>
    /// Interface for downloading client files and manifests.
    /// </summary>
    public interface IClientDownloader
    {
        /// <summary>
        /// Downloads the client manifest using the default region.
        /// </summary>
        /// <returns>The downloaded client manifest.</returns>
        Task<ClientManifest> DownloadManifest();

        /// <summary>
        /// Downloads the client manifest for a specific region.
        /// </summary>
        /// <param name="region">The region to download from.</param>
        /// <returns>The downloaded client manifest.</returns>
        Task<ClientManifest> DownloadManifestAsync(RegionType region);

        /// <summary>
        /// Downloads the client files using the default manifest.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DownloadClientAsync();

        /// <summary>
        /// Downloads the client files specified in the manifest.
        /// </summary>
        /// <param name="manifest">The manifest containing file information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DownloadClientAsync(ClientManifest manifest);
    }
}
