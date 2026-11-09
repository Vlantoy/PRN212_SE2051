using System;
using System.Collections.Generic;

namespace AttendanceManager.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int CourseId { get; set; }
        public DateTime TeachingDate { get; set; }
        public int Slot { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;

        public Course? Course { get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public string DisplayText => $"Slot {Slot} ({TimeSlot}) | Ngày {TeachingDate:dd/MM/yyyy} | Phòng {Room}";

        public override string ToString() => DisplayText;
    }
}
