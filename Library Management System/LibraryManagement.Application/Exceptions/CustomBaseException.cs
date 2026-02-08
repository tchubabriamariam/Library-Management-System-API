namespace LibraryManagement.Application.Exceptions;

public abstract class CustomBaseException : Exception
{
    public string Code { get; }
    protected CustomBaseException(string message, string code) : base(message) => Code = code;
}



