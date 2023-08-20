using BankAPI.BankModels;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class ClientService
    {
        private readonly BankContext _context;

        public ClientService(BankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetClientById(int id)
        {
            return  await _context.Clients.FindAsync(id);
        }

        public async Task<Client> CreateClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task UpdateClient(Client client)
        {
            var existingClient =  await GetClientById(client.Id);

            if (existingClient is not null)
            {
                existingClient.Name = client.Name;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Email = client.Email;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteClient(int id)
        {
            var clientToDelete = await GetClientById(id);

            if (clientToDelete is not null)
            {
                _context.Clients.Remove(clientToDelete); 
                await _context.SaveChangesAsync();
            }
        }
    }
}
