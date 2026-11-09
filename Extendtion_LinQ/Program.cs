using CourseManager;
using Extendtion_LinQ;

internal class Program
{
    private static void Main(string[] args)
    {
        //demoExtentionMethod();
        //demoCourseExtention();
        demoLinQUsingMethod();
    }
    public static void demoExtentionMethod()
    {
        List<Course> list = new List<Course> {
            new Course(7,"PRN222", new DateTime(2026,1,1)),
            new Course(5, "PRN212", new DateTime(2026, 9, 16)),
        new Course(2, "PRO192", new DateTime(2025, 9, 20)),
        new OnlineCourse(3, "CSD201", new DateTime(2025, 9, 10), "meet.com/hnjk"),
        new Course(1, "DBI202", new DateTime(2026, 9, 2)),
        new OnlineCourse(6, "PFP191", new DateTime(2026, 9, 05), "meet.com/hnjk"),
        new Course(4, "OSG202", new DateTime(2026, 9, 5))
    };
        list.display();
    }
    public static void demoCourseExtention()
    {
        Course c = new Course(1, "PRN212", new DateTime(2026,9,28));
        c.Display(5);
    }
    public static void demoLinQUsingMethod()
    {
        DemoLinQ demo = new DemoLinQ();
        Console.WriteLine("All courses:");
        demo.getAllCourse().display();
        Console.WriteLine("Course with Title using method:");
        demo.getCourseByTitleUsingMethod("P").display();
        Console.WriteLine("Course with Id using method:");
        demo.getCourseByIdUsingMethod(2).Display();
        Console.WriteLine("Course with (start - end) date using method:");
        demo.getCourseByStartDateUsingMethod(new DateTime(2026,1,1), new DateTime(2026,12,31)).display();
        Console.WriteLine("Course with Title using query:");
        demo.getCourseByTitleUsingQuery("P").display();
        Console.WriteLine("Course with Id using query:");
        demo.getCourseByIdUsingQuery(2).Display();
        Console.WriteLine("Course with (start - end) date using query:");
        //demo.getCourseByStartDateUsingQuery(new DateTime(2026, 1, 1), new DateTime(2026, 12, 31)).display();
        demo.getCourseByStartDateUsingQuery(new DateTime(2026, 1, 1), new DateTime(2026, 12, 31));
    }
}