using OnePipe.Core.Repositories;
using System;

namespace OnePipe.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeManagerRepository EmployeeManager { get; }
        IPermissionsRepository Permission { get; }
        ISalaryHistoryRepository SalaryHistory { get; }
        IUserClaimRepository UserClaim { get; }
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
    }
}
