namespace ezhire_api.Exceptions;

public class NotFound : Exception
{
    public NotFound() : base("Not Found.")
    {
    }
    
    public NotFound(string message) : base(message)
    {
    }
}