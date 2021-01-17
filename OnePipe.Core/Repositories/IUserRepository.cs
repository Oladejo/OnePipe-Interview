using OnePipe.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePipe.Core.Repositories
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<List<Users>> GetEmpoyeeForManager(List<string> managerId);
        Task<List<Users>> GetEmpoyeeForHR();
    }
}
