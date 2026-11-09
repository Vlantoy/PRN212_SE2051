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
        public void addCourseList()
        {
            while (true)
            {
                Console.WriteLine("Enter course type: Course(C) or OnlineCourse(O) or Exit(E)?");
                string type = Console.ReadLine();
                Course c;
                if (type.ToUpper().Equals("C"))
                {
                    c = new Course();
                    c.input();
                    courses.Add(c);
                }
                else if (type.ToUpper().Equals("O"))
                {
                    c = new OnlineCourse();
                    c.input();
                    courses.Add(c);
                }
                else if (type.ToUpper().Equals("E"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format!");
                    continue;
                }
                //courses.Add(c);
            }

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
            courses.Add(new Course(3, "CSD201", new DateTime(2025, 9, 10)));
            courses.Add(new Course(1, "DBI202", new DateTime(2026, 9, 2)));
            courses.Add(new Course(4, "OSG202", new DateTime(2026, 9, 5)));
        }
        public void readFromFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Course c = null;
                if (line[0] == 'C')
                {
                    c = new Course();

                }
                else
                {
                    c = new OnlineCourse();
                }
                c.readFromLine(line);
                courses.Add(c);
            }
            reader.Close();
        }

        public void searchCourseByStartDate()
        {
            Console.WriteLine("Input startDate:");
            DateTime start = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            Console.WriteLine("Input endDate:");
            DateTime end = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            search(start, end);
        }

        private void search(DateTime start, DateTime end)
        {
            foreach (Course c in courses)
            {
                if (c.StartDate >= start && c.StartDate <= end)
                {
                    Console.WriteLine(c);
                }
            }
        }
        public void SortByTitle()
        {
            //courses.Sort();
            courses.Sort(new TitleComparer());
        }
        public void SortByStartDate()
        {
            courses.Sort(new StartDateComparer());

        }
        public void SortById()
        {
            courses.Sort(new IdComparer());
        }
    }

}
