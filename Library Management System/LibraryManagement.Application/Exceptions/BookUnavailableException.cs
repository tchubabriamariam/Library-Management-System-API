namespace LibraryManagement.Application.Exceptions;

public class BookUnavailableException : CustomBaseException
{
    public BookUnavailableException(string message) 
        : base(message, "BookUnavailable") { }
}