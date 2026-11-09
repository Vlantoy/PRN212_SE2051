using System;
using System.Data;
using System.Data.SqlClient;

namespace CourseStudent.DAL
{
    internal class DAO
    {
        public static SqlConnection GetConnection()
        {
            string conn = "server=.\\SQLEXPRESS;database=SE1963;Trusted_Connection=True;TrustServerCertificate=True;";
            return new SqlConnection(conn);
        }

        public static DataTable GetDataBySql(string sql, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static int ExcuteSql(string sql, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }
    }
}
