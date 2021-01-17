using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Repositories;

namespace OpePipe.Data.Implementation
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
        }
    }
}
