//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using System.Diagnostics.CodeAnalysis;

namespace NosCore.ParserInputGenerator.I18N
{
    /// <summary>
    /// Enumeration of log message keys for localization.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum LogLanguageKey
    {
        /// <summary>
        /// Download successful message key.
        /// </summary>
        DOWNLOAD_SUCCESSFULL,

        /// <summary>
        /// Parser input generated message key.
        /// </summary>
        PARSER_INPUT_GENERATED,

        /// <summary>
        /// Error message key.
        /// </summary>
        ERROR,

        /// <summary>
        /// Downloading message key.
        /// </summary>
        DOWNLOADING
    }
}