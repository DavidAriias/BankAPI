using BankAPI.BankModels;
using BankAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class LoginService
    {
        private readonly BankContext _bankContext;
        public LoginService(BankContext context) 
        {
            _bankContext = context;
        }

        public async Task<Administrator?> GetAdmin(AdminDTO admin)
        {
            return await _bankContext.Administrators
                .SingleOrDefaultAsync(x => x.Email == admin.Email && x.Pwd == admin.Pwd);
        }

        public async Task<Client?> GetClient(ClientDTO client)
        {
            return await _bankContext.Clients
                .SingleOrDefaultAsync(x => x.Email == client.Email && x.Pwd == client.Pwd && x.Name == client.Name);
        }
    }
}
