# -*- coding: utf-8 -*-
"""
Chương 2: Bóc tách toàn diện và phân tích từng dòng code bài tập ASM1 (Console App & Delegate)
"""

from build_docx import (
    add_header_styled, add_paragraph, add_bullet, 
    add_code_block, add_callout, add_table_styled
)

def build_chapter_2(doc):
    add_header_styled(doc, "CHƯƠNG 2: BÓC TÁCH TOÀN DIỆN VÀ PHÂN TÍCH TỪNG DÒNG CODE BÀI TẬP ASM1", level=1)
    
    add_paragraph(doc,
        "Bài tập ASM1 là bài thực hành nền tảng trong PRN212 kết hợp toàn diện các kỹ thuật lập trình C# cốt lõi: "
        "Lập trình hướng đối tượng (OOP - Encapsulation, Constructors, Properties), "
        "Ủy nhiệm hàm (Delegates & Method Group Conversion), "
        "Xử lý tập tin đĩa (File I/O & chuỗi phân cách Delimited Text), "
        "và Truy vấn dữ liệu nâng cao với Ngôn ngữ truy vấn tích hợp (LINQ - Lambda Expressions). "
        "Dưới đây là phần bóc tách chi tiết từng dòng code trong từng tệp mã nguồn của dự án."
    )
    
    # 2.1 Bản chất Delegate
    add_header_styled(doc, "2.1. Bản chất Delegate và bài toán tính lương SalaryCaculation", level=2)
    add_paragraph(doc,
        "Một trong những câu hỏi lý thuyết kinh điển khi vấn đáp đồ án là: 'Delegate là gì? Tại sao ở đây không gọi trực tiếp hàm mà phải truyền qua Delegate?'"
    )
    add_paragraph(doc,
        "Về mặt định nghĩa: Delegate trong C# là một Kiểu dữ liệu tham chiếu an toàn kiểu (Type-safe Function Pointer). "
        "Nó cho phép đóng gói một phương thức có chữ ký (Signature) xác định và đối xử với phương thức đó như một tham số truyền vào hàm."
    )
    add_paragraph(doc,
        "Khai báo trong tệp SalaryCaculation.cs:"
    )
    add_code_block(doc,
"""namespace ASM1
{
    // KHAI BÁO ỦY NHIỆM HÀM (DELEGATE)
    public delegate double SalaryCaculation(double salary, string position);
}"""
    )
    
    add_header_styled(doc, "Cơ chế vận hành dưới gầm CLR:", level=3)
    add_paragraph(doc,
        "Khi trình biên dịch Roslyn nhìn thấy từ khóa 'delegate', nó không tạo ra một hàm thông thường mà tự động sinh ra một Class ẩn kế thừa từ System.MulticastDelegate (kế thừa gián tiếp từ System.Delegate). Lớp này sở hữu 3 trường nội bộ then chốt:"
    )
    add_bullet(doc, "Lưu tham chiếu đến thể hiện đối tượng sở hữu phương thức. Nếu phương thức được trỏ đến là phương thức tĩnh (static method) như SalaryCalculator.CalculateYearlySalary, trường này nhận giá trị null.", "1. _target (System.Object): ")
    add_bullet(doc, "Lưu con trỏ địa chỉ bộ nhớ nhị phân trỏ thẳng đến khối mã máy của phương thức trong vùng nhớ phương thức (Method Table). Khi ta gọi delObj(salary, position), CPU nhảy trực tiếp tới địa chỉ này.", "2. _methodPtr (System.IntPtr): ")
    add_bullet(doc, "Một mảng chứa danh sách các delegate con. Cho phép hỗ trợ kỹ thuật Multicast Delegate (gộp nhiều hàm vào một ủy nhiệm thông qua toán tử +=).", "3. _invocationList (System.Object[]): ")

    add_callout(doc, "Tại sao đề bài bắt buộc dùng Delegate cho Display()?",
        "Đây là ứng dụng thực tế của nguyên lý O trong SOLID (Open/Closed Principle - Mở rộng tính năng nhưng Đóng việc sửa mã nguồn):\n"
        "Đối tượng Employee chỉ chịu trách nhiệm chứa dữ liệu nhân viên và in ra màn hình. "
        "Employee KHÔNG ĐƯỢC BIẾT công thức tính lương năm cụ thể là gì (vì mỗi năm công ty có thể thay đổi cách tính thưởng cho Manager, Developer, Tester).\n"
        "Nhờ nhận một SalaryCaculation delegate, Employee.Display() giao việc tính toán cho bên ngoài. "
        "Khi phòng nhân sự ra công thức thưởng mới, lập trình viên chỉ cần viết một hàm mới và truyền vào, mà KHÔNG CẦN CHẠM VÀO MỘT DÒNG CODE NÀO CỦA CLASS EMPLOYEE!",
        "info"
    )

    # 2.2 Phân tích Employee.cs
    add_header_styled(doc, "2.2. Phân tích từng dòng mã nguồn Employee.cs", level=2)
    add_paragraph(doc, "Dưới đây là toàn bộ mã nguồn của lớp thực thể Employee kèm chú giải phân tích chuyên sâu:")
    
    add_code_block(doc,
"""namespace ASM1
{
    public class Employee
    {
        // 1. MẢNG HẰNG SỐ CHỨA DANH MỤC CHỨC VỤ HỢP LỆ
        public static readonly string[] Positions = { "Manager", "Developer", "Tester" };

        // 2. CÁC THUỘC TÍNH TỰ ĐỘNG (AUTO-IMPLEMENTED PROPERTIES)
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Position { get; set; }

        // 3. CONSTRUCTOR MẶC ĐỊNH (DEFAULT CONSTRUCTOR)
        public Employee()
        {
            Name = "";
            Position = "";
        }

        // 4. CONSTRUCTOR CÓ THAM SỐ (PARAMETERIZED CONSTRUCTOR)
        public Employee(int id, string name, double salary, string position)
        {
            Id = id;
            Name = name;
            Salary = salary;
            Position = position;
        }"""
    )
    
    add_paragraph(doc, "Phân tích các dòng từ 1 đến 25:", bold_prefix="Cơ chế ngôn ngữ: ")
    add_bullet(doc, "Tại sao dùng static readonly thay vì const? 'const' trong C# chỉ áp dụng được cho kiểu dữ liệu nguyên thủy (primitive types) và hằng số chuỗi ở thời điểm biên dịch (compile-time constant). Mảng chuỗi string[] là một đối tượng tham chiếu nằm trên Heap, do đó không thể gán const. Từ khóa 'static' đảm bảo mảng này chỉ được cấp phát duy nhất 1 lần cho cả lớp thay vì nhân bản theo từng đối tượng. Từ khóa 'readonly' ngăn chặn việc vô tình gán lại mảng mới từ bên ngoài.", "Dòng public static readonly string[] Positions: ")
    add_bullet(doc, "Cú pháp Auto-Implemented Property. Khi biên dịch ra mã IL, trình biên dịch C# tự động tạo ra một trường dữ liệu ẩn (Backing Field) có tên dạng private int <Id>k__BackingField; cùng 2 hàm getter (get_Id()) và setter (set_Id()). Điều này giúp bảo toàn nguyên lý Đóng gói (Encapsulation) mà không làm rườm rà mã nguồn.", "Dòng public int Id { get; set; }: ")
    add_bullet(doc, "Trong constructor không tham số, hai thuộc tính Name và Position được chủ động gán chuỗi rỗng \"\" thay vì để giá trị mặc định là null. Việc này giúp triệt tiêu hoàn toàn nguy cơ ứng dụng bị văng lỗi NullReferenceException khi sau này gọi Name.Trim() hoặc Position.Equals().", "Dòng Name = \"\"; Position = \"\";: ")

    add_paragraph(doc, "Tiếp theo là phương thức nhập dữ liệu Input():")
    add_code_block(doc,
"""        // 5. PHƯƠNG THỨC NHẬP LIỆU TỪ BÀN PHÍM CÓ KIỂM TRA TÍNH HỢP LỆ
        public void Input()
        {
            Id = ReadInt("Input Id: ");

            Console.Write("Input Name: ");
            Name = (Console.ReadLine() ?? "").Trim();

            Salary = ReadDouble("Input Salary (per month): ");

            while (true)
            {
                Console.Write("Input Position (Manager/Developer/Tester): ");
                string p = (Console.ReadLine() ?? "").Trim();
                
                // Tìm kiếm không phân biệt hoa thường trong danh mục cho phép
                string? match = Array.Find(Positions, x => x.Equals(p, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    Position = match; // Gán đúng chuẩn viết hoa danh mục
                    break;            // Thoát khỏi vòng lặp khi hợp lệ
                }
                Console.WriteLine("Invalid position! Please enter: Manager, Developer, or Tester.");
            }
        }"""
    )
    
    add_paragraph(doc, "Phân tích các kỹ thuật phòng thủ dữ liệu:", bold_prefix="Chi tiết giải thuật Input: ")
    add_bullet(doc, "Toán tử gộp null (Null-coalescing operator). Nếu người dùng ấn Ctrl+Z hoặc luồng input trả về null, toán tử sẽ tự động thay thế bằng \"\" trước khi gọi phương thức .Trim(). Phương thức .Trim() cắt bỏ toàn bộ ký tự khoảng trắng thừa ở hai đầu chuỗi.", "Dòng (Console.ReadLine() ?? \"\").Trim(): ")
    add_bullet(doc, "Sử dụng biểu thức Lambda 'x => x.Equals(...)' làm tiêu chuẩn tìm kiếm. 'StringComparison.OrdinalIgnoreCase' so sánh mã nhị phân byte của từng ký tự mà bỏ qua chữ hoa chữ thường. Ví dụ: người dùng nhập 'developer', 'DEVELOPER', hay 'Developer' đều được hệ thống nhận diện chính xác và quy chuẩn lại thành chữ 'Developer' chuẩn mực.", "Dòng Array.Find(Positions, ...): ")
    add_bullet(doc, "Tạo một vòng lặp vô hạn ép buộc người dùng phải nhập đúng một trong ba chức vụ hợp lệ thì mới gặp lệnh break để đi tiếp, chặn đứng dữ liệu rác.", "Dòng while(true): ")

    add_paragraph(doc, "Tiếp theo là phương thức hiển thị Display() và ToString():")
    add_code_block(doc,
"""        // 6. PHƯƠNG THỨC HIỂN THỊ THÔNG TIN KẾT HỢP VỚI DELEGATE
        public void Display(SalaryCaculation delObj)
        {
            Console.WriteLine($"Id: {Id} - Name: {Name} - Position: {Position}");
            Console.WriteLine($"Monthly salary: {Salary:N0}");
            
            // Lời gọi gián tiếp thông qua con trỏ delegate
            double yearlySalary = delObj(Salary, Position);
            Console.WriteLine($"Yearly salary : {yearlySalary:N0}");
        }

        public override string ToString()
        {
            return $"Id: {Id,-5} | Name: {Name,-20} | Position: {Position,-12} | Salary: {Salary,10:N0}";
        }"""
    )
    
    add_paragraph(doc, "Phân tích cú pháp chuỗi nội suy và định dạng số:", bold_prefix="Cú pháp hiển thị: ")
    add_bullet(doc, "delObj là tham chiếu delegate được truyền vào. CLR sẽ gọi phương thức mà delegate này đang trỏ tới (ở đây là hàm CalculateYearlySalary). Kết quả trả về được lưu vào biến yearlySalary.", "Dòng delObj(Salary, Position): ")
    add_bullet(doc, "Định dạng số Number format (N). Số 0 biểu thị không lấy phần thập phân. Kết quả số tiền sẽ tự động được thêm dấu phân cách hàng nghìn (ví dụ: 1000000 hiển thị thành '1,000,000'), giúp giao diện console chuyên nghiệp và dễ đọc.", "Cú pháp {Salary:N0}: ")
    add_bullet(doc, "Định dạng căn lề bảng Console: {Id,-5} căn lề trái chiếm 5 ký tự; {Salary,10:N0} căn lề phải chiếm 10 ký tự. Đảm bảo dữ liệu khi in ra danh sách sẽ thẳng hàng như một bảng dữ liệu Excel.", "Cú pháp lề {Name,-20}: ")

    add_paragraph(doc, "Cuối cùng là hai hàm hỗ trợ phòng thủ nhập liệu số nguyên và số thực:")
    add_code_block(doc,
"""        // 7. CÁC HÀM TIỆN ÍCH PHÒNG THỦ CHỐNG SẬP ỨNG DỤNG (CRASH-PROOF INPUT)
        private static int ReadInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (int.TryParse(Console.ReadLine(), out int v))
                {
                    return v;
                }
                Console.WriteLine("Wrong format! Please input an integer.");
            }
        }

        private static double ReadDouble(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                if (double.TryParse(Console.ReadLine(), out double v) && v >= 0)
                {
                    return v;
                }
                Console.WriteLine("Wrong format! Please input a positive number.");
            }
        }
    }
}"""
    )
    add_callout(doc, "Cảnh báo lỗi FormatException khi đi thi",
        "Rất nhiều sinh viên có thói quen viết: 'int id = int.Parse(Console.ReadLine());'. "
        "Nếu người dùng vô tình gõ chữ 'abc' hoặc nhấn phím Enter trống, int.Parse sẽ ném ngoại lệ FormatException ngay lập tức, "
        "khiến ứng dụng bị văng tắt giữa chừng. Giám khảo chấm thi sẽ đánh giá code kém.\n"
        "Bằng cách dùng int.TryParse kết hợp vòng lặp while(true), hàm trả về false một cách êm thấm khi sai định dạng "
        "và in thông báo yêu cầu nhập lại, bảo đảm chương trình hoạt động ổn định 100%.",
        "warning"
    )

    # 2.3 Phân tích SalaryCaculation.cs
    add_header_styled(doc, "2.3. Phân tích SalaryCaculation.cs & Lớp tiện ích SalaryCalculator", level=2)
    add_paragraph(doc,
        "Tệp mã nguồn này chứa định nghĩa Delegate và thuật toán tính lương năm áp dụng các hệ số thưởng theo vị trí việc làm:"
    )
    add_code_block(doc,
"""namespace ASM1
{
    // ĐỊNH NGHĨA CHỮ KÝ ỦY NHIỆM
    public delegate double SalaryCaculation(double salary, string position);

    // LỚP TIỆN ÍCH TÍNH TOÁN LƯƠNG
    public static class SalaryCalculator
    {
        public static double CalculateYearlySalary(double salary, string position)
        {
            // 12 tháng lương cơ bản
            double yearly = salary * 12;

            // Nhân hệ số thưởng dựa theo vị trí công việc
            if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.5; // Thưởng 50% cho cấp quản lý
            }
            else if (position.Equals("Developer", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.3; // Thưởng 30% cho lập trình viên
            }
            else if (position.Equals("Tester", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.2; // Thưởng 20% cho chuyên viên kiểm thử
            }

            return yearly;
        }
    }
}"""
    )
    add_paragraph(doc,
        "Đặc điểm kiến trúc của SalaryCalculator:"
    )
    add_bullet(doc, "Lớp tĩnh (static class) được CLR đảm bảo không thể tạo đối tượng bằng từ khóa new. Tất cả thành phần bên trong bắt buộc phải là static. Lớp này được nạp vào bộ nhớ một lần duy nhất khi ứng dụng chạy.", "Khai báo public static class: ")
    add_bullet(doc, "Phương thức nhận vào (double salary, string position) và trả về double. Chữ ký này trùng khớp 100% với định nghĩa của delegate SalaryCaculation. Do đó, ta có thể truyền trực tiếp tên hàm 'SalaryCalculator.CalculateYearlySalary' vào bất kỳ nơi nào yêu cầu tham số delegate mà không cần ép kiểu (Kỹ thuật Method Group Conversion).", "Tính tương thích chữ ký hàm: ")

    # 2.4 Phân tích EmployeeList.cs
    add_header_styled(doc, "2.4. Phân tích chi tiết EmployeeList.cs (File I/O & LINQ Queries)", level=2)
    add_paragraph(doc,
        "Lớp EmployeeList đóng vai trò là một Kho lưu trữ nội bộ (In-memory Repository) quản lý danh sách nhân viên, "
        "thực hiện việc đọc/ghi tập tin và cung cấp các hàm tìm kiếm lọc dữ liệu bằng LINQ."
    )
    
    add_header_styled(doc, "A. Thuật toán đọc tập tin định dạng Delimited Text (CSV/Pipe):", level=3)
    add_code_block(doc,
"""        // ĐỌC DỮ LIỆU TỪ TỆP THEO ĐỊNH DẠNG id|name|salary|position
        public void inputEmployeeFromFile(string filePath)
        {
            // 1. Kiểm tra sự tồn tại của tệp trên ổ đĩa
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }

            // 2. Đọc toàn bộ các dòng văn bản vào mảng
            string[] lines = File.ReadAllLines(filePath);
            int countAdded = 0;

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line)) continue; // Bỏ qua các dòng trống

                // 3. Phân tách dòng thành các phần tử qua ký tự phân cách |
                string[] parts = line.Split('|');
                if (parts.Length == 4)
                {
                    // 4. Kiểm tra ép kiểu an toàn cho ID và Lương
                    if (int.TryParse(parts[0].Trim(), out int id) &&
                        double.TryParse(parts[2].Trim(), out double salary))
                    {
                        string name = parts[1].Trim();
                        string position = parts[3].Trim();

                        // 5. Khởi tạo đối tượng bằng cú pháp Object Initializer
                        Employee e = new Employee
                        {
                            Id = id,
                            Name = name,
                            Salary = salary,
                            Position = position
                        };

                        AddEmployee(e);
                        countAdded++;
                    }
                }
            }

            Console.WriteLine($"Successfully loaded {countAdded} employee(s) from file: {filePath}");
        }"""
    )
    
    add_paragraph(doc, "Bóc tách từng bước xử lý File I/O:", bold_prefix="Cơ chế đọc file: ")
    add_bullet(doc, "Tránh việc trực tiếp gọi File.Open mà tệp không tồn tại sẽ gây lỗi FileNotFoundException làm văng ứng dụng.", "1. File.Exists(filePath): ")
    add_bullet(doc, "Đọc toàn bộ nội dung tệp văn bản từ đầu đến cuối và trả về một mảng chuỗi string[], trong đó mỗi phần tử tương ứng với một dòng text. Hàm này tự động mở tệp, đọc bằng mã hóa UTF-8 và tự động đóng tệp ngay sau đó.", "2. File.ReadAllLines(filePath): ")
    add_bullet(doc, "Cắt chuỗi tại vị trí ký tự sọc đứng '|'. Ví dụ với dòng '1|Hung|1000|Manager', mảng parts nhận được gồm 4 phần tử: parts[0]=\"1\", parts[1]=\"Hung\", parts[2]=\"1000\", parts[3]=\"Manager\".", "3. line.Split('|'): ")
    add_bullet(doc, "Đảm bảo dòng dữ liệu có đủ 4 trường thông tin. Nếu tệp có dòng lỗi thiếu trường (ví dụ chỉ có 3 phần tử), chương trình sẽ bỏ qua dòng đó một cách an toàn mà không bị lỗi IndexOutOfRangeException.", "4. if (parts.Length == 4): ")

    add_header_styled(doc, "B. Các phương thức tìm kiếm dữ liệu bằng LINQ (Language Integrated Query):", level=3)
    add_paragraph(doc,
        "LINQ là một trong những công nghệ mạnh mẽ nhất của C#. Dưới đây là cách triển khai 5 hàm tìm kiếm theo yêu cầu của đề bài:"
    )
    
    add_code_block(doc,
"""        // 1. TÌM KIẾM THEO ID (KHÓA CHÍNH - DUY NHẤT)
        public void searchEmployeeById(int id)
        {
            var foundEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (foundEmployee != null)
            {
                Console.WriteLine("Found employee:");
                foundEmployee.Display(SalaryCalculator.CalculateYearlySalary);
            }
            else
            {
                Console.WriteLine($"No employee found with ID '{id}'.");
            }
        }

        // 2. TÌM KIẾM THEO TÊN (CÓ THỂ CÓ NHIỀU KẾT QUẢ TRÙNG NHAU)
        public void searchEmployeeByName(string name)
        {
            var foundEmployees = employees
                .Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            DisplaySearchResult(foundEmployees, $"name matching '{name}'");
        }

        // 3. TÌM KIẾM THEO VỊ TRÍ CHỨC VỤ
        public void searchEmployeeByPosition(string position)
        {
            var foundEmployees = employees
                .Where(e => e.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                .ToList();

            DisplaySearchResult(foundEmployees, $"position '{position}'");
        }

        // 4. TÌM KIẾM THEO KHOẢNG LƯƠNG THÁNG CƠ BẢN
        public void searchEmployeeBySalaryInRange(double minSalary, double maxSalary)
        {
            var foundEmployees = employees
                .Where(e => e.Salary >= minSalary && e.Salary <= maxSalary)
                .OrderBy(e => e.Salary)
                .ToList();

            DisplaySearchResult(foundEmployees, $"monthly salary between {minSalary:N0} and {maxSalary:N0}");
        }

        // 5. TÌM KIẾM THEO KHOẢNG LƯƠNG CẢ NĂM (KẾT HỢP LINQ VỚI DELEGATE)
        public void searchEmployeeByYearlySalaryInRange(double minYearlySalary, double maxYearlySalary)
        {
            var foundEmployees = employees.Where(e =>
            {
                // Gọi hàm Delegate để tính lương năm thực tế trước khi lọc
                double yearlySalary = SalaryCalculator.CalculateYearlySalary(e.Salary, e.Position);
                return yearlySalary >= minYearlySalary && yearlySalary <= maxYearlySalary;
            })
            .OrderBy(e => SalaryCalculator.CalculateYearlySalary(e.Salary, e.Position))
            .ToList();

            DisplaySearchResult(foundEmployees, $"yearly salary between {minYearlySalary:N0} and {maxYearlySalary:N0}");
        }"""
    )
    
    add_paragraph(doc, "Bóc tách nguyên lý hoạt động của các toán tử LINQ:", bold_prefix="Cơ chế LINQ chuyên sâu: ")
    add_bullet(doc, "Toán tử duyệt tuần tự qua danh sách List<Employee>. Trả về phần tử đầu tiên thỏa mãn điều kiện e.Id == id. Nếu duyệt hết danh sách mà không thấy phần tử nào khớp, nó trả về giá trị mặc định của kiểu tham chiếu là null. So với toán tử .First() (sẽ ném ra ngoại lệ InvalidOperationException: Sequence contains no matching element nếu không tìm thấy), FirstOrDefault an toàn hơn rất nhiều.", "Toán tử FirstOrDefault(): ")
    add_bullet(doc, "Toán tử lọc dữ liệu theo cơ chế Thực thi lười (Deferred Execution / Lazy Evaluation). Bản thân hàm Where chỉ tạo ra một bộ duyệt IEnumerable<T> chứ chưa hề chạy vòng lặp lọc ngay lúc đó. Chỉ khi ta gọi hàm .ToList() (Immediate Execution), LINQ mới thực sự duyệt qua từng phần tử, tạo một vùng nhớ List<Employee> mới trên Heap và thêm các phần tử thỏa mãn vào.", "Toán tử Where() và .ToList(): ")
    add_bullet(doc, "Đây là điểm sáng tạo kỹ thuật cao nhất của đề bài: Biểu thức Lambda truyền vào Where là một khối lệnh đa dòng '{ ... }'. Trong khối lệnh này, hệ thống gọi hàm SalaryCalculator.CalculateYearlySalary(e.Salary, e.Position) để tính toán lương năm có thưởng cho từng nhân viên, rồi mới so sánh với khoảng [minYearlySalary, maxYearlySalary].", "Tìm kiếm theo Lương Năm: ")

    # 2.5 Phân tích Program.cs
    add_header_styled(doc, "2.5. Phân tích Program.cs (Luồng điều khiển Menu & Đường dẫn File)", level=2)
    add_paragraph(doc,
        "Tệp Program.cs là điểm vào (Entry Point) của ứng dụng Console. Tệp này chịu trách nhiệm khởi tạo menu điều khiển "
        "và xử lý bài toán đường dẫn file tương đối linh hoạt:"
    )
    
    add_code_block(doc,
"""namespace ASM1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            EmployeeList list = new EmployeeList();

            // 1. KỸ THUẬT TÌM VÀ NẠP FILE TỰ ĐỘNG CHỐNG LỖI ĐƯỜNG DẪN
            string defaultPath = "employees.txt";
            string projectFolderFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "employees.txt");
            
            if (File.Exists(defaultPath))
            {
                list.inputEmployeeFromFile(defaultPath);
            }
            else if (File.Exists(projectFolderFile))
            {
                list.inputEmployeeFromFile(projectFolderFile);
            }
            else
            {
                // Tự động khởi tạo file mẫu nếu chưa có trên máy
                File.WriteAllText(defaultPath, "1|Hung|1000|Manager\n2|Hoa|900|Developer\n3|Tuan|800|Tester");
                list.inputEmployeeFromFile(defaultPath);
            }

            // 2. VÒNG LẶP MENU ĐIỀU KHIỂN TƯƠNG TÁC
            while (true)
            {
                Console.WriteLine("\n========== EMPLOYEE MANAGEMENT (ASM1) ==========");
                Console.WriteLine("1. Input new employee from console");
                Console.WriteLine("2. Display all employees with yearly salary");
                Console.WriteLine("3. Load employees from file");
                Console.WriteLine("4. Search employee by ID");
                Console.WriteLine("5. Search employee by Name");
                Console.WriteLine("6. Search employee by Position");
                Console.WriteLine("7. Search employee by Monthly Salary range");
                Console.WriteLine("8. Search employee by Yearly Salary range");
                Console.WriteLine("0. Exit program");
                Console.WriteLine("================================================");
                Console.Write("Enter your choice (0-8): ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        Employee emp = new Employee();
                        emp.Input();
                        list.AddEmployee(emp);
                        Console.WriteLine("Employee added successfully!");
                        break;
                    case "2":
                        list.DisplayAllEmployees(SalaryCalculator.CalculateYearlySalary);
                        break;
                    case "3":
                        Console.Write("Enter file path: ");
                        string p = Console.ReadLine() ?? "";
                        list.inputEmployeeFromFile(p);
                        break;
                    // ... các case từ 4 đến 8 ...
                    case "0":
                        Console.WriteLine("Program ended. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please select 0 to 8.");
                        break;
                }
            }
        }
    }
}"""
    )
    
    add_paragraph(doc, "Những kinh nghiệm kỹ thuật quý giá từ Program.cs:", bold_prefix="Kinh nghiệm thực chiến: ")
    add_bullet(doc, "Mặc định trên Windows, Console chạy ở bảng mã ASCII/ANSI cũ khiến các chữ tiếng Việt có dấu bị lỗi hiển thị '???'. Dòng lệnh 'Console.OutputEncoding = System.Text.Encoding.UTF8;' ép Terminal hiển thị đúng chuẩn UTF-8 toàn cầu.", "1. Console.OutputEncoding = UTF8: ")
    add_bullet(doc, "Khi chạy dự án trong Visual Studio hoặc gõ 'dotnet run', thư mục hiện hành của ứng dụng là thư mục đầu ra 'bin\\Debug\\net8.0\\' chứ không phải thư mục chứa code .cs. Nếu chỉ viết 'File.Exists(\"employees.txt\")' thì chương trình sẽ không tìm thấy file nếu file đó nằm ở thư mục mã nguồn. Sử dụng 'AppDomain.CurrentDomain.BaseDirectory' kết hợp lùi 3 cấp thư mục ('..\\..\\..') giúp ứng dụng tìm thấy file gốc ở bất kỳ môi trường chạy nào.", "2. Giải quyết xung đột đường dẫn thư mục bin/Debug: ")

    print("Chapter 2 built successfully.")
