/* SafeVault/src/SafeVault.Data/SqlUserRepository.cs */
using Microsoft.Data.SqlClient;
using System.Data;

namespace SafeVault.Data;

public sealed class SqlUserRepository : IUserRepository
{
    private readonly string _connectionString;

    public SqlUserRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<int> CreateUserAsync(string username, string email, string passwordHash, string role = "User", CancellationToken ct = default)
    {
        const string sql = @"
INSERT INTO dbo.Users (Username, Email, PasswordHash, Role)
OUTPUT INSERTED.UserID
VALUES (@Username, @Email, @PasswordHash, @Role);";

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 100) { Value = username });
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 320) { Value = email });
        cmd.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 200) { Value = passwordHash });
        cmd.Parameters.Add(new SqlParameter("@Role", SqlDbType.NVarChar, 50) { Value = role });

        await conn.OpenAsync(ct);
        var newId = (int)(await cmd.ExecuteScalarAsync(ct) ?? 0);
        return newId;
    }

    public async Task<UserRecord?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = BuildGetByUsernameCommand(conn, username);

        await conn.OpenAsync(ct);
        await using var rdr = await cmd.ExecuteReaderAsync(ct);

        if (!await rdr.ReadAsync(ct)) return null;

        return new UserRecord(
            rdr.GetInt32(0),
            rdr.GetString(1),
            rdr.GetString(2),
            rdr.GetString(3),
            rdr.GetString(4)
        );
    }

    internal static SqlCommand BuildGetByUsernameCommand(SqlConnection conn, string username)
    {
        const string sql = @"
SELECT UserID, Username, Email, PasswordHash, Role
FROM dbo.Users
WHERE Username = @Username;";

        var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 100) { Value = username });
        return cmd;
    }
}
