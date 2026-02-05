namespace LibraryManagement.Domain.Entity
{
    public class Author
    {
        #region Properties

        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }

        #endregion

        #region Navigation Properties

        public ICollection<Book> Books { get; set; } = new List<Book>();

        #endregion
    }
}