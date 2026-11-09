using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class IdComparer : IComparer<Course>
    {
        public int Compare(Course? x, Course? y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
    class TitleComparer : IComparer<Course>
    {
        public int Compare(Course? x, Course? y)
        {
            return x.Title.CompareTo(y.Title);
        }
    }
    class StartDateComparer : IComparer<Course>
    {
        public int Compare(Course? x, Course? y)
        {
            return x.StartDate.CompareTo(y.StartDate);
        }
    }
}
