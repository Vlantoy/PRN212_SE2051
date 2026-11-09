using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EmployeeManager.Data;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AppManager.Views
{
    /// <summary>
    /// Interaction logic for EmployeeManagementView.xaml
    /// </summary>
    public partial class EmployeeManagementView : UserControl
    {
        private bool _isLoaded = false;

        public EmployeeManagementView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded) return;

            try
            {
                await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    context.Database.EnsureCreated();
                });

                await LoadDepartmentsAsync();
                await LoadEmployeesAsync();
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối CSDL Employee: {ex.Message}", "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var departments = await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    return context.Departments
                        .AsNoTracking()
                        .OrderBy(d => d.DepartmentId)
                        .ToList();
                });

                cbDept.ItemsSource = departments;
            }
            catch { }
        }

        public async Task LoadEmployeesAsync()
        {
            try
            {
                var employees = await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    return context.Employees
                        .Include(emp => emp.Department)
                        .AsNoTracking()
                        .OrderBy(emp => emp.EmployeeId)
                        .ToList();
                });

                dgEmployees.ItemsSource = employees;
            }
            catch { }
        }

        private bool ValidateName(string name, string fieldName, bool isOptional, out string errorMessage)
        {
            errorMessage = string.Empty;
            string trimmed = name.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (isOptional) return true;
                errorMessage = $"{fieldName} không được để trống!";
                return false;
            }

            if (trimmed.Length < 2)
            {
                errorMessage = $"{fieldName} phải có độ dài tối thiểu từ 2 ký tự trở lên!";
                return false;
            }

            if (trimmed.Any(char.IsDigit))
            {
                errorMessage = $"{fieldName} không được chứa số (chỉ được là chữ cái)!";
                return false;
            }

            if (!Regex.IsMatch(trimmed, @"^[\p{L}\s\.\-]+$"))
            {
                errorMessage = $"{fieldName} chỉ được chứa chữ cái, không được chứa ký tự đặc biệt!";
                return false;
            }

            return true;
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                if (!ValidateName(firstName, "First Name", isOptional: false, out string errFirst))
                {
                    MessageBox.Show(errFirst, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtFirstName.Focus();
                    return;
                }

                if (!ValidateName(lastName, "Last Name", isOptional: false, out string errLast))
                {
                    MessageBox.Show(errLast, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLastName.Focus();
                    return;
                }

                if (!ValidateName(middleName, "Middle Name", isOptional: true, out string errMid))
                {
                    MessageBox.Show(errMid, "Lỗi Validation Tên", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMiddleName.Focus();
                    return;
                }

                int deptId = 0;
                if (cbDept.SelectedValue is int selectedId)
                {
                    deptId = selectedId;
                }
                else if (!string.IsNullOrWhiteSpace(cbDept.Text))
                {
                    string inputDept = cbDept.Text.Trim();
                    using var ctx = new AppDbContext();
                    var foundDept = ctx.Departments.FirstOrDefault(d => d.DepartmentName.ToLower() == inputDept.ToLower());
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

                await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    int newId;
                    if (!string.IsNullOrEmpty(idText))
                    {
                        newId = int.Parse(idText);
                        if (context.Employees.Any(emp => emp.EmployeeId == newId))
                        {
                            throw new Exception($"Nhân viên với Id '{newId}' đã tồn tại!");
                        }
                    }
                    else
                    {
                        int maxId = context.Employees.Any() ? context.Employees.Max(emp => emp.EmployeeId) : 100;
                        newId = maxId + 1;
                    }

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
                });

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadEmployeesAsync();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                if (string.IsNullOrEmpty(idText) || !int.TryParse(idText, out int empId) || empId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên từ bảng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                if (cbDept.SelectedValue is not int deptId)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    var emp = context.Employees.FirstOrDefault(e => e.EmployeeId == empId);
                    if (emp != null)
                    {
                        emp.FirstName = firstName;
                        emp.MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
                        emp.LastName = lastName;
                        emp.DepartmentId = deptId;
                        emp.Gender = Employee.InferGender(firstName, middleName, lastName);
                        context.SaveChanges();
                    }
                });

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadEmployeesAsync();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                if (string.IsNullOrEmpty(idText) || !int.TryParse(idText, out int empId))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên từ bảng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var confirm = MessageBox.Show($"Xác nhận xóa nhân viên ID {empId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    var emp = context.Employees.FirstOrDefault(e => e.EmployeeId == empId);
                    if (emp != null)
                    {
                        context.Employees.Remove(emp);
                        context.SaveChanges();
                    }
                });

                MessageBox.Show("Đã xóa nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadEmployeesAsync();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idText = txtId.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string lastName = txtLastName.Text.Trim();

                string deptNameSearch = string.Empty;
                if (cbDept.SelectedItem is Department dept) deptNameSearch = dept.DepartmentName;
                else if (!string.IsNullOrWhiteSpace(cbDept.Text)) deptNameSearch = cbDept.Text.Trim();

                var results = await Task.Run(() =>
                {
                    using var context = new AppDbContext();
                    IQueryable<Employee> query = context.Employees.Include(emp => emp.Department).AsNoTracking();

                    if (!string.IsNullOrEmpty(idText) && int.TryParse(idText, out int searchId))
                        query = query.Where(emp => emp.EmployeeId == searchId);

                    if (!string.IsNullOrEmpty(firstName))
                        query = query.Where(emp => emp.FirstName.Contains(firstName));

                    if (!string.IsNullOrEmpty(middleName))
                        query = query.Where(emp => emp.MiddleName != null && emp.MiddleName.Contains(middleName));

                    if (!string.IsNullOrEmpty(lastName))
                        query = query.Where(emp => emp.LastName.Contains(lastName));

                    if (!string.IsNullOrEmpty(deptNameSearch))
                        query = query.Where(emp => emp.Department != null && emp.Department.DepartmentName.Contains(deptNameSearch));

                    return query.OrderBy(emp => emp.EmployeeId).ToList();
                });

                dgEmployees.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
            await LoadEmployeesAsync();
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

        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employee selectedEmployee)
            {
                txtId.Text = selectedEmployee.EmployeeId.ToString();
                txtId.IsReadOnly = true;
                txtFirstName.Text = selectedEmployee.FirstName;
                txtMiddleName.Text = selectedEmployee.MiddleName ?? string.Empty;
                txtLastName.Text = selectedEmployee.LastName;
                cbDept.SelectedValue = selectedEmployee.DepartmentId;
            }
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string idText = txtId.Text.Trim();
                if (int.TryParse(idText, out int searchId))
                {
                    using var context = new AppDbContext();
                    var emp = context.Employees.Include(x => x.Department).AsNoTracking().FirstOrDefault(x => x.EmployeeId == searchId);
                    if (emp != null)
                    {
                        txtFirstName.Text = emp.FirstName;
                        txtMiddleName.Text = emp.MiddleName ?? string.Empty;
                        txtLastName.Text = emp.LastName;
                        cbDept.SelectedValue = emp.DepartmentId;
                        txtFirstName.Focus();
                    }
                }
            }
        }
    }
}
