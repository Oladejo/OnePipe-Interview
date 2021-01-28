using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePipe.Core.Entities;

namespace OnePipe.API.Models
{
    public class NewUser
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public string ManagedById { get; set; }

        public List<SalaryHistory> SalaryHistories { get; set; }
    }
}
