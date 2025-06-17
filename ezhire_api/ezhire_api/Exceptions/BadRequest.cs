namespace ezhire_api.Exceptions;

public class BadRequest : Exception
{
    public BadRequest(string message): base(message)
    {
    }
}