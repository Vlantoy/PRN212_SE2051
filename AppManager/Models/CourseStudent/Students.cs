namespace AppManager.Models.CourseStudent
{
    public class Students
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

        public int StudentID { get; set; }
        public string RollNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
