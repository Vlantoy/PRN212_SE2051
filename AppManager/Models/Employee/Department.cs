using System.Collections.Generic;

namespace AppManager.Models.Employee
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        public virtual ICollection<EmployeeItem> Employees { get; set; } = new List<EmployeeItem>();

        public override string ToString()
        {
            return DepartmentName;
        }
    }
}
