using CourseStudent.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CourseStudent.DAL
{
    internal class StudentDAO
    {
        public static List<Students> getAllStudent(string sql = "Select * From STUDENTS")
        {
            DataTable dt = DAO.GetDataBySql(sql);
            List<Students> list = new List<Students>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Students(
                   Convert.ToInt32(dr["StudentId"]),
                   dr["Roll#"].ToString() ?? "",
                   dr["FirstName"].ToString() ?? "",
                   dr["MidName"].ToString() ?? "",
                   dr["LastName"].ToString() ?? ""
                ));
            }
            return list;
        }

        public static Students? getStudentById(int studentId)
        {
            string sql = "Select * From STUDENTS Where StudentId = @studentId";
            SqlParameter[] parameters = { new SqlParameter("@studentId", studentId) };
            DataTable dt = DAO.GetDataBySql(sql, parameters);
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            return new Students(
                Convert.ToInt32(dr["StudentId"]),
                dr["Roll#"].ToString() ?? "",
                dr["FirstName"].ToString() ?? "",
                dr["MidName"].ToString() ?? "",
                dr["LastName"].ToString() ?? ""
            );
        }

        public static int insertStudent(Students s)
        {
            string sql = "INSERT INTO STUDENTS (Roll#, FirstName, MidName, LastName) VALUES (@rollNo, @firstName, @midName, @lastName)";
            SqlParameter[] parameters = {
                new SqlParameter("@rollNo", s.RollNo ?? ""),
                new SqlParameter("@firstName", s.FirstName ?? ""),
                new SqlParameter("@midName", s.MidName ?? ""),
                new SqlParameter("@lastName", s.LastName ?? "")
            };
            return DAO.ExcuteSql(sql, parameters);
        }

        public static int updateStudent(Students s)
        {
            string sql = "UPDATE STUDENTS SET Roll# = @rollNo, FirstName = @firstName, LastName = @lastName WHERE StudentId = @studentId";
            SqlParameter[] parameters = {
                new SqlParameter("@rollNo", s.RollNo ?? ""),
                new SqlParameter("@firstName", s.FirstName ?? ""),
                new SqlParameter("@lastName", s.LastName ?? ""),
                new SqlParameter("@studentId", s.StudentID)
            };
            return DAO.ExcuteSql(sql, parameters);
        }

        public static int deleteStudent(int studentId)
        {
            string sql = "DELETE FROM STUDENTS WHERE StudentId = @studentId";
            SqlParameter[] parameters = {
                new SqlParameter("@studentId", studentId)
            };
            return DAO.ExcuteSql(sql, parameters);
        }

        public static List<Students> searchStudents(string keyword)
        {
            string sql = "SELECT * FROM STUDENTS WHERE Roll# LIKE @kw OR FirstName LIKE @kw OR LastName LIKE @kw";
            SqlParameter[] parameters = {
                new SqlParameter("@kw", "%" + keyword + "%")
            };
            DataTable dt = DAO.GetDataBySql(sql, parameters);
            List<Students> list = new List<Students>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Students(
                    Convert.ToInt32(dr["StudentId"]),
                    dr["Roll#"].ToString() ?? "",
                    dr["FirstName"].ToString() ?? "",
                    dr["MidName"].ToString() ?? "",
                    dr["LastName"].ToString() ?? ""
                ));
            }
            return list;
        }
    }
}
