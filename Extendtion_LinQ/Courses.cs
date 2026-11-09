using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class Course
    {
        public Course()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        public Course(int id, string title, DateTime startDate)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
        }
       

        public override string? ToString()
        {
            return $"Course: {Id} - {Title} - {StartDate}";
        }


        //public int CompareTo(Course? other)
        //{
        //    return Title.CompareTo(other.Title);
        //}

       
    }
    class OnlineCourse : Course
    {
        public OnlineCourse()
        {
        }

        public OnlineCourse(int id, string title, DateTime startDate, string linkMeet) : base(id, title, startDate)
        {
            LinkMeet = linkMeet;
        }

        string LinkMeet {  get; set; }

        public override string? ToString()
        {
            return base.ToString() + $"{LinkMeet}";
        }
    }
}
