namespace NotesApi.Exceptions;

//middleware turns this into 404
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
