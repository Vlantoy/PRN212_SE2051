using System;
using System.Collections.Generic;
using System.Linq;
using AttendanceManager.DAL;
using AttendanceManager.Models;
using AttendanceManager.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.BLL
{
    public class AttendanceService
    {
        public List<Course> GetCourses()
        {
            return CourseDAO.GetAllCourses();
        }

        public List<Schedule> GetSchedulesByCourse(int courseId)
        {
            return ScheduleDAO.GetSchedulesByCourseId(courseId);
        }

        public List<AttendanceItemViewModel> GetAttendancesBySchedule(int scheduleId)
        {
            return AttendanceDAO.GetAttendanceViewModelsByScheduleId(scheduleId);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của toàn bộ dữ liệu trước khi lưu
        /// </summary>
        public bool ValidateBeforeSave(Course? course, Schedule? schedule, IList<AttendanceItemViewModel> items, 
                                      out string errorMessage, out AttendanceItemViewModel? invalidItem)
        {
            errorMessage = string.Empty;
            invalidItem = null;

            if (course == null)
            {
                errorMessage = "Vui lòng chọn một Course trước khi thực hiện lưu!";
                return false;
            }

            if (schedule == null)
            {
                errorMessage = "Vui lòng chọn một Schedule trước khi thực hiện lưu!";
                return false;
            }

            if (items == null || items.Count == 0)
            {
                errorMessage = "Danh sách điểm danh trống! Không có dữ liệu để lưu.";
                return false;
            }

            // Row-level validation: kiểm tra độ dài ghi chú
            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.Note) && item.Note.Trim().Length > 200)
                {
                    errorMessage = $"Ghi chú của sinh viên [{item.RollNumber} - {item.FullName}] quá dài ({item.Note.Trim().Length} ký tự)!\n\nĐộ dài ghi chú tối đa cho phép là 200 ký tự.";
                    invalidItem = item;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Thực hiện lưu điểm danh xuống CSDL thông qua DAL
        /// </summary>
        public bool SaveAttendances(int scheduleId, IList<AttendanceItemViewModel> items, out int updatedCount, out string errorMessage)
        {
            updatedCount = 0;
            errorMessage = string.Empty;

            try
            {
                updatedCount = AttendanceDAO.SaveAttendances(scheduleId, items);
                foreach (var item in items)
                {
                    item.AcceptChanges();
                }
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                errorMessage = "Dữ liệu đã bị thay đổi bởi phiên làm việc khác trong quá trình lưu!\nVui lòng tải lại trang và thực hiện lại.";
                return false;
            }
            catch (DbUpdateException dbEx)
            {
                errorMessage = $"Lỗi cập nhật CSDL: {dbEx.InnerException?.Message ?? dbEx.Message}";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Đã xảy ra lỗi khi lưu: {ex.Message}";
                return false;
            }
        }
    }
}
