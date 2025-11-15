//  __  _  __    __   ___ __  ___ ___
// |  \| |/__\ /' _/ / _//__\| _ \ __|
// | | ' | \/ |`._`.| \_| \/ | v / _|
// |_|\__|\__/ |___/ \__/\__/|_|_\___|
// -----------------------------------

using System.Globalization;
using System.Resources;
using NosCore.Shared.Enumerations;

namespace NosCore.ParserInputGenerator.I18N
{
    /// <summary>
    /// Provides localized log messages based on language keys.
    /// </summary>
    public sealed class LogLanguage
    {
        private static LogLanguage? _instance;

        private static readonly CultureInfo ResourceCulture = new CultureInfo(Language.ToString());

        private readonly ResourceManager _manager;

        private LogLanguage()
        {
            var assem = typeof(LogLanguageKey).Assembly;
            _manager = new ResourceManager(
                assem.GetName().Name + ".Resource.LocalizedResources",
                assem);
        }

        /// <summary>
        /// Gets or sets the language/region type for localization.
        /// </summary>
        public static RegionType Language { get; set; }

        /// <summary>
        /// Gets the singleton instance of LogLanguage.
        /// </summary>
        public static LogLanguage Instance => _instance ??= new LogLanguage();

        /// <summary>
        /// Gets a localized message from the specified key using the default culture.
        /// </summary>
        /// <param name="messageKey">The message key to retrieve.</param>
        /// <returns>The localized message string.</returns>
        public string GetMessageFromKey(LogLanguageKey messageKey)
        {
            return GetMessageFromKey(messageKey, null);
        }

        /// <summary>
        /// Gets a localized message from the specified key using a specific culture.
        /// </summary>
        /// <param name="messageKey">The message key to retrieve.</param>
        /// <param name="culture">The culture name to use, or null for default.</param>
        /// <returns>The localized message string.</returns>
        public string GetMessageFromKey(LogLanguageKey messageKey, string? culture)
        {
            var cult = culture != null ? new CultureInfo(culture) : ResourceCulture;
            var resourceMessage = (_manager != null)
                ? _manager.GetResourceSet(cult, true,
                        cult.TwoLetterISOLanguageName == default(RegionType).ToString().ToLower(cult))
                    ?.GetString(messageKey.ToString()) : string.Empty;

            return !string.IsNullOrEmpty(resourceMessage) ? resourceMessage : $"#<{messageKey}>";
        }
    }
}