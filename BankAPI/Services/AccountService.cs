using BankAPI.BankModels;
using BankAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class AccountService
    {
        private readonly BankContext _bankContext;

        public AccountService(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<IEnumerable<AccountDtoOut>> GetAll()
        {
            return await _bankContext.Accounts.Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                ClientName = a.Client == null ? "" : a.Client.Name,
                Balance = a.Balance,
                RegDate = a.RegDate

            }).ToListAsync();
        }

        public async Task<AccountDtoOut?> GetDtoById(int Id)
        {
            return await _bankContext.Accounts.Where(a => a.Id == Id).Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                ClientName = a.Client == null ? "" : a.Client.Name,
                Balance = a.Balance,
                RegDate = a.RegDate

            }).SingleOrDefaultAsync();
        }
        public async Task<Account?> GetById(int id)
        {
            return await _bankContext.Accounts.FindAsync(id);
        }

        public async Task<Account> Create(AccountDTOIn newAccount)
        {
            Account account = new Account();
            
            account.AccountType = newAccount.AccountType;
            account.ClientId = newAccount.ClientId;
            account.Balance = newAccount.Balance;

            _bankContext.Accounts.Add(account);
            await _bankContext.SaveChangesAsync();

            return account;
        }

        public async Task Update(AccountDTOIn account)
        {
            var existingAccount = await GetById(account.Id);

            if (existingAccount is not null)
            {
                existingAccount.AccountType = account.AccountType;
                existingAccount.ClientId = account.ClientId;
                existingAccount.Balance = account.Balance;

                await _bankContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var accountToDelete = await GetById(id);

            if (accountToDelete is not null)
            {
                _bankContext.Accounts.Remove(accountToDelete);
                await _bankContext.SaveChangesAsync();
            }
        }
    }
}
