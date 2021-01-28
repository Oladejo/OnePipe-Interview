using System.Collections.Generic;
using System.Threading.Tasks;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;

namespace OnePipe.API.Services
{
    public interface IUsersManagerService
    {
        Task<ResponseMessageHandler> Login(string email, string password);
        Task<ResponseMessageHandler> AddNewUser(Users user);

        //Get Employees All
        Task<List<Users>> GetUsers();
        Task<Users> GetUsers(string id);

        Task<List<Users>> GetMyUsers(string id);

        Task<ResponseMessageHandler> UpdateUser(string userid, Users user);
        Task<bool> AddEmployeeToManager(string userId, string managerId);
        Task<List<Users>> GetHRUsers();
    }
}
