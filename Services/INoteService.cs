namespace NotesApi.Services;
using NotesApi.Dtos;

public interface INoteService{
    List<NoteResponseDto> GetAll(int? categoryId, string? search, bool includeArchived);
    NoteResponseDto GetById(int id);
    NoteResponseDto Create(CreateNoteDto dto);
    NoteResponseDto Update(int id, UpdateNoteDto dto);
    void Delete(int id);
    NoteResponseDto Archive(int id);
    NoteResponseDto Unarchive(int id);
}
