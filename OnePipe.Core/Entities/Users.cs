using Microsoft.AspNetCore.Identity.MongoDB;
using OnePipe.Core.Enum;
using System;
using System.Collections.Generic;

namespace OnePipe.Core.Entities
{
    public class Users : IdentityUser
    {
        public Users()
        {
            Permissions = new List<Permissions>();
            EmployeeManager = new List<EmployeeManager>();
            SalaryHistory = new List<SalaryHistory>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<Permissions> Permissions { get; set; }
        public List<EmployeeManager> EmployeeManager { get; set; }
        public decimal Salary { get; set; }
        public decimal AnnualBonus { get; set; }
        public decimal VacationBalance { get; set; }
        public List<SalaryHistory> SalaryHistory { get; set; }

        public string ManagedById { get; set; }

        public string UserType { get; set; }
    }
}
