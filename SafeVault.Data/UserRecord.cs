/* SafeVault/src/SafeVault.Data/UserRecord.cs */
namespace SafeVault.Data;

public sealed record UserRecord(
    int UserId,
    string Username,
    string Email,
    string PasswordHash,
    string Role
);
