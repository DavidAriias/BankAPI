namespace BankAPI.DTOs
{
    public class WithdrawalDTO
    {
        public int TransactionType { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }

        public int? ExternalAccount { get; set; }

    }
}
