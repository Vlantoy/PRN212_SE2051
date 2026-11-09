using System.Collections.Generic;
using System.Linq;
using AttendanceManager.Data;
using AttendanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.DAL
{
    public class ScheduleDAO
    {
        public static List<Schedule> GetSchedulesByCourseId(int courseId)
        {
            using var context = new AppDbContext();
            return context.Schedules
                .Where(s => s.CourseId == courseId)
                .OrderBy(s => s.TeachingDate)
                .ThenBy(s => s.Slot)
                .AsNoTracking()
                .ToList();
        }
    }
}
