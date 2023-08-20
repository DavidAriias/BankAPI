using BankAPI.BankModels;
using BankAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class TransactionService
    {
        private readonly BankContext _context;

        public TransactionService(BankContext context)
        {
            _context = context;
        }

        public async Task<AccountDtoOut?> GetAccountsUser(int clientId)
        {
            return await _context.Accounts.Where(account => account.ClientId == clientId).Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                ClientName = a.Client == null ? "" : a.Client.Name!,
                Balance = a.Balance,
                RegDate = a.RegDate

            }).SingleOrDefaultAsync();
        }

        public async Task MakeWithdrawal(WithdrawalDTO withdrawal)
        {
            var transaction = new BankTransaction
            {
                TransactionType = withdrawal.TransactionType,
                AccountId = withdrawal.AccountId,
                Amount = withdrawal.Amount,
                ExternalAccount = withdrawal.ExternalAccount
            };

            _context.BankTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task MakingDeposits(DepositDTO deposit)
        {
            var transaction = new BankTransaction
            {
                AccountId = deposit.AccountId,
                TransactionType = 1,
                Amount = deposit.Amount
            };

            _context.BankTransactions.Add(transaction);

            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetAccountById(int idAccount)
        {
            return await _context.Accounts.FindAsync(idAccount);            
        }

        public async Task DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
