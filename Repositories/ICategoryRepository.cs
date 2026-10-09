namespace NotesApi.Repositories;
using NotesApi.Model;

public interface ICategoryRepository{
    List<Category> GetAll();
    Category? GetById(int id);
    Category Add(Category category);
    bool Update(Category category);
    bool Delete(int id);
}
