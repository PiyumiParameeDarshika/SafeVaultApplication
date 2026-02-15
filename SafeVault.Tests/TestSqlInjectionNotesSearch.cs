/* SafeVault/tests/SafeVault.Tests/TestSqlInjectionNotesSearch.cs */
using NUnit.Framework;
using Microsoft.Data.SqlClient;
using SafeVault.Data;

namespace SafeVault.Tests;

[TestFixture]
public class TestSqlInjectionNotesSearch
{
    [Test]
    public void NotesSearch_UsesParameterizedQuery_IncludingMaliciousInput()
    {
        var payload = "%' OR 1=1;--";

        using var conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;");
        var cmd = SqlNoteRepository.BuildSearchCommand(conn, userId: 1, term: payload);

        // Command text should remain static and contain @Term placeholder
        Assert.That(cmd.CommandText, Does.Contain("@Term"));
        Assert.That(cmd.CommandText, Does.Not.Contain(payload));

        // Payload should exist ONLY in parameter value (with wildcards)
        Assert.That(cmd.Parameters.Contains("@Term"), Is.True);
        var termVal = cmd.Parameters["@Term"]!.Value!.ToString()!;
        Assert.That(termVal, Does.Contain(payload));
    }
}
