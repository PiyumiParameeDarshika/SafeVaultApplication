/* SafeVault/src/SafeVault.Data/NoteRecord.cs */
namespace SafeVault.Data;

public sealed record NoteRecord(
    int NoteId,
    int UserId,
    string Title,
    string Content,
    DateTime CreatedUtc
);
