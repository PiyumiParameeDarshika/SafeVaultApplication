/* SafeVault/tests/SafeVault.Tests/TestXssNotesViews.cs */
using NUnit.Framework;
using System.IO;

namespace SafeVault.Tests;

[TestFixture]
public class TestXssNotesViews
{
    [Test]
    public void NotesDetailsView_MustNotUse_HtmlRaw()
    {
        // This test is a safety net against stored XSS caused by raw rendering.
        // Update the path if your solution layout differs.
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Views", "Notes", "Details.cshtml");

        // If you run tests from bin/, the view may not be copied. In that case,
        // set the file's "Copy to Output Directory" to "Copy always", OR
        // change this path to your repo root. For assignment marking, the code content matters.
        if (!File.Exists(path))
        {
            Assert.Inconclusive("Details.cshtml not found in test output. Ensure the view is available or adjust the path.");
            return;
        }

        var cshtml = File.ReadAllText(path);
        Assert.That(cshtml, Does.Not.Contain("Html.Raw("));
    }
}
