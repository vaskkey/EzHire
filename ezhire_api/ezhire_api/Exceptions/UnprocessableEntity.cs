namespace ezhire_api.Exceptions;

public class UnprocessableEntity : Exception
{
    public UnprocessableEntity(string message) : base(message)
    {
    }
}