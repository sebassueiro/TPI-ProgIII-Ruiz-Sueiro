using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ECommerceContext _context;

        public AdminService(ECommerceContext context)
        {
            _context = context;
        }

        public List<User> GetAdmins()
        {
            return _context.Users.Where(p => p.UserType == "Admin").ToList();
        }

        public Admin? GetAdminById(int id)
        {
            return _context.Admins.FirstOrDefault(p => p.Id == id);
        }

        public Admin UpdateAdmin(Admin admin)
        {
            _context.Update(admin);
            _context.SaveChanges();
            return admin;
        }
    }
}
