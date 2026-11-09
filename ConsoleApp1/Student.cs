using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Student
    {
        public string name;
        public int age;

        public Student(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public virtual void input()
        {
            Console.WriteLine("Input name:");
            name = Console.ReadLine();
            Console.WriteLine("Input age:");
            age = Convert.ToInt32(Console.ReadLine());
        }

        public override string? ToString()
        {
            return $"Name: {name} - Age: {age}" ;
        }

        public virtual void display()
        {
            Console.WriteLine($"Name: {name} - Age: {age}");
        }
        public static void Main(string[] args)
        {
            Student s = new Student("Test name",20);
            Student s2 = new Student("Test name", 20);
            if (s.Equals(s2))
            {
                Console.WriteLine("Bang nhau");
            }
            else
            {
                Console.WriteLine("Khong bang nhau");
            }
            Console.WriteLine(s);
            //Student s = new Student();
            //s.input();
            //s.display();
            //Student f = new FStudent();
            //f.input();
            //f.display();
            //s.name = "Test name";


        }

        public override bool Equals(object? obj)
        {
            return obj is Student s &&
                   name == s.name &&
                   age == s.age;
        }
    }
    public class FStudent
    {
        public string name;
        //public string country;
        //public override void input()
        //{
        //    base.input();
        //    Console.WriteLine("Input country:");
        //    country = Console.ReadLine();

        //}

        //public override void display()
        //{
        //    base.display();
        //    Console.WriteLine($"country: {country}");
        //}

    }


}
