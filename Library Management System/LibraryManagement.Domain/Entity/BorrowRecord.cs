using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entity
{
    public class BorrowRecord
    {
        #region Properties

        public int Id { get; set; }

        public int BookId { get; set; }

        public int PatronId { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public BorrowStatus Status { get; set; }

        #endregion

        #region Navigation Properties

        public Book Book { get; set; } = null!;

        public Patron Patron { get; set; } = null!;

        #endregion
    }
}