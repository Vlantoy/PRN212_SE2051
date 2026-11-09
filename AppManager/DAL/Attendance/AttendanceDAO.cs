using System;
using System.Collections.Generic;
using System.Linq;
using AttendanceManager.Data;
using AttendanceManager.Models;
using AttendanceManager.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.DAL
{
    public class AttendanceDAO
    {
        public static List<AttendanceItemViewModel> GetAttendanceViewModelsByScheduleId(int scheduleId)
        {
            using var context = new AppDbContext();

            var attendances = context.Attendances
                .Include(a => a.Student)
                .Where(a => a.ScheduleId == scheduleId)
                .OrderBy(a => a.Student!.RollNumber)
                .ToList();

            // Nếu buổi học này chưa có bản ghi điểm danh, tự động nạp từ bảng Students
            if (attendances.Count == 0)
            {
                var allStudents = context.Students.OrderBy(s => s.RollNumber).ToList();
                int maxId = context.Attendances.Any() ? context.Attendances.Max(a => a.AttendanceId) : 0;

                foreach (var student in allStudents)
                {
                    maxId++;
                    var newAtt = new Attendance
                    {
                        AttendanceId = maxId,
                        ScheduleId = scheduleId,
                        StudentId = student.StudentId,
                        IsPresent = true,
                        Note = string.Empty,
                        Student = student
                    };
                    attendances.Add(newAtt);
                }
            }

            int index = 1;
            var result = new List<AttendanceItemViewModel>();
            foreach (var att in attendances)
            {
                result.Add(new AttendanceItemViewModel
                {
                    Index = index++,
                    AttendanceId = att.AttendanceId,
                    ScheduleId = att.ScheduleId,
                    StudentId = att.StudentId,
                    RollNumber = att.Student?.RollNumber ?? string.Empty,
                    FullName = att.Student?.FullName ?? string.Empty,
                    Gender = att.Student?.Gender ?? string.Empty,
                    IsPresent = att.IsPresent,
                    Note = att.Note ?? string.Empty,
                    OriginalIsPresent = att.IsPresent,
                    OriginalNote = att.Note ?? string.Empty
                });
            }

            return result;
        }

        public static int SaveAttendances(int scheduleId, IEnumerable<AttendanceItemViewModel> items)
        {
            using var context = new AppDbContext();

            var existingAttendances = context.Attendances
                .Where(a => a.ScheduleId == scheduleId)
                .ToList();

            int currentMaxId = context.Attendances.Any() ? context.Attendances.Max(a => a.AttendanceId) : 0;
            int modifiedCount = 0;

            foreach (var item in items)
            {
                var record = existingAttendances.FirstOrDefault(a => a.StudentId == item.StudentId);

                if (record != null)
                {
                    if (record.IsPresent != item.IsPresent || (record.Note ?? string.Empty) != (item.Note ?? string.Empty).Trim())
                    {
                        record.IsPresent = item.IsPresent;
                        record.Note = string.IsNullOrWhiteSpace(item.Note) ? null : item.Note.Trim();
                        record.UpdatedAt = DateTime.Now;
                        modifiedCount++;
                    }
                }
                else
                {
                    currentMaxId++;
                    var newRecord = new Attendance
                    {
                        AttendanceId = currentMaxId,
                        ScheduleId = scheduleId,
                        StudentId = item.StudentId,
                        IsPresent = item.IsPresent,
                        Note = string.IsNullOrWhiteSpace(item.Note) ? null : item.Note.Trim(),
                        UpdatedAt = DateTime.Now
                    };
                    context.Attendances.Add(newRecord);
                    modifiedCount++;
                }
            }

            context.SaveChanges();
            return modifiedCount;
        }
    }
}
