namespace NotesApi.Dtos;

//this is what goes back to the client, not the Note entity
public class NoteResponseDto
{
    public int Id {get;set;}
    public string Title {get;set;}=string.Empty;
    public string Content {get;set;}=string.Empty;
    public string CategoryName {get;set;}=string.Empty;//name instead of CategoryId
    public DateTime CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
}
