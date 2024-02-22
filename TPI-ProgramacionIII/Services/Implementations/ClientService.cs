using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly ECommerceContext _context;

        public ClientService(ECommerceContext context)
        {
            _context = context;
        }

        public List<User> GetClients()
        {
            return _context.Users.Where(p => p.UserType == "Client").ToList();
        }

        public Client? GetClientById(int id)
        {
            return _context.Clients
            .Include(c => c.Orders)
            .SingleOrDefault(c => c.Id == id);
        }

        public Client UpdateClient(Client client)
        {
            _context.Update(client);
            _context.SaveChanges();
            return client;
        }
    }
}
