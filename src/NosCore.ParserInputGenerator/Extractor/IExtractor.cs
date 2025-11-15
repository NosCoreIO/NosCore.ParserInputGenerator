//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using System.IO;
using System.Threading.Tasks;

namespace NosCore.ParserInputGenerator.Extractor
{
    /// <summary>
    /// Interface for extracting archived files.
    /// </summary>
    public interface IExtractor
    {
        /// <summary>
        /// Extracts a file to the default directory.
        /// </summary>
        /// <param name="file">The file to extract.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExtractAsync(FileInfo file);

        /// <summary>
        /// Extracts a file to a specified directory.
        /// </summary>
        /// <param name="file">The file to extract.</param>
        /// <param name="directory">The target directory.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExtractAsync(FileInfo file, string directory);

        /// <summary>
        /// Extracts a file to a specified directory with optional renaming.
        /// </summary>
        /// <param name="file">The file to extract.</param>
        /// <param name="directory">The target directory.</param>
        /// <param name="rename">Whether to rename extracted files.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExtractAsync(FileInfo file, string directory, bool rename);
    }
}