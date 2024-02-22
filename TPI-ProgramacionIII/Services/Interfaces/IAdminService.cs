using TPI_ProgramacionIII.Data.Entities;

namespace TPI_ProgramacionIII.Services.Interfaces
{
    public interface IAdminService
    {
        List<User> GetAdmins();
        Admin GetAdminById(int id);
        Admin UpdateAdmin(Admin admin);
    }
}
