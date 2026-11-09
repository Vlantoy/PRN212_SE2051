using System.Collections.Generic;

namespace EmployeeManager.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        // Navigation property for 1-to-many relationship
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public override string ToString()
        {
            return DepartmentName;
        }
    }
}
