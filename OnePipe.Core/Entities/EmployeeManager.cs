using OnePipe.Core.Entities.Base;

namespace OnePipe.Core.Entities
{
    public class EmployeeManager : Entity
    {
        public string ManagerId { get; set; }
        public string EmployeeId { get; set; }

    }
}
