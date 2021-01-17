using OnePipe.Core.Entities.Base;
using System.Collections.Generic;

namespace OnePipe.Core.Entities
{
    public class Permissions : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> EmployeeTypes { get; set; }
    }
}
