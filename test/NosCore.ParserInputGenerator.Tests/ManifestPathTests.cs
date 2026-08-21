using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NosCore.ParserInputGenerator.Downloader;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace NosCore.ParserInputGenerator.Tests;

[TestClass]
public class ManifestPathTests
{
    [TestMethod]
    [DataRow(@"NostaleData\NSlangData_PL.NOS", "NSlangData_PL.NOS")]
    [DataRow("NostaleData/NSlangData_PL.NOS", "NSlangData_PL.NOS")]
    [DataRow("NSlangData_PL.NOS", "NSlangData_PL.NOS")]
    public void GetFileNameAcceptsBothManifestSeparators(string manifestPath, string expected)
    {
        Assert.AreEqual(expected, ManifestPath.GetFileName(manifestPath));
    }

    [TestMethod]
    [DataRow(@"NostaleData\NSlangData_PL.NOS")]
    [DataRow("NostaleData/NSlangData_PL.NOS")]
    public void ToLocalPathWritesBelowOutputUsingHostSeparators(string manifestPath)
    {
        var expected = Path.Combine(".", "output", "NostaleData", "NSlangData_PL.NOS");

        Assert.AreEqual(expected, ManifestPath.ToLocalPath(Path.Combine(".", "output"), manifestPath));
    }
}
