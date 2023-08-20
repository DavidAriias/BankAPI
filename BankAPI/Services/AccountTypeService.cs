using BankAPI.BankModels;

namespace BankAPI.Services
{
    public class AccountTypeService
    {
        private readonly BankContext _bankContext;
        public AccountTypeService(BankContext context) 
        {
            _bankContext = context;
        }

        public async Task<AccountType?> GetById(int id)
        {
            return await _bankContext.AccountTypes.FindAsync(id);
        }
    }
}
