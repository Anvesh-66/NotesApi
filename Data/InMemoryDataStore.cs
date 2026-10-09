namespace NotesApi.Data;
using NotesApi.Model;

//this is my fake database, only repositories should use this class
public class InMemoryDataStore
{
    public Dictionary<int, Note> Notes {get;} = new();
    public Dictionary<int, Category> Categories {get;} = new();

    //separate counter for each, like identity column in a table
    public int NextNoteId {get;set;} = 1;
    public int NextCategoryId {get;set;} = 1;

    //for the bonus part, repositories lock on this before reading/writing
    public object Lock {get;} = new();

    public InMemoryDataStore()
    {
        SeedCategory("Personal");
        SeedCategory("Work");
        SeedCategory("Study");
    }

    private void SeedCategory(string name)
    {
        var category = new Category();
        category.Id = NextCategoryId++;
        category.Name = name;
        Categories.Add(category.Id, category);
    }
}
