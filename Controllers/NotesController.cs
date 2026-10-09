using Microsoft.AspNetCore.Mvc;
using NotesApi.Dtos;
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
[ProducesResponseType(typeof(List<NoteResponseDto>), StatusCodes.Status200OK)]
public IActionResult GetAll([FromQuery] int? categoryId, [FromQuery] string? search, [FromQuery] bool includeArchived = false){
    return Ok(_noteService.GetAll(categoryId, search, includeArchived));

}
[HttpGet("{id}")]
[ProducesResponseType(typeof(NoteResponseDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
public IActionResult GetById(int id)
{
    return Ok(_noteService.GetById(id));
}
[HttpPost]
[ProducesResponseType(typeof(NoteResponseDto), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
public IActionResult Create(CreateNoteDto dto)
{
    var created = _noteService.Create(dto);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
}
[HttpPut("{id}")]
[ProducesResponseType(typeof(NoteResponseDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
public IActionResult Update(int id, UpdateNoteDto dto)
{
    return Ok(_noteService.Update(id, dto));
}
[HttpDelete("{id}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
public IActionResult Delete(int id)
{
    _noteService.Delete(id);
    return NoContent();
}
}
