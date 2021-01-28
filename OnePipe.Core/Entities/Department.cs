using System;
using System.Collections.Generic;
using System.Text;
using OnePipe.Core.Entities.Base;

namespace OnePipe.Core.Entities
{
    public class Department : Entity
    {
        public new int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }

    }
}
