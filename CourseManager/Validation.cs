using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class Validation
    {
        public static int getInt(int min, int max, string message)
        {
            int result;
            while (true)
            {
                try
                {
                    Console.WriteLine(message);
                    result = Int32.Parse(Console.ReadLine());
                    if (result < min || result > max)
                        throw new OverflowException("Number is Overflow!");
                    return result;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Wrong format!");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        public static string getString(int minLength, int maxLength, string mess)
        {
            string result;
            while (true)
            {
                try
                {
                    Console.WriteLine(mess);
                    result = Console.ReadLine().Trim();
                    if (result.Length < minLength || result.Length > maxLength)
                        throw new OverflowException("String is Overflow!");
                    return result;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("String is wrong format!");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static DateTime getDate(DateTime start, DateTime end, string pattern, string mess)
        {
            DateTime result;
            while (true)
            {
                try
                {
                    Console.WriteLine(mess);
                    result = DateTime.ParseExact(Console.ReadLine(), pattern, null);
                    if (result < start || result > end) throw new OverflowException();
                    return result;

                }
                catch (FormatException e)
                {
                    Console.WriteLine("Wrong format!");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("Overflow date!");
                }
            }
        }
        }
}
