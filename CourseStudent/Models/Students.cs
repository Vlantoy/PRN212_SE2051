using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStudent.Models
{
    internal class Students
    {
        public Students()
        {
        }

        public Students(int studentID, string rollNo, string firstName, string midName, string lastName)
        {
            StudentID = studentID;
            RollNo = rollNo;
            FirstName = firstName;
            MidName = midName;
            LastName = lastName;
        }

        public int StudentID {  get; set; }
        public string RollNo { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }

    }
}
