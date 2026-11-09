using System;
using System.Linq;

namespace AppManager.Models.Employee
{
    public class EmployeeItem
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }

        private string? _gender;
        public string? Gender
        {
            get => !string.IsNullOrEmpty(_gender) ? _gender : InferGender(FirstName, MiddleName, LastName);
            set => _gender = value;
        }

        public virtual Department? Department { get; set; }
        public string DepartmentName => Department?.DepartmentName ?? string.Empty;

        public static string InferGender(string? firstName, string? middleName, string? lastName)
        {
            string mid = (middleName ?? "").Trim().ToLower();
            string first = (firstName ?? "").Trim().ToLower();

            if (mid.Contains("thị") || mid.Contains("thi"))
                return "Female";

            if (mid.Contains("văn") || mid.Contains("van"))
                return "Male";

            string[] femaleKeywords = new[]
            {
                "mary", "jane", "sarah", "emma", "lisa", "anna", "emily", "jessica", "maria", "grace",
                "hoa", "lan", "mai", "linh", "trang", "hương", "ngọc", "thảo", "nga", "hằng", 
                "yến", "phương", "chi", "dương", "hà", "hạnh", "ngân", "dung", "huyền", "tuyết"
            };

            if (femaleKeywords.Contains(first))
                return "Female";

            return "Male";
        }
    }
}
