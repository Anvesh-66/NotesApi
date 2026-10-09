namespace NotesApi.Exceptions;

//middleware turns this into 400
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}
