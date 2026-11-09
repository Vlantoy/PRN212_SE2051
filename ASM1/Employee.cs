namespace ASM1
{
    public class Employee
    {
        public static readonly string[] Positions = { "Manager", "Developer", "Tester" };

        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }     
        public string Position { get; set; }   

        public Employee()
        {
            Name = "";
            Position = "";
        }

        public Employee(int id, string name, double salary, string position)
        {
            Id = id;
            Name = name;
            Salary = salary;
            Position = position;
        }

        public void Input()
        {
            Id = ReadInt("Input Id (> 0): ");
            Name = ReadValidName("Input Name: ");
            Salary = ReadDouble("Input Salary (per month > 0): ");

            while (true)
            {
                Console.Write("Input Position (Manager/Developer/Tester): ");
                string p = (Console.ReadLine() ?? "").Trim();
                string? match = Array.Find(Positions, x => x.Equals(p, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    Position = match;
                    break;
                }
                Console.WriteLine("Invalid position! Must be Manager, Developer, or Tester.");
            }
        }
        public void Display(SalaryCaculation delObj)
        {
            Console.WriteLine($"Id: {Id} - Name: {Name} - Position: {Position}");
            Console.WriteLine($"Monthly salary: {Salary:N0}");
            Console.WriteLine($"Yearly salary : {delObj(Salary, Position):N0}");
        }

        private static int ReadInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (int.TryParse(Console.ReadLine(), out int v) && v > 0) return v;
                Console.WriteLine("Wrong format! Please input a positive integer (> 0).");
            }
        }

        private static string ReadValidName(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                string input = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrEmpty(input) && input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    return input;
                }
                Console.WriteLine("Wrong format! Name must not be empty and can only contain letters and spaces.");
            }
        }

        private static double ReadDouble(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (double.TryParse(Console.ReadLine(), out double v) && v > 0) return v;
                Console.WriteLine("Wrong format! Please input a positive number (> 0).");
            }
        }
    }
}
