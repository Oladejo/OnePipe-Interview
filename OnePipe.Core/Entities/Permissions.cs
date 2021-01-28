using OnePipe.Core.Entities.Base;
using System.Collections.Generic;

namespace OnePipe.Core.Entities
{
    public class Permissions : Entity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public List<string> EmployeeTypes { get; set; }

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }


   
}
