using OnePipe.Core;
using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Repositories;
using OpePipe.Data.Implementation;
using Scrutor.AspNetCore;
using System;
using System.Threading.Tasks;

namespace OpePipe.Data
{
    public class UnitOfWork : IUnitOfWork, IScopedLifetime
    {
        private readonly IOnePipeDatabaseSetting _settings;
        public UnitOfWork(IOnePipeDatabaseSetting settings)
        {
            _settings = settings;
        }

        private IEmployeeManagerRepository _employeeManagerRepository;
        private IPermissionsRepository _permissionsRepository;
        private ISalaryHistoryRepository _salaryHistoryRepository;
        private IUserClaimRepository _userClaimRepository;
        private IUserRepository _userRepository;
        private IUserRoleRepository _userRoleRepository;


        public IEmployeeManagerRepository EmployeeManager => _employeeManagerRepository ?? new EmployeeManagerRepository(_settings);
        public IPermissionsRepository Permission => _permissionsRepository ?? new PermissionsRepository(_settings);
        public ISalaryHistoryRepository SalaryHistory => _salaryHistoryRepository ?? new SalaryHistoryRepository(_settings);
        public IUserClaimRepository UserClaim => _userClaimRepository ?? new UserClaimRepository(_settings);
        public IUserRepository User => _userRepository ?? new UserRepository(_settings);
        public IUserRoleRepository UserRole => _userRoleRepository ?? new UserRoleRepository(_settings);

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
