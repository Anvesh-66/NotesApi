namespace NotesApi.Services;
using NotesApi.Model;

public interface INoteService{
    Note? AddNote(Note note);
    List<Note> GetAllNotes();
    Note? GetNoteById(int id);
    bool DeleteNote(int id);
}
