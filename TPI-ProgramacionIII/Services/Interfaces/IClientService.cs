using TPI_ProgramacionIII.Data.Entities;

namespace TPI_ProgramacionIII.Services.Interfaces
{
    public interface IClientService
    {
        List<User> GetClients();
        Client GetClientById(int id);
        Client UpdateClient(Client client);
    }
}
