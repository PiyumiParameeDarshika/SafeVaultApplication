/* SafeVault/src/SafeVault.Data/SqlNoteRepository.cs */
using Microsoft.Data.SqlClient;
using System.Data;

namespace SafeVault.Data;

public sealed class SqlNoteRepository : INoteRepository
{
    private readonly string _connectionString;

    public SqlNoteRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<int> CreateAsync(int userId, string title, string content, CancellationToken ct = default)
    {
        const string sql = @"
INSERT INTO dbo.Notes (UserID, Title, Content)
OUTPUT INSERTED.NoteID
VALUES (@UserID, @Title, @Content);";

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });
        cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200) { Value = title });
        cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.NVarChar) { Value = content });

        await conn.OpenAsync(ct);
        return (int)(await cmd.ExecuteScalarAsync(ct) ?? 0);
    }

    public async Task<NoteRecord?> GetAsync(int noteId, int userId, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = BuildGetCommand(conn, noteId, userId);

        await conn.OpenAsync(ct);
        await using var rdr = await cmd.ExecuteReaderAsync(ct);

        if (!await rdr.ReadAsync(ct)) return null;

        return new NoteRecord(
            rdr.GetInt32(0),
            rdr.GetInt32(1),
            rdr.GetString(2),
            rdr.GetString(3),
            rdr.GetDateTime(4)
        );
    }

    public async Task<IReadOnlyList<NoteRecord>> SearchAsync(int userId, string? term, CancellationToken ct = default)
    {
        term ??= string.Empty;

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = BuildSearchCommand(conn, userId, term);

        await conn.OpenAsync(ct);
        await using var rdr = await cmd.ExecuteReaderAsync(ct);

        var list = new List<NoteRecord>();
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new NoteRecord(
                rdr.GetInt32(0),
                rdr.GetInt32(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetDateTime(4)
            ));
        }

        return list;
    }

    // Internal to allow unit tests to validate parameterization without a database.
    internal static SqlCommand BuildSearchCommand(SqlConnection conn, int userId, string term)
    {
        const string sql = @"
SELECT NoteID, UserID, Title, Content, CreatedUtc
FROM dbo.Notes
WHERE UserID = @UserID
  AND (@Term = N'' OR Title LIKE @Term OR Content LIKE @Term)
ORDER BY CreatedUtc DESC;";

        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });

        // Parameterized LIKE: keep SQL static; include wildcards in parameter value.
        var like = string.IsNullOrWhiteSpace(term) ? string.Empty : $"%{term}%";
        cmd.Parameters.Add(new SqlParameter("@Term", SqlDbType.NVarChar, 4000) { Value = like });

        return cmd;
    }

    internal static SqlCommand BuildGetCommand(SqlConnection conn, int noteId, int userId)
    {
        const string sql = @"
SELECT NoteID, UserID, Title, Content, CreatedUtc
FROM dbo.Notes
WHERE NoteID = @NoteID AND UserID = @UserID;";

        var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@NoteID", SqlDbType.Int) { Value = noteId });
        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });
        return cmd;
    }
}
