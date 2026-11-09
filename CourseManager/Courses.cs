using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class Course:IComparer<Course>
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
        public virtual void input()
        {
            //Console.WriteLine("Input course Id:");
            //Id = Convert.ToInt32(Console.ReadLine());
            Id = Validation.getInt(1,100, "Input course Id:");
            Console.WriteLine("Input course Title:");
            Title = Console.ReadLine();
            Console.WriteLine("Input course StartDate:");
            StartDate = Convert.ToDateTime(Console.ReadLine());
        }

        public override string? ToString()
        {
            return $"Course: {Id} - {Title} - {StartDate}";
        }


        //public int CompareTo(Course? other)
        //{
        //    return Title.CompareTo(other.Title);
        //}

        public int Compare(Course? x, Course? y)
        {
           return x.Title.CompareTo(y.Title);
        }
        public virtual void readFromLine(string line)
        {
            string[] items = line.Split("|");
            Id = int.Parse(items[1]);
            Title = items[2];
            StartDate = Convert.ToDateTime(items[3]);
        }
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

        public override void input() { 
            base.input();
            Console.WriteLine("Input OnlineCourse Linkmeet");
            LinkMeet = Console.ReadLine();
        }

        public override void readFromLine(string line)
        {
            base.readFromLine(line);
            LinkMeet = line.Substring(line.LastIndexOf("|"));
        }

        public override string? ToString()
        {
            return base.ToString() + $"{LinkMeet}";
        }
    }
}
