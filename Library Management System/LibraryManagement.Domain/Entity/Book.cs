namespace LibraryManagement.Domain.Entity
{
    public class Book
    {
        #region Properties

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public int PublicationYear { get; set; }

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public int Quantity { get; set; }

        public int AuthorId { get; set; }

        #endregion

        #region Navigation Properties

        public Author Author { get; set; } = null!;

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

        #endregion
    }
}