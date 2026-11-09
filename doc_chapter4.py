# -*- coding: utf-8 -*-
"""
Chương 4: Bảng tra cứu cú pháp, xử lý sự cố Runtime và Checklist thi cử PRN212
"""

from build_docx import (
    add_header_styled, add_paragraph, add_bullet, 
    add_code_block, add_callout, add_table_styled
)

def build_chapter_4(doc):
    add_header_styled(doc, "CHƯƠNG 4: BẢNG TRA CỨU CÚ PHÁP, XỬ LÝ LỖI RUNTIME VÀ CHECKLIST THI CỬ", level=1)
    
    add_paragraph(doc,
        "Chương này đóng vai trò như một cẩm nang tra cứu nhanh (Cheat Sheet) cho các lập trình viên C# .NET và sinh viên "
        "chuẩn bị bảo vệ đồ án môn PRN212. Nội dung bao gồm bảng tra cứu các cú pháp hiện đại của C# 12 / .NET 8, "
        "bảng tổng hợp các lỗi Runtime kinh điển cùng nguyên nhân gốc rễ và giải pháp, "
        "cùng danh mục kiểm tra 10 bước an toàn trước khi nộp hoặc trình bày bài thi."
    )
    
    # 4.1 Bảng tra cứu cú pháp
    add_header_styled(doc, "4.1. Bảng tra cứu các cú pháp C# hiện đại thường dùng trong PRN212", level=2)
    
    table_headers = ["Cú pháp / Từ khóa", "Ví dụ minh họa", "Ý nghĩa và cơ chế hoạt động"]
    table_data = [
        ["Toán tử Gộp Null (??)", "string s = name ?? \"N/A\";", "Nếu vế trái khác null thì lấy vế trái, nếu null thì lấy vế phải."],
        ["Toán tử Điều kiện Null (?.)", "int? len = text?.Length;", "Nếu đối tượng khác null thì truy cập thuộc tính/gọi hàm, nếu null thì trả về null chứ không ném NullReferenceException."],
        ["Khai báo Using (C# 8+)", "using var conn = GetConnection();", "Tự động gọi .Dispose() để giải phóng socket/kết nối unmanaged ngay khi hàm kết thúc, chống rò rỉ RAM."],
        ["Auto-Property với Khởi tạo", "public string Name { get; set; } = \"\";", "Tự động sinh backing field ẩn và gán giá trị mặc định tránh null."],
        ["Object Initializer", "var e = new Employee { Id = 1, Name = \"A\" };", "Khởi tạo đối tượng và gán các thuộc tính cùng lúc mà không cần viết nhiều dòng."],
        ["Biểu thức Lambda", "list.Where(x => x.Salary > 1000);", "Hàm ẩn danh (Anonymous Function) ngắn gọn dùng làm predicate lọc dữ liệu."],
        ["Method Group Conversion", "emp.Display(SalaryCalc.GetYearly);", "Truyền trực tiếp tên hàm vào tham số Delegate mà không cần viết new DelegateName(GetYearly)."],
        ["Async / Await", "await Task.Run(() => DAO.GetAll());", "Đẩy tác vụ I/O nặng ra Worker Thread ngầm, giữ UI Thread 60 FPS không bị đơ."],
        ["CallerMemberName", "void OnChanged([CallerMemberName] string n = \"\")", "Trình biên dịch tự động điền tên thuộc tính đang gọi vào tham số n, loại bỏ lỗi typo chuỗi."],
        ["LINQ FirstOrDefault", "var item = list.FirstOrDefault(x => x.Id == 1);", "Lấy phần tử đầu tiên thỏa mãn. Nếu không thấy trả về null an toàn (không văng lỗi như .First())."]
    ]
    add_table_styled(doc, table_headers, table_data, col_widths=[1.5, 2.3, 3.0])

    # 4.2 Bảng xử lý lỗi Runtime
    add_header_styled(doc, "4.2. Bảng tổng hợp các lỗi Runtime kinh điển và giải pháp khắc phục", level=2)
    
    err_headers = ["Tên lỗi Runtime (Exception)", "Nguyên nhân gốc rễ", "Giải pháp xử lý triệt để"]
    err_data = [
        [
            "NullReferenceException:\nObject reference not set to an instance of an object",
            "Cố gắng truy cập thuộc tính hoặc phương thức của một biến đang mang giá trị null (ví dụ: emp.Name.Trim() khi Name chưa được khởi tạo).",
            "1. Sử dụng toán tử ?. (safe navigation).\n2. Khởi tạo chuỗi rỗng \"\" trong constructor mặc định.\n3. Kiểm tra if (obj != null) trước khi gọi."
        ],
        [
            "SqlException:\nCannot open database requested by the login",
            "Tên database trong Connection String (ví dụ SE1963) chưa được tạo hoặc chưa được restore trên SQL Server của máy tính hiện tại.",
            "1. Mở SQL Server Management Studio (SSMS) kiểm tra database.\n2. Chạy script tạo DB và bảng dữ liệu.\n3. Kiểm tra lại tên Instance (.\\SQLEXPRESS hoặc localhost)."
        ],
        [
            "SqlException:\nInvalid object name 'TableName'",
            "Câu lệnh SQL truy vấn vào một bảng không tồn tại trong Database đang kết nối (thường do Connection String trỏ nhầm database khác).",
            "1. Kiểm tra lại appsettings.json xem Connection String đang trỏ tới đúng CSDL chưa.\n2. Kiểm tra lại chính tả tên bảng (ví dụ: STUDENTS)."
        ],
        [
            "InvalidOperationException:\nThe calling thread cannot access this object because a different thread owns it",
            "Vi phạm quy tắc STA của WPF: Cố gắng cập nhật trực tiếp một control UI (TextBox, DataGrid) từ bên trong một Worker Thread (Task.Run).",
            "1. Sử dụng 'await' để CLR tự động đưa luồng về UI Thread.\n2. Hoặc bọc lệnh cập nhật trong: Application.Current.Dispatcher.Invoke(() => { txt.Text = ...; });"
        ],
        [
            "DbUpdateException / SqlException:\nThe DELETE statement conflicted with the REFERENCE constraint",
            "Cố gắng xóa một dòng dữ liệu cha (ví dụ: Department) khi đang có các dòng con (Employees) tham chiếu tới khóa ngoại đó.",
            "1. Hiển thị thông báo yêu cầu người dùng xóa hoặc chuyển toàn bộ nhân viên thuộc phòng ban đó sang phòng ban khác trước khi xóa phòng ban."
        ],
        [
            "FormatException:\nInput string was not in a correct format",
            "Sử dụng int.Parse() hoặc double.Parse() trên một chuỗi người dùng nhập chứa chữ cái hoặc để trống.",
            "Luôn luôn dùng int.TryParse(input, out int result) hoặc double.TryParse() kết hợp vòng lặp while(true) để bắt người dùng nhập lại."
        ],
        [
            "FileNotFoundException:\nCould not find file '...'",
            "Đọc file theo đường dẫn tương đối cứng 'data.txt' nhưng khi ứng dụng chạy từ thư mục bin\\Debug, tệp nằm ở thư mục nguồn dự án bên ngoài.",
            "1. Thiết lập 'Copy to Output Directory = Copy if newer' trong tệp .csproj.\n2. Hoặc dùng Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ...)."
        ]
    ]
    add_table_styled(doc, err_headers, err_data, col_widths=[1.8, 2.5, 2.5])

    # 4.3 Checklist 10 bước chuẩn bị
    add_header_styled(doc, "4.3. Checklist 10 bước vàng chuẩn bị trước khi bảo vệ / thi cử môn PRN212", level=2)
    add_paragraph(doc,
        "Trước khi nộp bài hoặc bấm nút Start trình bày đồ án trước mặt giám thị, hãy kiểm tra lần lượt 10 tiêu chí sau:"
    )
    
    checklist = [
        ("Bước 1: Kiểm tra build 0 Error 0 Warning", "Chạy 'dotnet build' từ terminal hoặc Rebuild Solution trong Visual Studio. Đảm bảo toàn bộ các project đều biên dịch thành công 100%."),
        ("Bước 2: Kiểm tra kết nối SQL Server cục bộ", "Đảm bảo SQL Server Service (.\\SQLEXPRESS) đang ở trạng thái 'Running'. Kiểm tra cả 3 database: SE1963, EmployeeDB, và CourseAttendanceDB đã có đủ dữ liệu mẫu."),
        ("Bước 3: Kiểm tra tệp cấu hình appsettings.json", "Đảm bảo file appsettings.json đã được đặt thuộc tính 'Copy to Output Directory' = 'PreserveNewest'. Kiểm tra chuỗi kết nối sử dụng 'Trusted_Connection=True;TrustServerCertificate=True;'."),
        ("Bước 4: Kiểm tra timeout kết nối CSDL", "Trong Connection String luôn đặt tham số 'Connect Timeout=3'. Điều này cứu bạn khỏi bị trừ điểm đơ máy nếu giám thị rút dây mạng hoặc máy chấm có cấu hình khác."),
        ("Bước 5: Kiểm tra phòng thủ nhập liệu số", "Toàn bộ các ô nhập số tuổi, lương, điểm số phải được kiểm tra bằng TryParse hoặc xử lý ngoại lệ chu đáo, không bao giờ để người dùng nhập chữ làm crash ứng dụng."),
        ("Bước 6: Kiểm tra trải nghiệm chuyển Tab", "Thử bấm liên tục giữa 4 Tab trên giao diện AppManager. Đảm bảo chuyển tab mượt mà tức thì 0ms, không có hiện tượng chớp nháy hay giật khung hình."),
        ("Bước 7: Kiểm tra cảnh báo dữ liệu chưa lưu", "Trên màn hình Điểm danh (Tab 4), tích chọn một sinh viên rồi thử chọn môn học khác. Đảm bảo hộp thoại xác nhận lưu dữ liệu xuất hiện đúng như thiết kế."),
        ("Bước 8: Kiểm tra giải phóng kết nối bằng using var", "Rà soát toàn bộ các hàm gọi CSDL trong DAO.cs và StudentDAO.cs, đảm bảo mọi đối tượng SqlConnection, SqlCommand, SqlDataAdapter đều có từ khóa 'using var'."),
        ("Bước 9: Kiểm tra tiếng Việt có dấu", "Đảm bảo tệp console Program.cs có dòng 'Console.OutputEncoding = Encoding.UTF8;' và các tệp mã nguồn lưu ở bảng mã UTF-8 with BOM hoặc UTF-8."),
        ("Bước 10: Nắm vững câu hỏi lý thuyết cốt lõi", "Sẵn sàng trả lời 3 câu hỏi vấn đáp tủ: 1. Tại sao dùng Delegate cho bài toán tính lương? 2. Sự khác biệt giữa ADO.NET và EF Core 8? 3. Cơ chế INotifyPropertyChanged hoạt động như thế nào trong WPF?")
    ]
    
    for title, desc in checklist:
        add_bullet(doc, desc, bold_prefix=f"{title}: ")
        
    add_callout(doc, "Lời khuyên khi thuyết trình",
        "Khi bảo vệ đồ án, hãy chủ động giới thiệu về kiến trúc hệ thống (Kiến trúc 3 lớp, View Caching chống giật lag, "
        "bất đồng bộ async/await, và cơ chế bảo vệ dữ liệu chưa lưu). "
        "Giám khảo sẽ đánh giá bạn là một kỹ sư phần mềm thực thụ có tư duy kiến trúc thay vì chỉ là một người gõ code theo mẫu!",
        "tip"
    )

    print("Chapter 4 built successfully.")
