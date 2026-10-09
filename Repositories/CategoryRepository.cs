namespace NotesApi.Repositories;
using NotesApi.Model;
using NotesApi.Data;
public class CategoryRepository : ICategoryRepository{
    private readonly InMemoryDataStore _store;

    public CategoryRepository(InMemoryDataStore store){
        _store = store;
    }

    public List<Category> GetAll(){
        lock (_store.Lock)
        {
            var result = new List<Category>();
            foreach (var category in _store.Categories.Values)
            {
                result.Add(category);
            }
            return result;
        }
    }
    public Category? GetById(int id){
        lock (_store.Lock)
        {
        if (_store.Categories.ContainsKey(id))
        {
            return _store.Categories[id];
        }
        return null;
        }
    }
    public Category Add(Category category){
        lock (_store.Lock)
        {
            category.Id = _store.NextCategoryId;
            _store.NextCategoryId++;
            _store.Categories.Add(category.Id, category);
            return category;
        }
    }

    public bool Update(Category category){
        lock (_store.Lock)
        {
            if (_store.Categories.ContainsKey(category.Id))
            {
                _store.Categories[category.Id] = category;
                return true;
            }
            return false;
        }
    }
   public bool Delete(int id){
        lock (_store.Lock)
        {
            if (_store.Categories.ContainsKey(id))
            {
                _store.Categories.Remove(id);
                return true;
            }
            return false;
        }
   }
}
