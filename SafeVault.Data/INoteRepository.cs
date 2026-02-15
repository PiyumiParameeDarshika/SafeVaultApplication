/* SafeVault/src/SafeVault.Data/INoteRepository.cs */
namespace SafeVault.Data;

public interface INoteRepository
{
    Task<int> CreateAsync(int userId, string title, string content, CancellationToken ct = default);
    Task<NoteRecord?> GetAsync(int noteId, int userId, CancellationToken ct = default);
    Task<IReadOnlyList<NoteRecord>> SearchAsync(int userId, string? term, CancellationToken ct = default);
}
