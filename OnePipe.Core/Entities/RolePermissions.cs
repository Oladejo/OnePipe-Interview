using System;
using System.Collections.Generic;
using System.Text;
using OnePipe.Core.Entities.Base;

namespace OnePipe.Core.Entities
{
    public class RolePermissions : Entity
    {
    
        public int Id { get; set; }

        public string RoleId { get; set; }

        public virtual Permissions Permission { get; set; }

        public int PermissionId { get; set; }


    }
}
