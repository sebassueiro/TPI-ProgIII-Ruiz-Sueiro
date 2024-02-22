using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.Models;

namespace TPI_ProgramacionIII.Services.Interfaces
{
    public interface IUserService
    {
        public BaseResponse UserValidation(string username, string password);
        public User? GetUserByEmail(string username);
        void DeleteUser(int userId);
        int CreateUser(User user);
    }
}
