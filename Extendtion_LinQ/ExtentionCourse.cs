using CourseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extendtion_LinQ
{
    static class ExtentionCourse
    {
        public static void Display(this Course c, int count = 1)
        {
            //Console.WriteLine($"Course's Information {count} times:");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(c);
            }
        }
    }
}
