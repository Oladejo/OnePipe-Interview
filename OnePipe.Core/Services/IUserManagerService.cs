using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePipe.Core.Services
{
    public interface IUsersManagerService
    {
        Task<string> Login(string email, string password);
        Task<ResponseMessageHandler> AddNewUser(Users user);

        //Get Employees All
        Task<List<Users>> GetUsers();
        Task<Users> GetUsers(string id);
        Task<ResponseMessageHandler> UpdateUser(string userid, Users user);
        Task<bool> AddEmployeeToManager(string userId, string managerId);
    }
}
