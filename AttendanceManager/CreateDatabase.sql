-- ========================================================
-- Script tạo Database & Bảng cho Hệ thống Quản Lý Điểm Danh (Course Attendance System)
-- ========================================================

USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CourseAttendanceDB')
BEGIN
    CREATE DATABASE CourseAttendanceDB;
END
GO

USE CourseAttendanceDB;
GO

-- 1. Bảng Courses
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
BEGIN
    CREATE TABLE Courses (
        CourseId INT PRIMARY KEY,
        CourseCode NVARCHAR(20) NOT NULL,
        CourseName NVARCHAR(150) NOT NULL,
        Description NVARCHAR(300) NULL
    );
END
GO

-- 2. Bảng Schedules
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Schedules')
BEGIN
    CREATE TABLE Schedules (
        ScheduleId INT PRIMARY KEY,
        CourseId INT NOT NULL,
        TeachingDate DATETIME2 NOT NULL,
        Slot INT NOT NULL,
        TimeSlot NVARCHAR(50) NOT NULL,
        Room NVARCHAR(50) NOT NULL,
        CONSTRAINT FK_Schedules_Courses FOREIGN KEY (CourseId) 
            REFERENCES Courses(CourseId) ON DELETE CASCADE
    );
END
GO

-- 3. Bảng Students
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
BEGIN
    CREATE TABLE Students (
        StudentId INT PRIMARY KEY,
        RollNumber NVARCHAR(20) NOT NULL,
        FirstName NVARCHAR(50) NOT NULL,
        MidName NVARCHAR(50) NULL,
        LastName NVARCHAR(50) NOT NULL,
        Gender NVARCHAR(10) NULL
    );
END
GO

-- 4. Bảng Attendances
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Attendances')
BEGIN
    CREATE TABLE Attendances (
        AttendanceId INT PRIMARY KEY,
        ScheduleId INT NOT NULL,
        StudentId INT NOT NULL,
        IsPresent BIT NOT NULL DEFAULT 0,
        Note NVARCHAR(250) NULL,
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT FK_Attendances_Schedules FOREIGN KEY (ScheduleId) 
            REFERENCES Schedules(ScheduleId) ON DELETE CASCADE,
        CONSTRAINT FK_Attendances_Students FOREIGN KEY (StudentId) 
            REFERENCES Students(StudentId) ON DELETE CASCADE,
        CONSTRAINT UQ_Schedule_Student UNIQUE (ScheduleId, StudentId)
    );
END
GO

-- 5. Dữ liệu mẫu (Seed Data)
IF NOT EXISTS (SELECT 1 FROM Courses)
BEGIN
    INSERT INTO Courses (CourseId, CourseCode, CourseName, Description) VALUES
    (1, N'PRN212', N'Basic Cross-Platform Application Programming with .NET', N'Lập trình ứng dụng đa nền tảng C# WPF & EF Core'),
    (2, N'PRM392', N'Mobile Programming with Android', N'Phát triển ứng dụng di động'),
    (3, N'CSD201', N'Data Structures and Algorithms', N'Cấu trúc dữ liệu và giải thuật'),
    (4, N'SWE201c', N'Introduction to Software Engineering', N'Nhập môn Kỹ nghệ phần mềm');
END
GO

IF NOT EXISTS (SELECT 1 FROM Schedules)
BEGIN
    INSERT INTO Schedules (ScheduleId, CourseId, TeachingDate, Slot, TimeSlot, Room) VALUES
    (1, 1, '2026-10-20', 1, N'07:30 - 09:50', N'BE-302'),
    (2, 1, '2026-10-22', 2, N'10:00 - 12:20', N'BE-302'),
    (3, 1, '2026-10-24', 3, N'12:50 - 15:10', N'AL-105'),
    (4, 1, '2026-10-27', 4, N'15:20 - 17:40', N'AL-105'),
    (5, 2, '2026-10-21', 1, N'07:30 - 09:50', N'DE-201'),
    (6, 2, '2026-10-23', 3, N'12:50 - 15:10', N'DE-201'),
    (7, 3, '2026-10-20', 4, N'15:20 - 17:40', N'CS-101'),
    (8, 3, '2026-10-25', 2, N'10:00 - 12:20', N'CS-101'),
    (9, 4, '2026-10-24', 5, N'17:50 - 20:10', N'BE-405');
