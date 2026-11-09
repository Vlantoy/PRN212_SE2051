using System.Collections.Generic;

namespace AttendanceManager.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public string DisplayText => $"{CourseCode} - {CourseName}";

        public override string ToString() => DisplayText;
    }
}
