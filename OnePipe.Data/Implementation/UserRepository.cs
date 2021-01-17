using MongoDB.Driver;
using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Enum;
using OnePipe.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpePipe.Data.Implementation
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        protected readonly IMongoCollection<Users> _context;

        public UserRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<Users>(typeof(Users).Name);
        }

        public async Task<List<Users>> GetEmpoyeeForManager(List<string> managerId)
        {
            var result = _context.AsQueryable<Users>().Where(x => managerId.Contains(x.Id)).ToList();
            return await Task.FromResult(result);
        }

        public async Task<List<Users>> GetEmpoyeeForHR()
        {
            var result = _context.AsQueryable<Users>().Where(x => x.EmployeeType != EmployeeType.HR).ToList();
            return await Task.FromResult(result);
        }
    }
}
