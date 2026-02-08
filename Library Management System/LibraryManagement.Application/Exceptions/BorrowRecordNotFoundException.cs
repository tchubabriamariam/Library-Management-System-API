namespace LibraryManagement.Application.Exceptions;

public class BorrowRecordNotFoundException : CustomBaseException
{
    public BorrowRecordNotFoundException(int id) : 
        base($"Active borrow record with ID {id} was not found or has already been returned.", "BorrowRecordNotFound") { }
    
}