namespace LibraryManagement.Common.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public bool IsPremium { get; set; }
        public decimal OutstandingFees { get; set; }
    }
}
