using CourseManager;
using DemoDeletage;

internal class Program
{
    private static void Main(string[] args)
    {
        //DemoBasicDelegate();
        //DemoMultiCast();
        DemoComparion();
    }
    public static int excersice(int x, int y)
    {
        Console.WriteLine("This is Excersice function!");
        return x;
    }
    public static int sum(int a, int b)
    {
        Console.WriteLine("This is sum function");
        return a + b;
    }
    public static void cacul(int x, int y, Caculation caculation)
    {
        caculation(x,y);
    }
    public static void DemoBasicDelegate()
    {
        Caculation caculate;//Khai bao 1 bien co kieu Caculation
        //caculate = new Caculation(sum);
        //caculate = new Caculation(excersice);
        //caculate = sum;
        //Console.WriteLine("Output:"+ caculate(5,8));
        cacul(10,20,excersice);

    }
    public static void DemoMultiCast()
    {
        Console.WriteLine("Demo Multicast:");
        Caculation dele = sum;
        dele += excersice;
        dele += sum;
        dele -= sum;
        dele -= sum;
        dele -= sum;
        dele -= excersice;
        //Console.WriteLine("Output:"+dele(8,5));runtime error
    }
    public static void DemoComparion()
    {
        ListCourse list = new ListCourse();
        list.initData();
        Console.WriteLine("List of courses after sorted by title:");
            list.SortByTitle();
        list.displayListCourse();
    }

}