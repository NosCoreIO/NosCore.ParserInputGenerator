using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NosCore.ParserInputGenerator.I18N;
using NosCore.Shared.Enumerations;

namespace NosCore.ParserInputGenerator.Tests;

[TestClass]
public class LogLanguageTests
{
    [TestMethod]
    public void EveryKeyHasAMessageInEveryLanguage()
    {
        foreach (var region in Enum.GetValues<RegionType>())
        {
            foreach (var key in Enum.GetValues<LogLanguageKey>())
            {
                var message = LogLanguage.Instance.GetMessageFromKey(key, region.ToString());
                Assert.AreNotEqual($"#<{key}>", message, $"{key} has no message for {region}");
            }
        }
    }
}
