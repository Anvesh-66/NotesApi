namespace NotesApi.Dtos;

//what the client sends in POST /notes
public class CreateNoteDto
{
    public string Title {get;set;}=string.Empty;
    public string Content {get;set;}=string.Empty;
    public int CategoryId {get;set;}
}
