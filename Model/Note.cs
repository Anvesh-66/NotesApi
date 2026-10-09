namespace NotesApi.Model;
public class Note
{
    public int Id{ get;set;}
    public string Title {get;set;}=string.Empty;//it gives "" insead of null so that it removes the warnings.
    public string Content {get;set;} =string.Empty;
    public DateTime CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}//null till the note is edited first time
    public bool IsArchived {get;set;}
    public int CategoryId {get;set;}//only the id, no Category object here

}
