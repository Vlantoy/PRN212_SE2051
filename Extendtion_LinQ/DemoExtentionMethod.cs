using CourseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extendtion_LinQ
{
    static class DemoExtentionMethod
    {
        public static void display(this List<Course> course)
        {
            foreach (Course c in course) { 
                Console.WriteLine(c);
            }
        }
    }
}
