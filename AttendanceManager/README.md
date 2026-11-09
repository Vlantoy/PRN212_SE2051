# Hệ Thống Quản Lý Điểm Danh Sinh Viên - PRN212 (Kiến Trúc 3-Layer / DAO Pattern)

Dự án được xây dựng theo đúng sơ đồ phác thảo trên bảng và áp dụng mô hình phân tầng **3-Layer / DAO Pattern** chuẩn mực:

```
AttendanceManager/
│
├── Models/                     # Tầng Entities (Thực thể dữ liệu)
│   ├── Course.cs
│   ├── Schedule.cs
│   ├── Student.cs
│   └── Attendance.cs
│
├── ViewModels/                 # Tầng ViewModel phục vụ DataGrid
│   └── AttendanceItemViewModel.cs
│
├── Data/                       # Tầng kết nối DbContext
│   └── AppDbContext.cs
│
├── DAL/                        # Data Access Layer (Truy vấn CSDL)
│   ├── CourseDAO.cs            # Lấy danh sách Courses
│   ├── ScheduleDAO.cs          # Lấy danh sách Schedules theo CourseId
│   └── AttendanceDAO.cs        # Lấy & cập nhật điểm danh theo ScheduleId
│
├── BLL/                        # Business Logic Layer (Xử lý nghiệp vụ & Validation)
│   └── AttendanceService.cs    # Thực hiện toàn bộ logic validation và điều phối DAL
│
└── GUI / Presentation          # Giao diện người dùng
    ├── MainWindow.xaml         # Giao diện chuẩn 1:1 theo ảnh bảng vẽ
    └── MainWindow.xaml.cs      # Chỉ bắt sự kiện UI, gọi Service và hiển thị thông báo
```

---

## 1. Giao Diện (Chuẩn 1:1 Theo Ảnh Vẽ)

1. **Course**: Label `Course` + ComboBox chọn một khóa học.
2. **Schedule**: Label `Schedule` + ComboBox chọn một buổi học.
3. **DataGrid**: Bảng 4 cột:
   - `Mã SV`
   - `Tên sinh viên`
   - `Điểm danh` (CheckBox Có mặt / Vắng căn lề thẳng trục)
   - `Ghi chú`
4. **Save Button**: Nút `[ Save ]` đặt ở góc dưới bên phải bảng.

---

## 2. Toàn Bộ Các Tầng Validation

1. **Validation Chọn Course & Schedule**: Nếu chưa chọn mà bấm Save $\rightarrow$ Hiện cảnh báo `MessageBox`.
2. **Cảnh báo mất dữ liệu khi đổi Course / Schedule**: Đang chỉnh sửa điểm danh mà chuyển Course hoặc Schedule khác $\rightarrow$ Hiện hộp thoại `Yes / No / Cancel` (Lưu trước khi chuyển / Hủy chuyển / Bỏ qua thay đổi).
3. **Validate độ dài Ghi chú**: Tối đa 200 ký tự. Nếu vượt quá $\rightarrow$ Báo lỗi cụ thể sinh viên vi phạm và tự động trỏ đến dòng đó trên DataGrid.
4. **Kiểm tra trạng thái thay đổi trước khi Save**: Nếu dữ liệu chưa bị thay đổi mà bấm Save $\rightarrow$ Báo *"Không có thay đổi nào mới về điểm danh để lưu"*.
5. **Xác nhận trước khi lưu (Confirmation Dialog)**: Hỏi `Yes/No` trước khi ghi xuống CSDL.
6. **Cảnh báo khi tắt cửa sổ (Window Closing)**: Nhắc người dùng lưu nếu còn dữ liệu chưa lưu trước khi đóng form.
7. **Xử lý ngoại lệ CSDL (try-catch)**: Bắt các lỗi kết nối, trùng khóa, lỗi đồng thời để ứng dụng chạy mượt mà.
