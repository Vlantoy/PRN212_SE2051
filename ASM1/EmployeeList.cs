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

        public bool IsIdExisted(int id)
        {
            return employees.Any(e => e.Id == id);
        }

        public bool AddEmployee(Employee e)
        {
            if (IsIdExisted(e.Id))
            {
                Console.WriteLine($"Error: Employee with ID {e.Id} already exists!");
                return false;
            }
            employees.Add(e);
            return true;
        }

        public void inputEmployeeList()
        {
            Console.Write("Enter number of employees to add: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"\n--- Input Employee #{i + 1} ---");
                    Employee e = new Employee();
                    while (true)
                    {
                        e.Input();
                        if (IsIdExisted(e.Id))
                        {
                            Console.WriteLine($"ID {e.Id} already exists in the list! Please re-enter this employee.");
                            continue;
                        }
                        AddEmployee(e);
                        Console.WriteLine("Added successfully!");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid number of employees (must be an integer > 0).");
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
            int addedCount = 0;
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length == 4)
                {
                    if (int.TryParse(parts[0].Trim(), out int id) &&
                        id > 0 &&
                        double.TryParse(parts[2].Trim(), out double salary) &&
                        salary > 0)
                    {
                        string name = parts[1].Trim();
                        string position = parts[3].Trim();

                        string? matchPos = Array.Find(Employee.Positions, x => x.Equals(position, StringComparison.OrdinalIgnoreCase));
                        if (matchPos == null)
                        {
                            Console.WriteLine($"Warning: Invalid position '{position}' on line: {line}");
                            continue;
                        }

                        if (IsIdExisted(id))
                        {
                            Console.WriteLine($"Warning: Duplicate ID {id} found in file. Skipping line: {line}");
                            continue;
                        }

                        Employee e = new Employee
                        {
                            Id = id,
                            Name = name,
                            Salary = salary,
                            Position = matchPos
                        };
                        employees.Add(e);
                        addedCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing line (Invalid ID or Salary): {line}");
                    }
                }
            }
            Console.WriteLine($"Loaded {addedCount} employee(s) from file.");
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
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with salary in range {minSalary:N0} - {maxSalary:N0}:");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with salary in range {minSalary:N0} - {maxSalary:N0}.");
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
                Console.WriteLine($"Found {foundEmployees.Count} employee(s) with yearly salary in range {minYearlySalary:N0} - {maxYearlySalary:N0}:");
                foreach (var emp in foundEmployees)
                {
                    emp.Display(SalaryCalculator.CalculateYearlySalary);
                }
            }
            else
            {
                Console.WriteLine($"No employee found with yearly salary in range {minYearlySalary:N0} - {maxYearlySalary:N0}.");
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
