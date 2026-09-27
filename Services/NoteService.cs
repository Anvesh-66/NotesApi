namespace NotesApi.Services;
using NotesApi.Model;
using Microsoft.Extensions.Options;
public class NoteService : INoteService{
    private readonly List<Note> _notes = new();
    private int _nextId = 1;
    private readonly int _maxNotes;

    public NoteService(IOptions<NoteSettings> options){
        _maxNotes = options.Value.MaxNotes;
    }

 public Note? AddNote(Note note){
        if (_notes.Count >= _maxNotes)
        {
            return null;
        }
        note.Id = _nextId++;
        note.CreatedAt = DateTime.Now;
        _notes.Add(note);
        return note;
    }
    public List<Note> GetAllNotes(){
        return _notes;
    }
   public Note? GetNoteById(int id)
  {
    foreach (var note in _notes)
    {
        if (note.Id == id)
        {
            return note;
        }
    }
    return null;
    }
    public bool DeleteNote(int id){
        var note = GetNoteById(id);
        if (note != null)
        {
            _notes.Remove(note);
            return true;
        }
        return false;
 }
}
