using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1
{
    public class EmployeeList
    {
        private List<Employee> employees = new List<Employee>();

        public void AddEmployee(Employee e)
        {
            employees.Add(e);
        }

        public void inputEmployeeList()
        {
            Console.Write("Enter number of employees: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"\n--- Input Employee #{i + 1} ---");
                    Employee e = new Employee();
                    e.Input();
                    AddEmployee(e);
                }
            }
            else
            {
                Console.WriteLine("Invalid number of employees.");
            }
        }

        public void inputEmployeeFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length == 4)
                {
                    if (int.TryParse(parts[0].Trim(), out int id) &&
                        double.TryParse(parts[2].Trim(), out double salary))
                    {
                        Employee e = new Employee
                        {
                            Id = id,
                            Name = parts[1].Trim(),
                            Salary = salary,
                            Position = parts[3].Trim()
                        };
                        AddEmployee(e);
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing line: {line}");
                    }
                }
            }
        }

        public void searchEmployeeByName(string name)
        {
            var foundEmployees = employees.Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (foundEmployees.Count > 0)
            {
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with name '{name}':");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with name '{name}'.");
            }
        }

        public void searchEmployeeById(int id)
        {
            var foundEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (foundEmployee != null)
            {
                Console.WriteLine($"Found employee with ID '{id}':");
                foundEmployee.Display(SalaryCalculator.CalculateYearlySalary);
            }
            else
            {
                Console.WriteLine($"No employee found with ID '{id}'.");
            }
        }

        public void searchEmployeeByPosition(string position)
        {
            var foundEmployees = employees.Where(e => e.Position.Equals(position, StringComparison.OrdinalIgnoreCase)).ToList();
            if (foundEmployees.Count > 0)
            {
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with position '{position}':");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with position '{position}'.");
            }
        }

        public void searchEmployeeSalaryInRange(double minSalary, double maxSalary)
        {
            var foundEmployees = employees.Where(e => e.Salary >= minSalary && e.Salary <= maxSalary).ToList();
            if (foundEmployees.Count > 0)
            {
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with salary in range {minSalary} - {maxSalary}:");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with salary in range {minSalary} - {maxSalary}.");
            }
        }

        public void searchEmployeeByYearlySalaryInRange(double minYearlySalary, double maxYearlySalary)
        {
            var foundEmployees = employees.Where(e =>
            {
                double yearlySalary = SalaryCalculator.CalculateYearlySalary(e.Salary, e.Position);
                return yearlySalary >= minYearlySalary && yearlySalary <= maxYearlySalary;
            }).ToList();
            if (foundEmployees.Count > 0)
            {
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with yearly salary in range {minYearlySalary} - {maxYearlySalary}:");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with yearly salary in range {minYearlySalary} - {maxYearlySalary}.");
            }
        }

        public void DisplayAllEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("Employee list is empty.");
                return;
            }

            Console.WriteLine($"=== Total Employees: {employees.Count} ===");
            foreach (var emp in employees)
            {
                emp.Display(SalaryCalculator.CalculateYearlySalary);
                Console.WriteLine("-----------------------------------");
            }
        }
    }
}
