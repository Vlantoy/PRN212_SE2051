-- ========================================================
-- Script tạo Database & Bảng cho hệ thống Quản Lý Nhân Viên
-- ========================================================

USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END
GO

USE EmployeeDB;
GO

-- 1. Bảng Departments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    CREATE TABLE Departments (
        DepartmentId INT PRIMARY KEY,
        DepartmentName NVARCHAR(100) NOT NULL
    );
END
GO

-- 2. Bảng Employees
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId INT PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        MiddleName NVARCHAR(50) NULL,
        LastName NVARCHAR(50) NOT NULL,
        DepartmentId INT NOT NULL,
        CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId) 
            REFERENCES Departments(DepartmentId)
    );
END
GO

-- 3. Dữ liệu mẫu ban đầu (Seed data)
IF NOT EXISTS (SELECT 1 FROM Departments)
BEGIN
    INSERT INTO Departments (DepartmentId, DepartmentName) VALUES
    (1, N'IT'),
    (2, N'HR'),
    (3, N'Marketing'),
    (4, N'Finance'),
    (5, N'Sales');
END
GO

IF NOT EXISTS (SELECT 1 FROM Employees)
BEGIN
    INSERT INTO Employees (EmployeeId, FirstName, MiddleName, LastName, DepartmentId) VALUES
    (101, N'John', N'William', N'Smith', 1),
    (102, N'Sarah', N'Jane', N'Connor', 2),
    (103, N'Michael', N'David', N'Brown', 1),
    (104, N'Emma', N'Marie', N'Watson', 3),
    (105, N'James', N'Robert', N'Wilson', 4);
END
GO
