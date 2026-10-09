namespace NotesApi.Dtos;

//for PUT /notes/{id}
public class UpdateNoteDto
{
    public string Title {get;set;}=string.Empty;
    public string Content {get;set;}=string.Empty;
    public int CategoryId {get;set;}
    public bool IsArchived {get;set;}
}
