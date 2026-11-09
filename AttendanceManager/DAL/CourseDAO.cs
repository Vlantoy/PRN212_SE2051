using System.Collections.Generic;
using System.Linq;
using AttendanceManager.Data;
using AttendanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.DAL
{
    public class CourseDAO
    {
        public static List<Course> GetAllCourses()
        {
            using var context = new AppDbContext();
            return context.Courses
                .AsNoTracking()
                .OrderBy(c => c.CourseId)
                .ToList();
        }
    }
}
