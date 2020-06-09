using Newtonsoft.Json;

namespace NosCore.ParserInputGenerator.Downloader
{
    public class ClientManifest
    {
        [JsonProperty("entries")]
        public Entry[] Entries { get; set; }

        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        [JsonProperty("build")]
        public long Build { get; set; }
    }

    public class Entry
    {
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("sha1", NullValueHandling = NullValueHandling.Ignore)]
        public string Sha1 { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("folder")]
        public bool Folder { get; set; }
    }

}
