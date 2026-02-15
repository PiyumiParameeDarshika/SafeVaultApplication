/* SafeVault/src/SafeVault.Data/IUserRepository.cs */
namespace SafeVault.Data;

public interface IUserRepository
{
    Task<UserRecord?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<int> CreateUserAsync(string username, string email, string passwordHash, string role = "User", CancellationToken ct = default);
}
