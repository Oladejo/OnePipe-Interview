using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Repositories;

namespace OpePipe.Data.Implementation
{
    public class EmployeeManagerRepository : Repository<EmployeeManager>, IEmployeeManagerRepository
    {
        public EmployeeManagerRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
        }
    }
}
