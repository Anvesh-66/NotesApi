namespace NotesApi.Dtos;

//for GET /categories/summary
public class CategorySummaryDto
{
    public int Id {get;set;}
    public string Name {get;set;}=string.Empty;
    public int NoteCount {get;set;}
}
