using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Models;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ECommerceContext _context;

        public UserService(ECommerceContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public BaseResponse UserValidation(string email, string password)
        {
            BaseResponse response = new BaseResponse();

            if (email == "string" || password == "string")
            {
                response.Result = false;
                response.Message = "Por favor, ingrese email y contraseña";
                return response;
            }

            User? userForLogin = _context.Users.FirstOrDefault(u => u.Email == email);
            if (userForLogin != null)
            {
                if (userForLogin.Password == password)
                {
                    response.Result = true;
                    response.Message = "registro exitoso";
                }
                else
                {
                    response.Result = false;
                    response.Message = "contraseña incorrecta";
                }
            }
            else
            {
                response.Result = false;
                response.Message = "email incorrecto";
            }
            return response;
        }


        public void DeleteUser(int Id)
        {
            User? userToDelete = _context.Users.FirstOrDefault(u => u.Id == Id);
            userToDelete.State = false;
            _context.Update(userToDelete);
            _context.SaveChanges();

        }

        public int CreateUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
    }
}
