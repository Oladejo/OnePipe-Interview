using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Repositories;

namespace OpePipe.Data.Implementation
{
    public class SalaryHistoryRepository : Repository<SalaryHistory>, ISalaryHistoryRepository
    {
        public SalaryHistoryRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
        }
    }
}
