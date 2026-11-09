using System;
using System.IO;
using AttendanceManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Attendance> Attendances => Set<Attendance>();

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = GetConnectionString();
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        public static string GetConnectionString()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();
                string? connStr = configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connStr))
                {
                    return connStr;
                }
            }
            catch
            {
                // Fallback below
            }

            return "Server=.\\SQLEXPRESS;Database=CourseAttendanceDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.CourseId).ValueGeneratedNever();
                entity.Property(c => c.CourseCode).IsRequired().HasMaxLength(20);
                entity.Property(c => c.CourseName).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Description).HasMaxLength(300);
            });

            // Configure Schedule
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedules");
                entity.HasKey(s => s.ScheduleId);
                entity.Property(s => s.ScheduleId).ValueGeneratedNever();
                entity.Property(s => s.TeachingDate).IsRequired();
                entity.Property(s => s.Slot).IsRequired();
                entity.Property(s => s.TimeSlot).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Room).IsRequired().HasMaxLength(50);

                entity.HasOne(s => s.Course)
                      .WithMany(c => c.Schedules)
                      .HasForeignKey(s => s.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(st => st.StudentId);
                entity.Property(st => st.StudentId).ValueGeneratedNever();
                entity.Property(st => st.RollNumber).IsRequired().HasMaxLength(20);
                entity.Property(st => st.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(st => st.MidName).HasMaxLength(50);
                entity.Property(st => st.LastName).IsRequired().HasMaxLength(50);
                entity.Property(st => st.Gender).HasMaxLength(10);
            });

            // Configure Attendance
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendances");
                entity.HasKey(a => a.AttendanceId);
                entity.Property(a => a.AttendanceId).ValueGeneratedNever();
                entity.Property(a => a.IsPresent).IsRequired();
                entity.Property(a => a.Note).HasMaxLength(250);

                entity.HasOne(a => a.Schedule)
                      .WithMany(s => s.Attendances)
                      .HasForeignKey(a => a.ScheduleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Student)
                      .WithMany(st => st.Attendances)
                      .HasForeignKey(a => a.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure a student only has one attendance record per schedule
                entity.HasIndex(a => new { a.ScheduleId, a.StudentId }).IsUnique();
            });

            // 1. Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseCode = "PRN212", CourseName = "Basic Cross-Platform Application Programming with .NET", Description = "Lập trình ứng dụng đa nền tảng C# WPF & EF Core" },
                new Course { CourseId = 2, CourseCode = "PRM392", CourseName = "Mobile Programming with Android", Description = "Phát triển ứng dụng di động" },
                new Course { CourseId = 3, CourseCode = "CSD201", CourseName = "Data Structures and Algorithms", Description = "Cấu trúc dữ liệu và giải thuật" },
                new Course { CourseId = 4, CourseCode = "SWE201c", CourseName = "Introduction to Software Engineering", Description = "Nhập môn Kỹ nghệ phần mềm" }
            );

            // 2. Seed Schedules
            modelBuilder.Entity<Schedule>().HasData(
                // PRN212
                new Schedule { ScheduleId = 1, CourseId = 1, TeachingDate = new DateTime(2026, 10, 20), Slot = 1, TimeSlot = "07:30 - 09:50", Room = "BE-302" },
                new Schedule { ScheduleId = 2, CourseId = 1, TeachingDate = new DateTime(2026, 10, 22), Slot = 2, TimeSlot = "10:00 - 12:20", Room = "BE-302" },
                new Schedule { ScheduleId = 3, CourseId = 1, TeachingDate = new DateTime(2026, 10, 24), Slot = 3, TimeSlot = "12:50 - 15:10", Room = "AL-105" },
                new Schedule { ScheduleId = 4, CourseId = 1, TeachingDate = new DateTime(2026, 10, 27), Slot = 4, TimeSlot = "15:20 - 17:40", Room = "AL-105" },
                // PRM392
                new Schedule { ScheduleId = 5, CourseId = 2, TeachingDate = new DateTime(2026, 10, 21), Slot = 1, TimeSlot = "07:30 - 09:50", Room = "DE-201" },
                new Schedule { ScheduleId = 6, CourseId = 2, TeachingDate = new DateTime(2026, 10, 23), Slot = 3, TimeSlot = "12:50 - 15:10", Room = "DE-201" },
                // CSD201
                new Schedule { ScheduleId = 7, CourseId = 3, TeachingDate = new DateTime(2026, 10, 20), Slot = 4, TimeSlot = "15:20 - 17:40", Room = "CS-101" },
                new Schedule { ScheduleId = 8, CourseId = 3, TeachingDate = new DateTime(2026, 10, 25), Slot = 2, TimeSlot = "10:00 - 12:20", Room = "CS-101" },
                // SWE201c
                new Schedule { ScheduleId = 9, CourseId = 4, TeachingDate = new DateTime(2026, 10, 24), Slot = 5, TimeSlot = "17:50 - 20:10", Room = "BE-405" }
            );

            // 3. Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1001, RollNumber = "HE180011", FirstName = "An", MidName = "Văn", LastName = "Nguyễn", Gender = "Nam" },
                new Student { StudentId = 1002, RollNumber = "HE180022", FirstName = "Bình", MidName = "Thị", LastName = "Trần", Gender = "Nữ" },
                new Student { StudentId = 1003, RollNumber = "SE180033", FirstName = "Cường", MidName = "Quốc", LastName = "Lê", Gender = "Nam" },
                new Student { StudentId = 1004, RollNumber = "SE180044", FirstName = "Dung", MidName = "Thị", LastName = "Phạm", Gender = "Nữ" },
                new Student { StudentId = 1005, RollNumber = "HE180055", FirstName = "Đạt", MidName = "Tiến", LastName = "Hoàng", Gender = "Nam" },
                new Student { StudentId = 1006, RollNumber = "SE180066", FirstName = "Hương", MidName = "Mai", LastName = "Vũ", Gender = "Nữ" },
                new Student { StudentId = 1007, RollNumber = "QE180077", FirstName = "Khoa", MidName = "Đăng", LastName = "Đỗ", Gender = "Nam" },
                new Student { StudentId = 1008, RollNumber = "SE180088", FirstName = "Linh", MidName = "Thùy", LastName = "Bùi", Gender = "Nữ" },
                new Student { StudentId = 1009, RollNumber = "HE180099", FirstName = "Minh", MidName = "Nhật", LastName = "Ngô", Gender = "Nam" },
                new Student { StudentId = 1010, RollNumber = "SE180100", FirstName = "Nam", MidName = "Phương", LastName = "Dương", Gender = "Nam" }
            );

            // 4. Seed Attendances for Schedule 1 (PRN212 - Slot 1 Ngày 20/10)
            modelBuilder.Entity<Attendance>().HasData(
                new Attendance { AttendanceId = 1, ScheduleId = 1, StudentId = 1001, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 2, ScheduleId = 1, StudentId = 1002, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 3, ScheduleId = 1, StudentId = 1003, IsPresent = false, Note = "Nghỉ có phép (bị sốt)" },
                new Attendance { AttendanceId = 4, ScheduleId = 1, StudentId = 1004, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 5, ScheduleId = 1, StudentId = 1005, IsPresent = true, Note = "Vào muộn 10 phút" },
                new Attendance { AttendanceId = 6, ScheduleId = 1, StudentId = 1006, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 7, ScheduleId = 1, StudentId = 1007, IsPresent = false, Note = "Vắng không phép" },
                new Attendance { AttendanceId = 8, ScheduleId = 1, StudentId = 1008, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 9, ScheduleId = 1, StudentId = 1009, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 10, ScheduleId = 1, StudentId = 1010, IsPresent = true, Note = "" },

                // Attendances for Schedule 2 (PRN212 - Slot 2 Ngày 22/10)
                new Attendance { AttendanceId = 11, ScheduleId = 2, StudentId = 1001, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 12, ScheduleId = 2, StudentId = 1002, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 13, ScheduleId = 2, StudentId = 1003, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 14, ScheduleId = 2, StudentId = 1004, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 15, ScheduleId = 2, StudentId = 1005, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 16, ScheduleId = 2, StudentId = 1006, IsPresent = false, Note = "Trùng lịch thi IELTS" },
                new Attendance { AttendanceId = 17, ScheduleId = 2, StudentId = 1007, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 18, ScheduleId = 2, StudentId = 1008, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 19, ScheduleId = 2, StudentId = 1009, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 20, ScheduleId = 2, StudentId = 1010, IsPresent = false, Note = "Vắng" },

                // Attendances for Schedule 3 (PRN212 - Slot 3 Ngày 24/10)
                new Attendance { AttendanceId = 21, ScheduleId = 3, StudentId = 1001, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 22, ScheduleId = 3, StudentId = 1002, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 23, ScheduleId = 3, StudentId = 1003, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 24, ScheduleId = 3, StudentId = 1004, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 25, ScheduleId = 3, StudentId = 1005, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 26, ScheduleId = 3, StudentId = 1006, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 27, ScheduleId = 3, StudentId = 1007, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 28, ScheduleId = 3, StudentId = 1008, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 29, ScheduleId = 3, StudentId = 1009, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 30, ScheduleId = 3, StudentId = 1010, IsPresent = true, Note = "" },

                // Attendances for Schedule 5 (PRM392)
                new Attendance { AttendanceId = 31, ScheduleId = 5, StudentId = 1001, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 32, ScheduleId = 5, StudentId = 1002, IsPresent = false, Note = "Bị hỏng xe" },
                new Attendance { AttendanceId = 33, ScheduleId = 5, StudentId = 1003, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 34, ScheduleId = 5, StudentId = 1004, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 35, ScheduleId = 5, StudentId = 1005, IsPresent = true, Note = "" },

                // Attendances for Schedule 7 (CSD201)
                new Attendance { AttendanceId = 36, ScheduleId = 7, StudentId = 1006, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 37, ScheduleId = 7, StudentId = 1007, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 38, ScheduleId = 7, StudentId = 1008, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 39, ScheduleId = 7, StudentId = 1009, IsPresent = false, Note = "Ốm" },
                new Attendance { AttendanceId = 40, ScheduleId = 7, StudentId = 1010, IsPresent = true, Note = "" },

                // Attendances for Schedule 9 (SWE201c)
                new Attendance { AttendanceId = 41, ScheduleId = 9, StudentId = 1001, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 42, ScheduleId = 9, StudentId = 1003, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 43, ScheduleId = 9, StudentId = 1005, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 44, ScheduleId = 9, StudentId = 1007, IsPresent = true, Note = "" },
                new Attendance { AttendanceId = 45, ScheduleId = 9, StudentId = 1009, IsPresent = true, Note = "" }
            );
        }
    }
}
