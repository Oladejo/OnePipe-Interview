using OnePipe.Core.Entities.Base;
using System;

namespace OnePipe.Core.Entities
{
    public class SalaryHistory : Entity
    {
   
        public string Type { get; set; }

        public DateTime DateEarned { get; set; }

        public decimal Amount { get; set; }
    }


}
