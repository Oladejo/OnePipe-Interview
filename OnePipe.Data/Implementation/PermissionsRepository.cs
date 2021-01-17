using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Repositories;

namespace OpePipe.Data.Implementation
{
    public class PermissionsRepository : Repository<Permissions>, IPermissionsRepository
    {
        public PermissionsRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
        }
    }
}
