using Microsoft.AspNetCore.Mvc;
using NotesApi.Model;
using NotesApi.Services;

namespace NotesApi.Controllers;
[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

public NotesController(INoteService noteService)
{
    _noteService = noteService;
}

[HttpGet]
public IActionResult GetAll(){
    return Ok(_noteService.GetAllNotes());

}
[HttpGet("{id}")]
public IActionResult GetById(int id)
{
    var note = _noteService.GetNoteById(id);
    if (note == null)
    {
        return NotFound();
    }
    return Ok(note);
}
[HttpPost]
public IActionResult Create(Note note)
{
    if (string.IsNullOrWhiteSpace(note.Title))
    {
        return BadRequest("Title is required.");
    }
    var created = _noteService.AddNote(note);
    if (created == null)
    {
        return BadRequest("Maximum number of notes reached.");
    }
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
}
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    var deleted = _noteService.DeleteNote(id);
    if (!deleted)
    {
        return NotFound();
    }
    return NoContent();
}
}