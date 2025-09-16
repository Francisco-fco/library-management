namespace LibraryManagement.Common.Helpers
{
    public static class FeeCalculator
    {
        public const decimal MaxOutstandingFeesBeforeBorrowBlock = 100.00m;
        private const int LoanPeriodDays = 14;
        public const decimal LateFeePerDayPerBook = 5.00m;

        public static bool CanMemberBorrow(decimal outstandingFees)
        {
            return outstandingFees <= MaxOutstandingFeesBeforeBorrowBlock;
        }

        public static DateTime CalculateDueDate()
        {
            return DateTime.Now.AddDays(LoanPeriodDays);
        }
    }
}
