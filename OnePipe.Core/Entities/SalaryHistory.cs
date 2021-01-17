using OnePipe.Core.Entities.Base;
using System;

namespace OnePipe.Core.Entities
{
    public class SalaryHistory : Entity
    {
        public string EmployeeId { get; set; }
        public decimal Salary { get; set; }
        public DateTime Date { get; set; }
    }
}
