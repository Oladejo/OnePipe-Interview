using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePipe.Core.Entities;
using OnePipe.Core.Enum;

namespace OnePipe.API.Models
{
    public class PermissionHelper
    {
        public static List<Permissions> GetEmployeePermissions(string employeeType)
        {
            if (employeeType.ToLower() == EmployeeType.Employee.ToString().ToLower())
            {
                return new List<Permissions> {new Permissions
                {
                    Code = "EMP-Read",
                    EmployeeTypes = new List<string> { employeeType},
                    Name = "REad Employee"
                },
                    new Permissions
                    {
                        Code = "EMP-Write",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Write Employee"
                    }
                };
            }
            if (employeeType.ToLower() == EmployeeType.Administrative.ToString().ToLower())
            {
                return new List<Permissions> {new Permissions
                    {
                        Code = "EMP-Add",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Add Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-Delete",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Delete Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-ReadAll",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Read All Employee"
                    }
                };
            }
            if (employeeType.ToLower() == EmployeeType.Manager.ToString().ToLower())
            {
                return new List<Permissions> {new Permissions
                    {
                        Code = "EMP-MNG-Edit",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Edit Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-MNG-View",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "View Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-Read",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Read Employee"
                    }
                };
            }
            if (employeeType.ToLower() == EmployeeType.HR.ToString().ToLower())
            {
                return new List<Permissions> {new Permissions
                    {
                        Code = "EMP-HR-Edit",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Edit Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-HR-View",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "View Employee"
                    },
                    new Permissions
                    {
                        Code = "EMP-HR-Read",
                        EmployeeTypes = new List<string> { employeeType},
                        Name = "Read Employee"
                    }
                };
            }

            return new List<Permissions>();
        }
    }
}
