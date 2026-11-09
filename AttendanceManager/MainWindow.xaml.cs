using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AttendanceManager.BLL;
using AttendanceManager.Data;
using AttendanceManager.Models;
using AttendanceManager.ViewModels;

namespace AttendanceManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml (GUI Layer)
    /// Chỉ đảm nhận vai trò hiển thị giao diện và hứng sự kiện người dùng.
    /// Toàn bộ logic nghiệp vụ, validation và truy xuất CSDL được ủy quyền cho Service và DAO.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AttendanceService _service = new();
        private readonly ObservableCollection<AttendanceItemViewModel> _attendanceList = new();

        private bool _isRevertingCourse = false;
        private bool _isRevertingSchedule = false;

        private Course? _previousCourse = null;
        private Schedule? _previousSchedule = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Form Load & Initialization

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new AppDbContext();
                context.Database.EnsureCreated();
                dgAttendance.ItemsSource = _attendanceList;
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo CSDL: {ex.Message}", "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCourses()
        {
            try
            {
                var courses = _service.GetCourses();
                cbCourse.ItemsSource = courses;
                cbCourse.DisplayMemberPath = "DisplayText";
                cbCourse.SelectedValuePath = "CourseId";

                if (courses.Count > 0)
                {
                    cbCourse.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách Course: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSchedulesForCourse(int courseId)
        {
            try
            {
                var schedules = _service.GetSchedulesByCourse(courseId);
                cbSchedule.ItemsSource = schedules;
                cbSchedule.DisplayMemberPath = "DisplayText";
                cbSchedule.SelectedValuePath = "ScheduleId";

                if (schedules.Count > 0)
                {
                    cbSchedule.SelectedIndex = 0;
                }
                else
                {
                    _attendanceList.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách Schedule: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAttendanceList(int scheduleId)
        {
            try
            {
                _attendanceList.Clear();
                var items = _service.GetAttendancesBySchedule(scheduleId);
                foreach (var item in items)
                {
                    _attendanceList.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách điểm danh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ComboBox Events (Với Validation cảnh báo chưa lưu)

        private void cbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isRevertingCourse) return;

            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Bạn có những thay đổi điểm danh chưa được lưu!\n\nBạn có muốn LƯU các thay đổi này trước khi chuyển sang Course khác không?",
                    "Cảnh Báo",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (!ValidateAndSaveAttendance())
                    {
                        RevertCourseSelection();
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    RevertCourseSelection();
                    return;
                }
            }

            var selectedCourse = cbCourse.SelectedItem as Course;
            _previousCourse = selectedCourse;

            if (selectedCourse == null)
            {
                cbSchedule.ItemsSource = null;
                _attendanceList.Clear();
                return;
            }

            LoadSchedulesForCourse(selectedCourse.CourseId);
        }

        private void RevertCourseSelection()
        {
            _isRevertingCourse = true;
            cbCourse.SelectedItem = _previousCourse;
            _isRevertingCourse = false;
        }

        private void cbSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isRevertingSchedule) return;

            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Bạn có những thay đổi điểm danh chưa được lưu!\n\nBạn có muốn LƯU các thay đổi này trước khi chuyển sang buổi học khác không?",
                    "Cảnh Báo",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (!ValidateAndSaveAttendance())
                    {
                        RevertScheduleSelection();
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    RevertScheduleSelection();
                    return;
                }
            }

            var selectedSchedule = cbSchedule.SelectedItem as Schedule;
            _previousSchedule = selectedSchedule;

            if (selectedSchedule == null)
            {
                _attendanceList.Clear();
                return;
            }

            LoadAttendanceList(selectedSchedule.ScheduleId);
        }

        private void RevertScheduleSelection()
        {
            _isRevertingSchedule = true;
            cbSchedule.SelectedItem = _previousSchedule;
            _isRevertingSchedule = false;
        }

        #endregion

        #region Save Action & Validation

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ValidateAndSaveAttendance(showSuccessDialog: true);
        }

        private bool ValidateAndSaveAttendance(bool showSuccessDialog = false)
        {
            var selectedCourse = cbCourse.SelectedItem as Course;
            var selectedSchedule = cbSchedule.SelectedItem as Schedule;

            // 1. Gọi Service thực hiện kiểm tra tính hợp lệ
            if (!_service.ValidateBeforeSave(selectedCourse, selectedSchedule, _attendanceList, 
                                            out string validationError, out var invalidItem))
            {
                MessageBox.Show(validationError, "Cảnh Báo Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (invalidItem != null)
                {
                    dgAttendance.SelectedItem = invalidItem;
                    dgAttendance.ScrollIntoView(invalidItem);
                }
                return false;
            }

            // 2. Kiểm tra xem có thay đổi thực sự hay không
            int modifiedCount = _attendanceList.Count(x => x.IsModified);
            if (modifiedCount == 0)
            {
                if (showSuccessDialog)
                {
                    MessageBox.Show("Không có thay đổi nào mới về điểm danh để lưu.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return true;
            }

            // 3. Hộp thoại xác nhận
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn lưu lại kết quả điểm danh cho buổi học:\n\n" +
                $"• Course: {selectedCourse!.DisplayText}\n" +
                $"• Schedule: {selectedSchedule!.DisplayText}\n" +
                $"• Số bản ghi thay đổi: {modifiedCount}\n\n" +
                $"Xác nhận lưu vào CSDL?",
                "Xác Nhận Lưu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.Yes)
            {
                return false;
            }

            // 4. Gọi Service thực hiện lưu xuống CSDL
            if (_service.SaveAttendances(selectedSchedule.ScheduleId, _attendanceList, out int updatedCount, out string dbError))
            {
                if (showSuccessDialog)
                {
                    MessageBox.Show($"Đã lưu điểm danh thành công!\n({updatedCount} bản ghi đã được cập nhật)", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return true;
            }
            else
            {
                MessageBox.Show(dbError, "Lỗi Lưu CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool HasUnsavedChanges()
        {
            return _attendanceList.Any(x => x.IsModified);
        }

        #endregion

        #region Window Closing

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Bạn có những thay đổi điểm danh chưa được lưu!\n\nBạn có muốn LƯU các thay đổi này trước khi thoát không?",
                    "Cảnh Báo",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (!ValidateAndSaveAttendance())
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion
    }
}
