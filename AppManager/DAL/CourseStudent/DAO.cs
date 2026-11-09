using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AppManager.DAL.CourseStudent
{
    public class DAO
    {
        public static SqlConnection GetConnection()
        {
            string conn = "server=.\\SQLEXPRESS;database=SE1963;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3";
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true);
                var config = builder.Build();
                string? cfgConn = config.GetConnectionString("CourseStudentConnection");
                if (!string.IsNullOrEmpty(cfgConn)) conn = cfgConn;
            }
            catch { }
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
