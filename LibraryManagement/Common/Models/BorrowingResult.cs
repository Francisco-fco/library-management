namespace LibraryManagement.Common.Models
{
    public class BorrowingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<BorrowedBook> CurrentBooks { get; set; } = new List<BorrowedBook>();
    }
}
