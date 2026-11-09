using System;
using System.Linq;

namespace EmployeeManager.Models
{
    public class Employee
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

        // Navigation property
        public virtual Department? Department { get; set; }

        // Helper property to easily show Department Name in bindings
        public string DepartmentName => Department?.DepartmentName ?? string.Empty;

        /// <summary>
        /// Code tự động suy ra Gender dựa vào Name theo yêu cầu của đề:
        /// - Nếu tên đệm có "Thị" -> Female
        /// - Nếu tên đệm có "Văn" -> Male
        /// - Dò theo danh sách tên nữ phổ biến (Jane, Sarah, Emma, Hoa, Lan, Linh, Trang...)
        /// </summary>
        public static string InferGender(string? firstName, string? middleName, string? lastName)
        {
            string mid = (middleName ?? "").Trim().ToLower();
            string first = (firstName ?? "").Trim().ToLower();

            // Quy tắc tiếng Việt
            if (mid.Contains("thị") || mid.Contains("thi"))
                return "Female";

            if (mid.Contains("văn") || mid.Contains("van"))
                return "Male";

            // Danh sách các tên nữ phổ biến (Anh + Việt)
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
