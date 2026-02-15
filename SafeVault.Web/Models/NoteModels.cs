/* SafeVault/src/SafeVault.Web/Models/NoteModels.cs */
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Models;

public sealed class NoteCreateModel
{
    [Required]
    [StringLength(200)]
    public string? Title { get; set; }

    [Required]
    [StringLength(4000)] // limit input size to reduce abuse
    public string? Content { get; set; }
}

public sealed class NoteListItem
{
    public int NoteId { get; init; }
    public string Title { get; init; } = "";
    public string ContentPreview { get; init; } = "";
    public DateTime CreatedUtc { get; init; }
}

public sealed class NoteListViewModel
{
    public string? Term { get; set; }
    public List<NoteListItem> Notes { get; set; } = new();
}

public sealed class NoteDetailsViewModel
{
    public int NoteId { get; init; }
    public string Title { get; init; } = "";
    public string Content { get; init; } = "";
    public DateTime CreatedUtc { get; init; }
}
