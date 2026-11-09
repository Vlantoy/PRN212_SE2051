using CourseManager;

internal class Program
{
    private static void Main(string[] args)
    {
        ListCourse listCourse = new ListCourse();
        //listCourse.addCourseList();
        //listCourse.initData();
        listCourse.readFromFile($"courses.txt");
        Console.WriteLine("List of courses:");
        listCourse.displayListCourse();
        
        //listCourse.searchCourseByStartDate();
        //Console.WriteLine("List of coures you have found:");
        Console.WriteLine("List of courses after sorted by Title:");
        listCourse.SortByTitle();
        listCourse.displayListCourse();
        Console.WriteLine("List of courses after sorted by StartDate:");
        listCourse.SortByStartDate();
        listCourse.displayListCourse();
        Console.WriteLine("List of courses after sorted by Id:");
        listCourse.SortById();
        listCourse.displayListCourse();

    }
}