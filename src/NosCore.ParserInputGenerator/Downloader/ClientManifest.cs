using System.Text.Json;
using System.Text.Json.Serialization;

namespace NosCore.ParserInputGenerator.Downloader
{
    /// <summary>
    /// Represents a client manifest containing file entries and metadata.
    /// </summary>
    public class ClientManifest
    {
        /// <summary>
        /// Gets or sets the array of file entries in the manifest.
        /// </summary>
        [JsonPropertyName("entries")]
        public Entry[] Entries { get; set; } = null!;

        /// <summary>
        /// Gets or sets the total size of all files in bytes.
        /// </summary>
        [JsonPropertyName("totalSize")]
        public long TotalSize { get; set; }

        /// <summary>
        /// Gets or sets the build number of the client.
        /// </summary>
        [JsonPropertyName("build")]
        public long Build { get; set; }
    }

    /// <summary>
    /// Represents an individual file entry in the client manifest.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the SHA1 hash of the file.
        /// </summary>
        [JsonPropertyName("sha1")]
        public string? Sha1 { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [JsonPropertyName("file")]
        public string File { get; set; } = null!;

        /// <summary>
        /// Gets or sets file flags.
        /// </summary>
        [JsonPropertyName("flags")]
        public long Flags { get; set; }

        /// <summary>
        /// Gets or sets the file size in bytes.
        /// </summary>
        [JsonPropertyName("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this entry is a folder.
        /// </summary>
        [JsonPropertyName("folder")]
        public bool Folder { get; set; }
    }

}
