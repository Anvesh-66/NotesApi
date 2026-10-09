namespace NotesApi.Repositories;
using NotesApi.Model;

public interface INoteRepository{
    List<Note> GetAll();
    Note? GetById(int id);
    Note Add(Note note);
    bool Update(Note note);
    bool Delete(int id);
    List<Note> GetByCategoryId(int categoryId);
    List<Note> Search(string keyword);
}
