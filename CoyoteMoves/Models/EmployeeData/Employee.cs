using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.EmployeeData
{
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public int Id { get; set; }
        public string ManagerName { get; set; }
        public string Template { get; set; } 
        public string SecurityItemRights { get; set; }
    }
}