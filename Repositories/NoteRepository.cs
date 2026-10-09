namespace NotesApi.Repositories;
using NotesApi.Model;
using NotesApi.Data;
public class NoteRepository : INoteRepository{
    private readonly InMemoryDataStore _store;

    public NoteRepository(InMemoryDataStore store){
        _store = store;
    }

    public List<Note> GetAll(){
        lock (_store.Lock)
        {
        var result = new List<Note>();
        foreach (var note in _store.Notes.Values)
        {
            result.Add(note);
        }
        return result;
        }
    }
   public Note? GetById(int id)
   {
    lock (_store.Lock)
    {
        if (_store.Notes.ContainsKey(id))
        {
            return _store.Notes[id];
        }
        return null;
    }
   }
    public Note Add(Note note){
        lock (_store.Lock)
        {
            note.Id = _store.NextNoteId;
            _store.NextNoteId++;
            _store.Notes.Add(note.Id, note);
            return note;
        }
    }
    public bool Update(Note note){
        lock (_store.Lock)
        {
            if (_store.Notes.ContainsKey(note.Id))
            {
                _store.Notes[note.Id] = note;
                return true;
            }
            return false;
        }
    }
    public bool Delete(int id){
        lock (_store.Lock)
        {
            if (_store.Notes.ContainsKey(id))
            {
                _store.Notes.Remove(id);
                return true;
            }
            return false;
        }
 }

    //same loop as given in mail
    public List<Note> GetByCategoryId(int categoryId){
        lock (_store.Lock)
        {
            var result = new List<Note>();
            foreach (var note in _store.Notes.Values)
            {
                if (note.CategoryId == categoryId)
                {
                    result.Add(note);
                }
            }
            return result;
        }
    }
    public List<Note> Search(string keyword){
        var word = keyword.ToLower();//making both lower so that case does not matter
        lock (_store.Lock)
        {
            var result = new List<Note>();
            foreach (var note in _store.Notes.Values)
            {
                if (note.Title.ToLower().Contains(word) || note.Content.ToLower().Contains(word))
                {
                    result.Add(note);
                }
            }
            return result;
        }
    }
}
