using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EmployeeManager.Data;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new AppDbContext();
                // Tự động kiểm tra và tạo database + seed data nếu chưa có
                context.Database.EnsureCreated();

                LoadDepartments();
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", 
                                "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                using var context = new AppDbContext();
                var departments = context.Departments
                    .AsNoTracking()
                    .OrderBy(d => d.DepartmentId)
                    .ToList();

                cbDept.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách phòng ban: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                using var context = new AppDbContext();
                var employees = context.Employees
                    .Include(e => e.Department)
                    .AsNoTracking()
                    .OrderBy(e => e.EmployeeId)
                    .ToList();

                dgEmployees.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của Tên (First Name, Last Name, Middle Name)
        /// - Phải là chữ cái (hỗ trợ tiếng Việt có dấu và tiếng Anh)
        /// - Không được chứa số, không được chứa ký tự đặc biệt
        /// </summary>
        private bool ValidateName(string name, string fieldName, bool isOptional, out string errorMessage)
        {
            errorMessage = string.Empty;
            string trimmed = name.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (isOptional) return true; // Tên đệm có thể để trống
                errorMessage = $"{fieldName} không được để trống!";
                return false;
            }

            // Độ dài tối thiểu
            if (trimmed.Length < 2)
            {
                errorMessage = $"{fieldName} phải có độ dài tối thiểu từ 2 ký tự trở lên!";
                return false;
            }

            // Không được chứa chữ số
            if (trimmed.Any(char.IsDigit))
            {
                errorMessage = $"{fieldName} không được chứa số (chỉ được là chữ cái)!";
                return false;
            }

            // Chỉ cho phép chữ cái Unicode (tiếng Việt có dấu, tiếng Anh), dấu cách, dấu chấm (A.), dấu gạch ngang
            if (!Regex.IsMatch(trimmed, @"^[\p{L}\s\.\-]+$"))
            {
                errorMessage = $"{fieldName} chỉ được chứa chữ cái, không được chứa ký tự đặc biệt (@, #, $, %,...)!";
                return false;
            }

            return true;
        }

        // 1. Chức năng ADD (Thêm nhân viên)
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                // Validate First Name
                if (!ValidateName(firstName, "First Name", isOptional: false, out string errFirst))
                {
                    MessageBox.Show(errFirst, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtFirstName.Focus();
                    return;
                }

                // Validate Last Name
                if (!ValidateName(lastName, "Last Name", isOptional: false, out string errLast))
                {
                    MessageBox.Show(errLast, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLastName.Focus();
                    return;
                }

                // Validate Middle Name (nếu có nhập)
                if (!ValidateName(middleName, "Middle Name", isOptional: true, out string errMid))
                {
                    MessageBox.Show(errMid, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMiddleName.Focus();
                    return;
                }

                using var context = new AppDbContext();

                // Xác định Department: hỗ trợ chọn từ ComboBox hoặc gõ Department Name
                int deptId;
                if (cbDept.SelectedValue is int selectedId)
                {
                    deptId = selectedId;
                }
                else if (!string.IsNullOrWhiteSpace(cbDept.Text))
                {
                    string inputDept = cbDept.Text.Trim();
                    var foundDept = context.Departments.FirstOrDefault(d => d.DepartmentName.ToLower() == inputDept.ToLower());
                    if (foundDept == null)
                    {
                        MessageBox.Show($"Không tìm thấy phòng ban có tên '{inputDept}'! Vui lòng chọn từ danh sách.", "Lỗi Phòng Ban", MessageBoxButton.OK, MessageBoxImage.Warning);
                        cbDept.Focus();
                        return;
                    }
                    deptId = foundDept.DepartmentId;
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập Phòng ban (Dept)!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbDept.Focus();
                    return;
                }

                // Xác định EmployeeId: Nếu người dùng nhập Id thì kiểm tra, nếu bỏ trống thì tự động tăng
                int newId;
                if (!string.IsNullOrEmpty(idText))
                {
                    if (!int.TryParse(idText, out newId) || newId <= 0)
                    {
                        MessageBox.Show("Id phải là số nguyên dương hợp lệ (lớn hơn 0)!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtId.Focus();
                        return;
                    }

                    if (context.Employees.Any(emp => emp.EmployeeId == newId))
                    {
                        MessageBox.Show($"Nhân viên với Id '{newId}' đã tồn tại trong CSDL!", "Trùng khóa chính", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtId.Focus();
                        return;
                    }
                }
                else
                {
                    int maxId = context.Employees.Any() ? context.Employees.Max(emp => emp.EmployeeId) : 100;
                    newId = maxId + 1;
                }

                // Tự động suy luận Gender từ tên theo yêu cầu bài toán
                string deducedGender = Employee.InferGender(firstName, middleName, lastName);

                var newEmployee = new Employee
                {
                    EmployeeId = newId,
                    FirstName = firstName,
                    MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName,
                    LastName = lastName,
                    DepartmentId = deptId,
                    Gender = deducedGender
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();

                MessageBox.Show($"Thêm nhân viên thành công!\n- Họ và tên: {newEmployee.FirstName} {newEmployee.LastName}\n- ID: {newId}\n- Gender (suy ra): {deducedGender}", 
                                "Thông báo thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadEmployees();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 2. Chức năng UPDATE (Cập nhật nhân viên)
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                if (string.IsNullOrEmpty(idText) || !int.TryParse(idText, out int empId) || empId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên từ bảng hoặc nhập Id hợp lệ cần cập nhật!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                // Validate First Name
                if (!ValidateName(firstName, "First Name", isOptional: false, out string errFirst))
                {
                    MessageBox.Show(errFirst, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtFirstName.Focus();
                    return;
                }

                // Validate Last Name
                if (!ValidateName(lastName, "Last Name", isOptional: false, out string errLast))
                {
                    MessageBox.Show(errLast, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLastName.Focus();
                    return;
                }

                // Validate Middle Name (nếu có nhập)
                if (!ValidateName(middleName, "Middle Name", isOptional: true, out string errMid))
                {
                    MessageBox.Show(errMid, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMiddleName.Focus();
                    return;
                }

                using var context = new AppDbContext();
                var emp = context.Employees.FirstOrDefault(e => e.EmployeeId == empId);
                if (emp == null)
                {
                    MessageBox.Show($"Không tìm thấy nhân viên với Id '{empId}' để cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác định Department ID
                int deptId;
                if (cbDept.SelectedValue is int selectedId)
                {
                    deptId = selectedId;
                }
                else if (!string.IsNullOrWhiteSpace(cbDept.Text))
                {
                    string inputDept = cbDept.Text.Trim();
                    var foundDept = context.Departments.FirstOrDefault(d => d.DepartmentName.ToLower() == inputDept.ToLower());
                    if (foundDept == null)
                    {
                        MessageBox.Show($"Không tìm thấy phòng ban có tên '{inputDept}'! Vui lòng chọn lại.", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    deptId = foundDept.DepartmentId;
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập Phòng ban (Dept)!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                emp.FirstName = firstName;
                emp.MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
                emp.LastName = lastName;
                emp.DepartmentId = deptId;
                // Cập nhật lại gender suy ra từ tên mới
                emp.Gender = Employee.InferGender(firstName, middleName, lastName);

                context.SaveChanges();

                MessageBox.Show($"Cập nhật thông tin nhân viên ID '{empId}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadEmployees();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 3. Chức năng DELETE (Xóa nhân viên)
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                if (string.IsNullOrEmpty(idText) || !int.TryParse(idText, out int empId))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên từ bảng hoặc nhập Id hợp lệ cần xóa!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên có Id '{empId}' không?", 
                                                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmResult != MessageBoxResult.Yes)
                {
                    return;
                }

                using var context = new AppDbContext();
                var emp = context.Employees.FirstOrDefault(e => e.EmployeeId == empId);
                if (emp == null)
                {
                    MessageBox.Show($"Không tìm thấy nhân viên với Id '{empId}' để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.Employees.Remove(emp);
                context.SaveChanges();

                MessageBox.Show($"Xóa nhân viên (ID: {empId}) thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadEmployees();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 4. Chức năng SEARCH: Tìm kiếm theo Department Name, Id, First, Mid, Last
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                // Lấy Department Name để search theo department name
                string deptNameSearch = string.Empty;
                if (cbDept.SelectedItem is Department dept)
                {
                    deptNameSearch = dept.DepartmentName;
                }
                else if (!string.IsNullOrWhiteSpace(cbDept.Text))
                {
                    deptNameSearch = cbDept.Text.Trim();
                }

                using var context = new AppDbContext();
                IQueryable<Employee> query = context.Employees
                    .Include(emp => emp.Department)
                    .AsNoTracking();

                bool hasFilter = false;

                // Lọc theo Id nếu có
                if (!string.IsNullOrEmpty(idText) && int.TryParse(idText, out int searchId))
                {
                    query = query.Where(emp => emp.EmployeeId == searchId);
                    hasFilter = true;
                }

                // Lọc theo First Name nếu có
                if (!string.IsNullOrEmpty(firstName))
                {
                    query = query.Where(emp => emp.FirstName.Contains(firstName));
                    hasFilter = true;
                }

                // Lọc theo Middle Name nếu có
                if (!string.IsNullOrEmpty(middleName))
                {
                    query = query.Where(emp => emp.MiddleName != null && emp.MiddleName.Contains(middleName));
                    hasFilter = true;
                }

                // Lọc theo Last Name nếu có
                if (!string.IsNullOrEmpty(lastName))
                {
                    query = query.Where(emp => emp.LastName.Contains(lastName));
                    hasFilter = true;
                }

                // Lọc theo Department Name (chứ không phải Department Id)
                if (!string.IsNullOrEmpty(deptNameSearch))
                {
                    query = query.Where(emp => emp.Department != null && emp.Department.DepartmentName.Contains(deptNameSearch));
                    hasFilter = true;
                }

                var results = query.OrderBy(emp => emp.EmployeeId).ToList();
                dgEmployees.ItemsSource = results;

                if (results.Count == 0 && hasFilter)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp với điều kiện tìm kiếm!", "Kết quả tìm kiếm", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 5. Chức năng RESET (Làm mới form & danh sách)
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
            LoadEmployees();
        }

        private void ResetForm()
        {
            txtId.Clear();
            txtId.IsReadOnly = false;
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            cbDept.SelectedIndex = -1;
            cbDept.Text = string.Empty;
            dgEmployees.SelectedItem = null;
            txtId.Focus();
        }

        // 6. Sự kiện chọn dòng trong DataGrid
        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employee selectedEmployee)
            {
                txtId.Text = selectedEmployee.EmployeeId.ToString();
                txtId.IsReadOnly = true; // Khóa Id khi đang chọn để update/delete
                txtFirstName.Text = selectedEmployee.FirstName;
                txtMiddleName.Text = selectedEmployee.MiddleName ?? string.Empty;
                txtLastName.Text = selectedEmployee.LastName;
                cbDept.SelectedValue = selectedEmployee.DepartmentId;
            }
        }

        // 7. Sự kiện nhấn phím Enter tại ô Id: Tự động tra cứu và hiển thị thông tin nếu ID đã tồn tại
        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string idText = txtId.Text.Trim();
                if (string.IsNullOrEmpty(idText))
                {
                    return;
                }

                if (!int.TryParse(idText, out int searchId) || searchId <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Id là số nguyên dương hợp lệ!", "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtId.Focus();
                    return;
                }

                using var context = new AppDbContext();
                var emp = context.Employees
                    .Include(e => e.Department)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.EmployeeId == searchId);

                if (emp != null)
                {
                    // Tự động điền toàn bộ thông tin của nhân viên vào các ô nhập
                    txtFirstName.Text = emp.FirstName;
                    txtMiddleName.Text = emp.MiddleName ?? string.Empty;
                    txtLastName.Text = emp.LastName;
                    cbDept.SelectedValue = emp.DepartmentId;

                    // Đồng thời highlight và cuộn đến dòng tương ứng trên DataGrid
                    if (dgEmployees.ItemsSource is System.Collections.IEnumerable list)
                    {
                        foreach (var item in list)
                        {
                            if (item is Employee gridEmp && gridEmp.EmployeeId == emp.EmployeeId)
                            {
                                dgEmployees.SelectedItem = item;
                                dgEmployees.ScrollIntoView(item);
                                break;
                            }
                        }
                    }

                    // Di chuyển con trỏ chuột đến ô First Name để người dùng có thể cập nhật nhanh
                    txtFirstName.Focus();
                    txtFirstName.SelectAll();
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy nhân viên nào có ID: {searchId} trong cơ sở dữ liệu!\nBạn có thể tiếp tục nhập thông tin để thêm mới.", 
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Xóa trắng các trường thông tin bên dưới để sẵn sàng thêm mới
                    txtFirstName.Clear();
                    txtMiddleName.Clear();
                    txtLastName.Clear();
                    cbDept.SelectedIndex = -1;
                    cbDept.Text = string.Empty;
                    dgEmployees.SelectedItem = null;
                    txtFirstName.Focus();
                }
            }
        }
    }
}
