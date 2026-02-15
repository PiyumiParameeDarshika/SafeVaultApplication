/* SafeVault/tests/SafeVault.Tests/TestInputValidation.cs */
using NUnit.Framework;
using SafeVault.Web.Security;
using SafeVault.Web.Validation;
using SafeVault.Data;
using Microsoft.Data.SqlClient;

namespace SafeVault.Tests;

[TestFixture]
public class TestInputValidation
{
    private IInputSanitizer _sanitizer = null!;

    [SetUp]
    public void Setup()
    {
        _sanitizer = new InputSanitizer();
    }

    [Test]
    public void TestForSQLInjection_InputRejectedOrNeutralized()
    {
        // Typical SQL injection payload
        var payload = "admin' OR 1=1;--";

        var sanitized = _sanitizer.SanitizeUsername(payload);

        // Our username validator only allows safe characters; the payload must fail validation.
        var attr = new UsernameAttribute();
        Assert.That(attr.IsValid(sanitized), Is.False);

        // Also check sanitization removed SQL glue characters like quotes/semicolon
        Assert.That(sanitized, Does.Not.Contain("'"));
        Assert.That(sanitized, Does.Not.Contain(";"));
    }

    [Test]
    public void TestForXSS_ScriptTagsRemoved_AndRejected()
    {
        var payload = "<script>alert('xss')</script>";

        var sanitized = _sanitizer.SanitizeUsername(payload);

        // Ensure angle brackets removed by sanitizer (defense-in-depth)
        Assert.That(sanitized, Does.Not.Contain("<"));
        Assert.That(sanitized, Does.Not.Contain(">"));

        // But even if some content remains, validator should still reject it.
        var attr = new UsernameAttribute();
        Assert.That(attr.IsValid(sanitized), Is.False);
    }

    [Test]
    public void Repository_UsesParameterizedQuery_ForLookup()
    {
        using var conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;");
        var cmd = SqlUserRepository.BuildGetByUsernameCommand(conn, "anyuser");

        // 1) Command text uses @Username placeholder (not string concatenation)
        Assert.That(cmd.CommandText, Does.Contain("@Username"));

        // 2) Parameter exists and value is assigned
        Assert.That(cmd.Parameters.Contains("@Username"), Is.True);
        Assert.That(cmd.Parameters["@Username"]!.Value, Is.EqualTo("anyuser"));
    }

    [Test]
    public void Email_Disallows_ControlCharacters()
    {
        var attr = new NoControlCharsAttribute();
        Assert.That(attr.IsValid("user@example.com\r\nBCC: attacker@example.com"), Is.False);
    }
}
