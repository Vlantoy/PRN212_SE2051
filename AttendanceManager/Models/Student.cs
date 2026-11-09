using System.Collections.Generic;

namespace AttendanceManager.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string RollNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MidName))
                    return $"{LastName} {FirstName}".Trim();
                return $"{LastName} {MidName} {FirstName}".Trim();
            }
        }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public override string ToString() => $"{RollNumber} - {FullName}";
    }
}
