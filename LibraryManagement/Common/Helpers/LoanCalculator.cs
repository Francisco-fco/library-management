namespace LibraryManagement.Common.Helpers
{
    public static class LoanCalculator
    {
        private const int StandardMemberMaxBooks = 3;
        private const int PremiumMemberMaxBooks = 5;

        public static bool CanMemberBorrowMoreBooks(int currentBooksCount, bool isPremium)
        {
            int maxBooks = GetMaxBooksForMember(isPremium);
            return currentBooksCount < maxBooks;
        }

        public static int GetMaxBooksForMember(bool isPremium)
        {
            return isPremium ? PremiumMemberMaxBooks : StandardMemberMaxBooks;
        }

        public static int GetRemainingBorrowCapacity(int currentBooksCount, bool isPremium)
        {
            int maxBooks = GetMaxBooksForMember(isPremium);
            return Math.Max(0, maxBooks - currentBooksCount);
        }
    }
}
