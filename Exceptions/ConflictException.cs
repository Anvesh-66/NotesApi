namespace NotesApi.Exceptions;

//for duplicate category name, middleware turns this into 409
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
