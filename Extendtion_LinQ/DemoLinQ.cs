using CourseManager;

namespace Extendtion_LinQ
{
    internal class DemoLinQ
    {
        List<Course> list;
        public DemoLinQ()
        {
            list = new List<Course>()
            {
                new Course(7,"PRN222", new DateTime(2026,1,1)),
                new Course(5, "PRN212", new DateTime(2026, 9, 16)),
                new Course(2, "PRO192", new DateTime(2025, 9, 20)),
                new OnlineCourse(3, "CSD201", new DateTime(2025, 9, 10), "meet.com/hnjk"),
                new Course(1, "DBI202", new DateTime(2026, 9, 2)),
                new OnlineCourse(6, "PFP191", new DateTime(2026, 9, 05), "meet.com/hnjk"),
                new Course(4, "OSG202", new DateTime(2026, 9, 5))
            };
        }
        public List<Course> getAllCourse()
        {
            return list;
        }
        public Course getCourseByIdUsingMethod(int Id)
        {
            return list.First(c => c.Id == Id);
        }
        public List<Course> getCourseByStartDateUsingMethod(DateTime start, DateTime end)
        {
            return list.Where(x => x.StartDate>=start&&x.StartDate<=end).ToList();
        }
        public List<Course> getCourseByTitleUsingMethod(string partten)
        {
            return list.Where(x =>x.Title.Contains(partten)).ToList();
        }

        public Course getCourseByIdUsingQuery(int Id)
        {
            return (from c in list
                   where c.Id == Id
                   select c).First();
        }

        public void getCourseByStartDateUsingQuery(DateTime start, DateTime end)
        {
            var result  = list.Where(x => x.StartDate >= start && x.StartDate <= end)
                           .Select(x=>(x.Title,x.StartDate));
            foreach(var c in result)
            {
                Console.WriteLine($"{c.Title} - {c.StartDate.ToString("dd/MM/yyyy")}");
            }
        }
        //public List<Course> getCourseByStartDateUsingQuery(DateTime start, DateTime end)
        //{
        //    return (from c in list
        //           where c.StartDate>=start && c.StartDate<=end
        //           select c).ToList();
        //}
        public List<Course> getCourseByTitleUsingQuery(string partten)
        {
            return (from c in list
                   where c.Title.Contains(partten)
                   select c).ToList();
        }
    }
}
