/* SafeVault/src/SafeVault.Web/Controllers/NotesController.cs */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Web.Models;
using System.Security.Claims;

namespace SafeVault.Web.Controllers;

[Authorize] // must be logged in
public class NotesController : Controller
{
    private readonly INoteRepository _notes;

    public NotesController(INoteRepository notes) => _notes = notes;

    private int CurrentUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("/notes")]
    public async Task<IActionResult> Index(string? term, CancellationToken ct)
    {
        var userId = CurrentUserId();
        var list = await _notes.SearchAsync(userId, term, ct);

        // PREVIEW: keep as plain text; Razor will encode it.
        var vm = new NoteListViewModel
        {
            Term = term,
            Notes = list.Select(n => new NoteListItem
            {
                NoteId = n.NoteId,
                Title = n.Title,
                ContentPreview = n.Content.Length <= 120 ? n.Content : n.Content.Substring(0, 120) + "...",
                CreatedUtc = n.CreatedUtc
            }).ToList()
        };

        return View(vm);
    }

    [HttpGet("/notes/create")]
    public IActionResult Create() => View(new NoteCreateModel());

    [HttpPost("/notes/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] NoteCreateModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = CurrentUserId();
        var newId = await _notes.CreateAsync(userId, model.Title!.Trim(), model.Content!, ct);

        return RedirectToAction(nameof(Details), new { id = newId });
    }

    [HttpGet("/notes/{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var userId = CurrentUserId();
        var note = await _notes.GetAsync(id, userId, ct);
        if (note is null) return NotFound();

        var vm = new NoteDetailsViewModel
        {
            NoteId = note.NoteId,
            Title = note.Title,
            Content = note.Content,
            CreatedUtc = note.CreatedUtc
        };

        // IMPORTANT XSS FIX:
        // In the view we output @Model.Content (NOT Html.Raw), so scripts won't execute.
        return View(vm);
    }
}
