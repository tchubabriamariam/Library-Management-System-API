namespace LibraryManagement.Application.Exceptions;

public class EntityNotFoundException : CustomBaseException
{
    public EntityNotFoundException(string entityName, object id) 
        : base($"{entityName} with ID {id} was not found.", "EntityNotFound") { }
}