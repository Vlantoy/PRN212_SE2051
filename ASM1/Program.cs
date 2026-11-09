using System;
using System.IO;

namespace ASM1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            EmployeeList employeeList = new EmployeeList();
            string filePath = "employees.txt";
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(AppContext.BaseDirectory, "employees.txt");
            }

            // Tự động nạp file ban đầu nếu file tồn tại
            if (File.Exists(filePath))
            {
                employeeList.inputEmployeeFromFile(filePath);
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n================== EMPLOYEE MANAGEMENT MENU ==================");
                Console.WriteLine("1. Nhập thông tin 1 Employee & In lương năm (Dùng Delegate)");
                Console.WriteLine("2. Đọc dữ liệu nhân viên từ file (employees.txt)");
                Console.WriteLine("3. Hiển thị toàn bộ danh sách nhân viên");
                Console.WriteLine("4. Tìm Employee theo ID");
                Console.WriteLine("5. Tìm Employee theo Name");
                Console.WriteLine("6. Tìm Employee theo Position (Manager/Developer/Tester)");
                Console.WriteLine("7. Tìm Employee có Salary (tháng) trong khoảng");
                Console.WriteLine("8. Tìm Employee có kết quả Salary năm trong khoảng");
                Console.WriteLine("9. Nhập thêm danh sách Employee từ bàn phím");
                Console.WriteLine("0. Thoát chương trình");
                Console.WriteLine("==============================================================");
                Console.Write("Vui lòng chọn chức năng (0-9): ");

                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=== NHẬP & HIỂN THỊ 1 EMPLOYEE ===");
                        Employee singleEmp = new Employee();
                        singleEmp.Input();
                        Console.WriteLine("\n--- THÔNG TIN EMPLOYEE VỪA NHẬP ---");
                        // Sử dụng delegate SalaryCaculation
                        singleEmp.Display(SalaryCalculator.CalculateYearlySalary);
                        break;

                    case "2":
                        Console.WriteLine("=== ĐỌC DỮ LIỆU TỪ FILE ===");
                        employeeList = new EmployeeList(); // Khởi tạo lại danh sách
                        employeeList.inputEmployeeFromFile(filePath);
                        Console.WriteLine("Đã tải dữ liệu từ file thành công!");
                        employeeList.DisplayAllEmployees();
                        break;

                    case "3":
                        Console.WriteLine("=== DANH SÁCH TẤT CẢ NHÂN VIÊN ===");
                        employeeList.DisplayAllEmployees();
                        break;

                    case "4":
                        Console.Write("Nhập ID cần tìm: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId) && searchId > 0)
                        {
                            employeeList.searchEmployeeById(searchId);
                        }
                        else
                        {
                            Console.WriteLine("ID không hợp lệ! (Phải là số nguyên dương > 0).");
                        }
                        break;

                    case "5":
                        while (true)
                        {
                            Console.Write("Nhập Name cần tìm: ");
                            string searchName = (Console.ReadLine() ?? "").Trim();
                            if (!string.IsNullOrEmpty(searchName))
                            {
                                employeeList.searchEmployeeByName(searchName);
                                break;
                            }
                            Console.WriteLine("Tên tìm kiếm không được để trống!");
                        }
                        break;

                    case "6":
                        while (true)
                        {
                            Console.Write("Nhập Position cần tìm (Manager/Developer/Tester): ");
                            string searchPos = (Console.ReadLine() ?? "").Trim();
                            string? matchPos = Array.Find(Employee.Positions, p => p.Equals(searchPos, StringComparison.OrdinalIgnoreCase));
                            if (matchPos != null)
                            {
                                employeeList.searchEmployeeByPosition(matchPos);
                                break;
                            }
                            Console.WriteLine("Chức vụ không hợp lệ! Vui lòng chọn Manager, Developer hoặc Tester.");
                        }
                        break;

                    case "7":
                        double minSal = ReadPositiveDouble("Nhập mức lương tháng nhỏ nhất (min Salary >= 0): ");
                        double maxSal;
                        while (true)
                        {
                            maxSal = ReadPositiveDouble("Nhập mức lương tháng lớn nhất (max Salary): ");
                            if (maxSal >= minSal) break;
                            Console.WriteLine($"Mức lương lớn nhất phải >= mức nhỏ nhất ({minSal:N0})!");
                        }
                        employeeList.searchEmployeeSalaryInRange(minSal, maxSal);
                        break;

                    case "8":
                        double minYearly = ReadPositiveDouble("Nhập mức lương năm nhỏ nhất (min Yearly Salary >= 0): ");
                        double maxYearly;
                        while (true)
                        {
                            maxYearly = ReadPositiveDouble("Nhập mức lương năm lớn nhất (max Yearly Salary): ");
                            if (maxYearly >= minYearly) break;
                            Console.WriteLine($"Mức lương năm lớn nhất phải >= mức nhỏ nhất ({minYearly:N0})!");
                        }
                        employeeList.searchEmployeeByYearlySalaryInRange(minYearly, maxYearly);
                        break;

                    case "9":
                        Console.WriteLine("=== NHẬP DANH SÁCH EMPLOYEE TỪ BÀN PHÍM ===");
                        employeeList.inputEmployeeList();
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("Tạm biệt! Cảm ơn bạn đã sử dụng chương trình.");
                        break;

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ! Vui lòng chọn từ 0 đến 9.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
            }
        }

        private static double ReadPositiveDouble(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (double.TryParse(Console.ReadLine(), out double v) && v >= 0)
                {
                    return v;
                }
                Console.WriteLine("Vui lòng nhập một số không âm (>= 0) hợp lệ!");
            }
        }
    }
}