END
GO

IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (StudentId, RollNumber, FirstName, MidName, LastName, Gender) VALUES
    (1001, N'HE180011', N'An', N'Văn', N'Nguyễn', N'Nam'),
    (1002, N'HE180022', N'Bình', N'Thị', N'Trần', N'Nữ'),
    (1003, N'SE180033', N'Cường', N'Quốc', N'Lê', N'Nam'),
    (1004, N'SE180044', N'Dung', N'Thị', N'Phạm', N'Nữ'),
    (1005, N'HE180055', N'Đạt', N'Tiến', N'Hoàng', N'Nam'),
    (1006, N'SE180066', N'Hương', N'Mai', N'Vũ', N'Nữ'),
    (1007, N'QE180077', N'Khoa', N'Đăng', N'Đỗ', N'Nam'),
    (1008, N'SE180088', N'Linh', N'Thùy', N'Bùi', N'Nữ'),
    (1009, N'HE180099', N'Minh', N'Nhật', N'Ngô', N'Nam'),
    (1010, N'SE180100', N'Nam', N'Phương', N'Dương', N'Nam');
END
GO

IF NOT EXISTS (SELECT 1 FROM Attendances)
BEGIN
    INSERT INTO Attendances (AttendanceId, ScheduleId, StudentId, IsPresent, Note) VALUES
    (1, 1, 1001, 1, N''),
    (2, 1, 1002, 1, N''),
    (3, 1, 1003, 0, N'Nghỉ có phép (bị sốt)'),
    (4, 1, 1004, 1, N''),
    (5, 1, 1005, 1, N'Vào muộn 10 phút'),
    (6, 1, 1006, 1, N''),
    (7, 1, 1007, 0, N'Vắng không phép'),
    (8, 1, 1008, 1, N''),
    (9, 1, 1009, 1, N''),
    (10, 1, 1010, 1, N''),

    (11, 2, 1001, 1, N''),
    (12, 2, 1002, 1, N''),
    (13, 2, 1003, 1, N''),
    (14, 2, 1004, 1, N''),
    (15, 2, 1005, 1, N''),
    (16, 2, 1006, 0, N'Trùng lịch thi IELTS'),
    (17, 2, 1007, 1, N''),
    (18, 2, 1008, 1, N''),
    (19, 2, 1009, 1, N''),
    (20, 2, 1010, 0, N'Vắng'),

    (21, 3, 1001, 1, N''),
    (22, 3, 1002, 1, N''),
    (23, 3, 1003, 1, N''),
    (24, 3, 1004, 1, N''),
    (25, 3, 1005, 1, N''),
    (26, 3, 1006, 1, N''),
    (27, 3, 1007, 1, N''),
    (28, 3, 1008, 1, N''),
    (29, 3, 1009, 1, N''),
    (30, 3, 1010, 1, N''),

    (31, 5, 1001, 1, N''),
    (32, 5, 1002, 0, N'Bị hỏng xe'),
    (33, 5, 1003, 1, N''),
    (34, 5, 1004, 1, N''),
    (35, 5, 1005, 1, N''),

    (36, 7, 1006, 1, N''),
    (37, 7, 1007, 1, N''),
    (38, 7, 1008, 1, N''),
    (39, 7, 1009, 0, N'Ốm'),
    (40, 7, 1010, 1, N''),

    (41, 9, 1001, 1, N''),
    (42, 9, 1003, 1, N''),
    (43, 9, 1005, 1, N''),
    (44, 9, 1007, 1, N''),
    (45, 9, 1009, 1, N'');
END
GO
