# -*- coding: utf-8 -*-
"""
Chương 1: Nền tảng kiến trúc .NET 8, C#, CLR và WPF
"""

from build_docx import (
    add_header_styled, add_paragraph, add_bullet, 
    add_code_block, add_callout, add_table_styled
)

def build_chapter_1(doc):
    add_header_styled(doc, "CHƯƠNG 1: NỀN TẢNG KIẾN TRÚC .NET 8, CLR VÀ WPF", level=1)
    
    add_paragraph(doc, 
        "Để làm chủ các dự án lập trình C# từ Console đến ứng dụng Desktop WPF trong môn học PRN212, "
        "việc học thuộc cú pháp là chưa đủ. Lập trình viên cần thấu hiểu cơ chế vận hành bên dưới của "
        "Common Language Runtime (CLR), mô hình cấp phát bộ nhớ (Stack vs Managed Heap), cơ chế dọn dẹp "
        "rác tự động (Garbage Collection), và đặc biệt là kiến trúc luồng (Threading Model) của WPF. "
        "Chương này cung cấp nền tảng lý thuyết chuyên sâu làm cơ sở cho toàn bộ các phân tích mã nguồn tiếp theo."
    )
    
    # 1.1 Chu trình biên dịch
    add_header_styled(doc, "1.1. Chu trình biên dịch và thực thi mã lệnh trong .NET 8", level=2)
    add_paragraph(doc,
        "Khác với các ngôn ngữ biên dịch trực tiếp sang mã máy nhị phân như C/C++, hay các ngôn ngữ "
        "thông dịch hoàn toàn như Python hay JavaScript, C# sử dụng mô hình biên dịch hai tầng (Two-stage Compilation). "
        "Quá trình này diễn ra qua 4 giai đoạn cụ thể:"
    )
    add_bullet(doc, "Mã nguồn C# (.cs) được viết bởi lập trình viên tuân thủ các đặc tả C# 12 trên nền .NET 8.", "Giai đoạn 1 (Source Code): ")
    add_bullet(doc, "Trình biên dịch Roslyn (dotnet build hoặc csc.exe) kiểm tra cú pháp, tối ưu hóa và biên dịch mã nguồn thành mã trung gian IL (Intermediate Language hay Common Intermediate Language - CIL). Kết quả đóng gói trong tệp thực thi di động .dll hoặc .exe.", "Giai đoạn 2 (Roslyn Compiler): ")
    add_bullet(doc, "Khi ứng dụng được khởi chạy, CLR (Common Language Runtime) được nạp vào không gian địa chỉ tiến trình. CLR chịu trách nhiệm quản lý toàn bộ vòng đời ứng dụng: nạp assembly, kiểm tra an toàn kiểu (type safety), cấp phát bộ nhớ và kích hoạt bộ thu gom rác.", "Giai đoạn 3 (CLR Host & Loading): ")
    add_bullet(doc, "Trình biên dịch JIT (Just-In-Time) của CLR chuyển đổi từng khối mã IL thành mã máy nhị phân (Native Machine Code) tương thích chính xác với kiến trúc CPU hiện tại (x64, ARM64). Sau đó, CPU thực thi trực tiếp các lệnh này ở tốc độ phần cứng tối đa.", "Giai đoạn 4 (JIT Compilation & Execution): ")
    
    add_callout(doc, "Ý nghĩa thực tiễn", 
        "Nhờ mô hình IL và CLR, ứng dụng .NET đạt được tính độc lập nền tảng (Cross-platform). Cùng một mã IL có thể chạy trên Windows, Linux hoặc macOS thông qua .NET Runtime tương ứng mà không cần viết lại mã nguồn.", 
        "info"
    )

    # 1.2 Quản lý bộ nhớ: Stack vs Heap & IDisposable
    add_header_styled(doc, "1.2. Cơ chế quản lý bộ nhớ: Stack, Managed Heap và Mẫu IDisposable", level=2)
    add_paragraph(doc,
        "Hiểu rõ Stack và Heap là chìa khóa để phân tích cách truyền tham số, vòng đời đối tượng và hiện tượng rò rỉ bộ nhớ (Memory Leak) trong C#."
    )
    
    add_header_styled(doc, "A. Vùng nhớ Stack (Ngăn xếp cuộc gọi)", level=3)
    add_paragraph(doc,
        "Stack là vùng nhớ hoạt động theo nguyên lý LIFO (Last In, First Out). Stack được cấp phát cố định cho mỗi luồng (Thread) thực thi. "
        "Nơi đây lưu trữ:"
    )
    add_bullet(doc, "Bao gồm int, double, float, bool, char, struct, enum. Giá trị thực của biến được ghi trực tiếp vào ô nhớ Stack.", "Kiểu giá trị (Value Types): ")
    add_bullet(doc, "Biến cục bộ nằm trong thân hàm và các tham số được truyền vào hàm.", "Phạm vi cục bộ (Local Scope): ")
    add_bullet(doc, "Địa chỉ con trỏ (32-bit hoặc 64-bit) trỏ đến vùng nhớ của đối tượng thực sự trên Managed Heap.", "Địa chỉ tham chiếu (Reference Pointers): ")
    add_paragraph(doc,
        "Đặc tính kỹ thuật của Stack: Tốc độ truy cập cực nhanh vì chỉ việc di chuyển con trỏ ngăn xếp (Stack Pointer). "
        "Khi hàm kết thúc (gặp dấu đóng ngoặc nhọn '}'), toàn bộ Stack Frame của hàm đó tự động bị giải phóng tức thì mà không cần Garbage Collector can thiệp."
    )

    add_header_styled(doc, "B. Vùng nhớ Managed Heap (Đống do CLR quản lý)", level=3)
    add_paragraph(doc,
        "Managed Heap là vùng nhớ chung dùng để chứa tất cả các đối tượng thuộc Kiểu tham chiếu (Reference Types), bao gồm: "
        "các thể hiện của class (như Employee, Student), mảng (arrays), chuỗi (string), delegate và đối tượng DbContext. "
        "Dữ liệu trên Heap tồn tại độc lập với thời gian chạy của hàm và được dọn dẹp bởi Garbage Collector (GC)."
    )
    add_paragraph(doc,
        "Garbage Collector trong .NET hoạt động theo mô hình Thế hệ (Generational GC) gồm 3 thế hệ:"
    )
    add_bullet(doc, "Chứa các đối tượng vừa mới được khởi tạo (bằng từ khóa new). GC quét thế hệ này thường xuyên nhất vì phần lớn đối tượng trong ứng dụng có tuổi thọ rất ngắn (short-lived).", "Thế hệ 0 (Generation 0): ")
    add_bullet(doc, "Chứa các đối tượng sống sót sau một chu kỳ thu gom của Thế hệ 0. Đóng vai trò vùng đệm giữa đối tượng ngắn hạn và dài hạn.", "Thế hệ 1 (Generation 1): ")
    add_bullet(doc, "Chứa các đối tượng tồn tại lâu dài (long-lived) như biến static, bộ nhớ đệm cache, hoặc kết nối CSDL toàn cục. GC rất hiếm khi thu gom thế hệ này để tránh làm chậm hệ thống.", "Thế hệ 2 (Generation 2): ")
    add_bullet(doc, "Vùng nhớ dành riêng cho các đối tượng có kích thước lớn hơn 85,000 bytes (ví dụ: mảng lớn, mảng byte file ảnh). LOH không được gom nén (compaction) mặc định để tránh chi phí sao chép bộ nhớ khổng lồ.", "Large Object Heap (LOH): ")

    add_header_styled(doc, "C. Quản lý tài nguyên Unmanaged và từ khóa using var (C# 8+)", level=3)
    add_paragraph(doc,
        "Garbage Collector chỉ có thể tự động giải phóng bộ nhớ Managed (RAM do CLR phân bổ). "
        "GC hoàn toàn không biết cách giải phóng các Tài nguyên Unmanaged (tài nguyên do hệ điều hành quản lý trực tiếp) như: "
        "kết nối socket mạng tới SQL Server (SqlConnection), tay cầm tệp đĩa (FileStream), bút vẽ đồ họa (GDI+ Handles)."
    )
    add_paragraph(doc,
        "Nếu lập trình viên mở một SqlConnection mà không đóng lại, kết nối đó vẫn tồn tại trong SQL Server Connection Pool, "
        "dẫn đến lỗi cạn kiệt kết nối ('Timeout expired. The timeout period elapsed prior to obtaining a connection from the pool')."
    )
    add_paragraph(doc,
        "Để giải quyết triệt để vấn đề này, .NET định nghĩa interface IDisposable với phương thức Dispose(). "
        "Kể từ C# 8, cú pháp 'using var' ra đời giúp code ngắn gọn, thanh lịch và an toàn tuyệt đối:"
    )
    add_code_block(doc, 
"""// Cách viết chuẩn mực trong PRN212 (C# 8+)
public static DataTable GetData(string sql)
{
    using var conn = GetConnection();        // Tự động Dispose khi hàm kết thúc
    using var cmd = new SqlCommand(sql, conn); // Tự động Dispose
    using var adapter = new SqlDataAdapter(cmd);
    var dt = new DataTable();
    adapter.Fill(dt);
    return dt;
} // <--- Tại đây conn.Close() và conn.Dispose() ĐƯỢC ĐẢM BẢO GỌI 100%, kể cả khi có Exception xảy ra!"""
    )
    add_callout(doc, "Bản chất ngầm của using var",
        "Khi biên dịch ra mã IL, trình biên dịch C# tự động bọc khối lệnh này vào cấu trúc 'try { ... } finally { conn?.Dispose(); }'. "
        "Dù trong quá trình thực thi câu lệnh SQL có bị văng lỗi (SqlException) hay lỗi mạng, khối finally vẫn luôn luôn được thực thi, "
        "bảo vệ ứng dụng khỏi lỗi Connection Leakage.",
        "tip"
    )

    # 1.3 Mô hình luồng WPF & UI Thread
    add_header_styled(doc, "1.3. Mô hình đa luồng (Threading Architecture) & Quy tắc bất biến của UI Thread trong WPF", level=2)
    add_paragraph(doc,
        "Một trong những nguyên nhân phổ biến nhất khiến sinh viên bị trừ điểm nặng trong các bài thi đồ án WPF "
        "là ứng dụng bị 'đơ' (Not Responding) mỗi khi bấm nút tải dữ liệu, hoặc ứng dụng bị văng lỗi 'InvalidOperationException'. "
        "Nguyên nhân bắt nguồn từ việc vi phạm kiến trúc luồng của WPF."
    )
    add_paragraph(doc,
        "WPF được thiết kế trên mô hình Single-Threaded Apartment (STA) kết hợp với hệ thống kết xuất đồ họa DirectX phân tán. "
        "Trong một ứng dụng WPF luôn có 2 luồng cốt lõi hoạt động song song:"
    )
    add_bullet(doc, "Chạy ẩn dưới tầng phần cứng, chịu trách nhiệm gửi các lệnh vẽ tam giác, đổ bóng và kết xuất pixel tới GPU thông qua DirectX. Luồng này chạy độc lập với tốc độ 60 FPS.", "1. Render Thread: ")
    add_bullet(doc, "Là luồng chính khởi tạo cửa sổ (Window). Luồng này sở hữu Dispatcher (hàng đợi điều phối thông điệp). Luồng này chịu trách nhiệm: đón nhận sự kiện người dùng (chuột, phím, chạm), tính toán Layout (đo đạc kích thước và bố trí vị trí các control trên cây Visual Tree), và thực thi code-behind.", "2. UI Thread (Dispatcher Thread): ")

    add_callout(doc, "Quy tắc vàng bất biến của WPF",
        "1. KHÔNG BAO GIỜ thực thi tác vụ nặng hoặc I/O (truy vấn database, đọc ghi tệp lớn, gọi web service) trên UI Thread.\n"
        "2. KHÔNG BAO GIỜ truy cập hoặc thay đổi thuộc tính của control giao diện (TextBox, DataGrid, Button) từ một luồng Worker phụ.",
        "danger"
    )

    add_paragraph(doc,
        "Khi bạn thực thi một câu lệnh truy vấn SQL kéo dài 3 giây trực tiếp trên UI Thread, Dispatcher sẽ bị nghẽn (blocked). "
        "Nó không thể xử lý thông điệp Windows (WM_PAINT, WM_LBUTTONDOWN). Kết quả là hệ điều hành Windows sẽ gán nhãn ứng dụng là '(Not Responding)' và màn hình bị trắng xóa."
    )
    
    add_header_styled(doc, "Cơ chế giải cứu: async / await kết hợp Task.Run", level=3)
    add_paragraph(doc,
        "Để giao diện luôn đạt tốc độ 60 khung hình/giây mượt mà, ta sử dụng mô hình lập trình bất đồng bộ Task-based Asynchronous Pattern (TAP):"
    )
    add_code_block(doc,
"""// Trong Views/StudentManagementView.xaml.cs
public async Task LoadDataAsync()
{
    try
    {
        // 1. ĐẨY TÁC VỤ NẶNG RA LUỒNG PHỤ (THREADPOOL WORKER THREAD)
        var list = await Task.Run(() => StudentDAO.getAllStudent());
        
        // 2. TỰ ĐỘNG QUAY TRỞ LẠI UI THREAD ĐỂ CẬP NHẬT GIAO DIỆN
        grid1.ItemsSource = list;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi CSDL: {ex.Message}");
    }
}"""
    )
    add_paragraph(doc,
        "Giải thích cơ chế vận hành bên dưới CLR:"
    )
    add_bullet(doc, "CLR lấy một luồng rảnh từ ThreadPool để thực thi hàm StudentDAO.getAllStudent(). Luồng này mở kết nối TCP tới SQL Server và chờ dữ liệu trả về.", "Bước 1 - Task.Run: ")
    add_bullet(doc, "Khi gặp từ khóa await, trình biên dịch C# tự động chia hàm LoadDataAsync thành một Máy trạng thái (State Machine). Luồng chính UI Thread ngay lập tức được giải phóng để tiếp tục phục vụ người dùng (hover chuột, đổi tab, vẽ hiệu ứng).", "Bước 2 - await: ")
    add_bullet(doc, "Khi tác vụ truy vấn SQL ở luồng phụ hoàn tất, CLR bắt lấy SynchronizationContext của WPF và tự động đưa phần code còn lại (grid1.ItemsSource = list) trở về xếp hàng trên Dispatcher của UI Thread để gán dữ liệu an toàn.", "Bước 3 - Resume trên UI Thread: ")

    # 1.4 So sánh ADO.NET vs EF Core 8
    add_header_styled(doc, "1.4. So sánh hai mô hình truy xuất dữ liệu: ADO.NET thuần vs Entity Framework Core 8", level=2)
    add_paragraph(doc,
        "Trong chương trình PRN212, sinh viên bắt buộc phải nắm vững cả hai trường phái truy cập CSDL: "
        "ADO.NET thuần (được ứng dụng trong Tab 2 Quản lý sinh viên CSDL SE1963) và "
        "Entity Framework Core 8 (được ứng dụng trong Tab 3 Quản lý nhân viên CSDL EmployeeDB). "
        "Dưới đây là bảng phân tích và đối sánh toàn diện:"
    )
    
    table_headers = ["Tiêu chí đối sánh", "ADO.NET thuần (System.Data.SqlClient)", "Entity Framework Core 8 (ORM)"]
    table_data = [
        ["Bản chất kỹ thuật", "Tương tác hướng kết nối mức thấp thông qua các lệnh SQL thô.", "Khung ánh xạ quan hệ đối tượng mức cao (Object-Relational Mapping)."],
        ["Tốc độ & Hiệu năng", "Tốc độ nhanh nhất, tiêu tốn ít CPU và RAM nhất vì không qua tầng trung gian.", "Chậm hơn một chút ở lần gọi đầu do cần dịch LINQ sang SQL và khởi tạo DbContext."],
        ["Cú pháp truy vấn", "Viết câu lệnh SQL trực tiếp dưới dạng chuỗi (SELECT * FROM ...). Dễ gõ sai chính tả cột.", "Viết hoàn toàn bằng ngôn ngữ C# thông qua LINQ (Strongly Typed). Bắt lỗi ngay lúc compile."],
        ["Xử lý quan hệ bảng (Join)", "Lập trình viên phải tự viết các mệnh đề INNER JOIN / LEFT JOIN phức tạp bằng tay.", "Hỗ trợ thuộc tính điều hướng (Navigation Property) kết hợp hàm .Include() tải dữ liệu tự động."],
        ["Phòng chống SQL Injection", "Bắt buộc phải nhớ dùng SqlParameter bằng tay. Nếu dùng phép cộng chuỗi sẽ bị dính lỗ hổng bảo mật.", "Tự động tham số hóa 100% tất cả các câu truy vấn LINQ, loại bỏ nguy cơ SQL Injection mặc định."],
        ["Bảo trì & Thay đổi CSDL", "Khó bảo trì. Nếu đổi tên cột trong SQL, phải tìm và sửa lại toàn bộ chuỗi SQL trong code.", "Rất dễ bảo trì. Hỗ trợ Migration đồng bộ cấu trúc DB và kiểm tra lỗi ngay trong Visual Studio."],
        ["Theo dõi trạng thái (Tracking)", "Không có. Muốn cập nhật phải tự viết câu lệnh UPDATE chỉ định rõ cột cần đổi.", "Có Change Tracker tự động theo dõi thực thể bị sửa đổi và tự sinh lệnh UPDATE tối ưu khi SaveChanges."],
        ["Ngữ cảnh phù hợp", "Hệ thống cần tối ưu vi mô tốc độ, báo cáo tài chính lớn, hoặc CSDL cũ có sẵn phức tạp.", "Các ứng dụng doanh nghiệp hiện đại, nghiệp vụ CRUD chuẩn hóa, phát triển nhanh (Rapid Development)."]
    ]
    add_table_styled(doc, table_headers, table_data, col_widths=[1.5, 2.5, 2.8])

    print("Chapter 1 built successfully.")
