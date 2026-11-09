using System.Security;

namespace CourseManager
{
    internal class ListCourse
    {
        List<Course> courses;

        public ListCourse()
        {
            courses = new List<Course>();
        }
       
        public void displayListCourse()
        {
            foreach (Course c in courses)
            {
                Console.WriteLine(c);
            }
        }
        public void initData()
        {
            courses.Add(new Course(5, "PRN212", new DateTime(2026, 9, 16)));
            courses.Add(new Course(2, "PRO192", new DateTime(2025, 9, 20)));
            courses.Add(new OnlineCourse(3,"CSD201", new DateTime(2025, 9, 10),"meet.com/hnjk"));
            courses.Add(new Course(1, "DBI202", new DateTime(2026, 9, 2)));
            courses.Add(new OnlineCourse(6, "PFP191", new DateTime(2026, 9, 05), "meet.com/hnjk"));
            courses.Add(new Course(4, "OSG202", new DateTime(2026, 9, 5)));
        }
        //public int CompareTitle(Course x, Course y)
        //{
        //    return x.Title.CompareTo(y.Title);
        //}
        public int CompareStartDate(Course x, Course y)
        {
            return x.StartDate.CompareTo(y.StartDate);
        }
        public void SortByTitle()
        {
            //courses.Sort();
            //Anonymous function
            //courses.Sort(delegate (Course x, Course y)
            //{
            //    return x.Title.CompareTo (y.Title);
            //});

            courses.Sort((x,y) => x.Title.CompareTo(y.Title));
        }
        public void SortByStartDate()
        {
            courses.Sort(CompareStartDate);

        }
        public void SortById()
        {
            courses.Sort((x,y)=>x.Id.CompareTo(y.Id));
        }
    }

}
