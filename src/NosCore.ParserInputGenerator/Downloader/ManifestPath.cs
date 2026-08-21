using System.IO;

namespace NosCore.ParserInputGenerator.Downloader;

/// <summary>
/// Converts Gameforge manifest paths, whose separator is platform-independent,
/// into paths suitable for the current host.
/// </summary>
public static class ManifestPath
{
    /// <summary>
    /// Gets the final file name from a manifest path using either slash style.
    /// </summary>
    public static string GetFileName(string manifestPath)
    {
        var normalized = manifestPath.Replace('\\', '/').TrimEnd('/');
        var separator = normalized.LastIndexOf('/');
        return separator < 0 ? normalized : normalized[(separator + 1)..];
    }

    /// <summary>
    /// Converts a manifest path into a local path below <paramref name="root"/>.
    /// </summary>
    public static string ToLocalPath(string root, string manifestPath)
    {
        var relativePath = manifestPath
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);

        return Path.Combine(root, relativePath);
    }
}
