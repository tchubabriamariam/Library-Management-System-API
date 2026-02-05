namespace LibraryManagement.Domain.Entity
{
    public class Patron
    {
        #region Properties

        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime MembershipDate { get; set; }

        #endregion

        #region Navigation Properties

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

        #endregion
    }
}