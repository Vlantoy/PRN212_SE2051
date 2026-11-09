using System;

namespace AttendanceManager.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int ScheduleId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; } = false;
        public string? Note { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Schedule? Schedule { get; set; }
        public Student? Student { get; set; }
    }
}
