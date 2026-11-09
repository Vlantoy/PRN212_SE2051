using System.Collections;
using System.Runtime.InteropServices;
using ConsoleApp1;
internal class Program
{
    private static void Main(string[] args)
    {
        //DemoArrayList();
        DemoList();
    }
    public static void DemoArrayList()
    {
        ArrayList ar = new ArrayList();
        ar.Add(1);
        ar.Add(2);
        ar.Add("Hello");
        ar.Add(new Student("Hung", 20));
        int sum = (int)ar[0] + (int)ar[1];
        sum += (int)ar[2]; //Rumtime error
        Console.WriteLine(sum);
    }
    public static void DemoList()
    {
        List<Student> list = new List<Student>();
        list.Add(new Student("Hoa", 19));
        list.Add(new Student("Hang", 20));
        list.Add(new Student("Hoang", 21));
        //list.Add(new FStudent()); //Compile error
        list[0].display();
        Student s = new Student("Hoang", 21);
        if (list.Contains(s))
        {
            Console.WriteLine("Co tim thay");
        }
        else {
            Console.WriteLine("KHong tim thay");
        }

    }
}