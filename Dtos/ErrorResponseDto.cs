namespace NotesApi.Dtos;

//every error from the api comes back in this same shape
public class ErrorResponseDto
{
    public int StatusCode {get;set;}
    public string Message {get;set;}=string.Empty;
}
