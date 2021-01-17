using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Entities;
using OnePipe.Core.Repositories;

namespace OpePipe.Data.Implementation
{
    public class UserClaimRepository : Repository<UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(IOnePipeDatabaseSetting settings) : base(settings)
        {
        }
    }
}
