using AppManager.DAL.CourseStudent;
using AppManager.Models.CourseStudent;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppManager.Views
{
    /// <summary>
    /// Interaction logic for StudentManagementView.xaml
    /// </summary>
    public partial class StudentManagementView : UserControl
    {
        private bool _isLoaded = false;

        public StudentManagementView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var list = await Task.Run(() => StudentDAO.getAllStudent());
                grid1.ItemsSource = list;
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối CSDL SE1963: {ex.Message}", "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearInputs()
        {
            txtStudentId.Text = string.Empty;
            txtRollNo.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtSearch.Text = string.Empty;
            grid1.SelectedItem = null;
        }

        private void grid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid1.SelectedItem is Students student)
            {
                txtStudentId.Text = student.StudentID.ToString();
                txtRollNo.Text = student.RollNo;
                txtFirstName.Text = student.FirstName;
                txtLastName.Text = student.LastName;
            }
        }

        private async void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRollNo.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Vui lòng nhập Roll No và First Name!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Students s = new Students
                {
                    RollNo = txtRollNo.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    MidName = "",
                    LastName = txtLastName.Text.Trim()
                };

                int result = await Task.Run(() => StudentDAO.insertStudent(s));
                if (result > 0)
                {
                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm sinh viên thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text) || !int.TryParse(txtStudentId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Students s = new Students
                {
                    StudentID = id,
                    RollNo = txtRollNo.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    MidName = "",
                    LastName = txtLastName.Text.Trim()
                };

                int result = await Task.Run(() => StudentDAO.updateStudent(s));
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text) || !int.TryParse(txtStudentId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có ID: {id} không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    int result = await Task.Run(() => StudentDAO.deleteStudent(id));
                    if (result > 0)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Xóa sinh viên thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            await LoadDataAsync();
        }

        private async void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    await LoadDataAsync();
                }
                else
                {
                    var results = await Task.Run(() => StudentDAO.searchStudents(keyword));
                    grid1.ItemsSource = results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
