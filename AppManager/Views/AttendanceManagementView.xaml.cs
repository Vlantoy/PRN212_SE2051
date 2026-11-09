using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AttendanceManager.BLL;
using AttendanceManager.Data;
using AttendanceManager.Models;
using AttendanceManager.ViewModels;

namespace AppManager.Views
{
    /// <summary>
    /// Interaction logic for AttendanceManagementView.xaml
    /// </summary>
    public partial class AttendanceManagementView : UserControl
    {
        private readonly AttendanceService _service = new();
        private readonly ObservableCollection<AttendanceItemViewModel> _attendanceList = new();

        private bool _isLoaded = false;
        private bool _isRevertingCourse = false;
        private bool _isRevertingSchedule = false;

        private Course? _previousCourse = null;
        private Schedule? _previousSchedule = null;

        public AttendanceManagementView()
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

                dgAttendance.ItemsSource = _attendanceList;
                await LoadCoursesAsync();
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi CSDL: {ex.Message}", "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await Task.Run(() => _service.GetCourses());
                cbCourse.ItemsSource = courses;
                cbCourse.DisplayMemberPath = "DisplayText";
                cbCourse.SelectedValuePath = "CourseId";

                if (courses.Count > 0)
                {
                    cbCourse.SelectedIndex = 0;
                }
            }
            catch { }
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
            catch { }
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
            catch { }
        }

        private void cbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isRevertingCourse) return;

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

        private void cbSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isRevertingSchedule) return;

            var selectedSchedule = cbSchedule.SelectedItem as Schedule;
            _previousSchedule = selectedSchedule;

            if (selectedSchedule == null)
            {
                _attendanceList.Clear();
                return;
            }

            LoadAttendanceList(selectedSchedule.ScheduleId);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = cbCourse.SelectedItem as Course;
            var selectedSchedule = cbSchedule.SelectedItem as Schedule;

            if (!_service.ValidateBeforeSave(selectedCourse, selectedSchedule, _attendanceList, 
                                            out string validationError, out var invalidItem))
            {
                MessageBox.Show(validationError, "Cảnh Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int modifiedCount = _attendanceList.Count(x => x.IsModified);
            if (modifiedCount == 0)
            {
                MessageBox.Show("Không có thay đổi nào mới để lưu.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_service.SaveAttendances(selectedSchedule!.ScheduleId, _attendanceList, out int updatedCount, out string dbError))
            {
                MessageBox.Show($"Đã lưu thành công {updatedCount} bản ghi!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(dbError, "Lỗi Lưu CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool HasUnsavedChanges()
        {
            return _attendanceList.Any(x => x.IsModified);
        }
    }
}
