using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CNPM1
{
    /// <summary>
    /// Lớp tiện ích truy cập cơ sở dữ liệu SQL Server cho toàn bộ ứng dụng.
    /// Chuỗi kết nối được đọc từ App.config (mục connectionStrings, tên "QLCuaHang").
    /// Mọi giá trị đầu vào người dùng phải truyền qua SqlParameter (không nối chuỗi vào câu lệnh SQL)
    /// để tránh SQL Injection.
    /// </summary>
    public static class DbHelper
    {
        private const string ConnectionName = "QLCuaHang";

        /// <summary>Chuỗi kết nối đọc từ App.config.</summary>
        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[ConnectionName];
                if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
                {
                    throw new InvalidOperationException(
                        "Không tìm thấy chuỗi kết nối \"" + ConnectionName + "\" trong App.config.");
                }
                return cs.ConnectionString;
            }
        }

        /// <summary>Tạo đối tượng SqlConnection mới (chưa mở). Người gọi chịu trách nhiệm Dispose.</summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>Tạo SqlParameter; giá trị null được chuyển thành DBNull.Value.</summary>
        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>Kiểm tra có kết nối được tới CSDL hay không (dùng ở form đăng nhập hoặc khi khởi động).</summary>
        public static bool TestConnection(out string error)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                }
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Chạy câu SELECT và trả về DataTable (gán trực tiếp cho DataGridView.DataSource).
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, sql, parameters))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(table); // SqlDataAdapter tự mở/đóng kết nối
            }
            return table;
        }

        /// <summary>
        /// Chạy INSERT / UPDATE / DELETE. Trả về số dòng bị ảnh hưởng (int).
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, sql, parameters))
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Chạy câu lệnh trả về một giá trị (COUNT, SUM, SCOPE_IDENTITY...).
        /// Kiểu trả về là object; có thể là null hoặc DBNull.Value nếu không có kết quả,
        /// nên cần kiểm tra trước khi ép kiểu.
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, sql, parameters))
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Chạy một nhóm thao tác trong cùng một transaction (ví dụ: lưu hóa đơn + chi tiết hóa đơn
        /// + trừ tồn kho khi bấm "Hoàn tất thanh toán"). Nếu có lỗi, toàn bộ được rollback.
        /// </summary>
        public static void ExecuteTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        work(conn, tran);
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>INSERT / UPDATE / DELETE bên trong một transaction đã mở (dùng trong ExecuteTransaction).</summary>
        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction tran, string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = CreateCommand(conn, tran, sql, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Băm mật khẩu bằng SHA-256, trả về chuỗi hex IN HOA 64 ký tự (vừa cột MatKhau VARCHAR(255)).
        /// Khớp với CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', N'...'), 2) trong SQL Server (với mật khẩu ASCII).
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
                StringBuilder sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes) sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }

        /// <summary>ExecuteScalar bên trong một transaction đã mở (dùng để lấy SCOPE_IDENTITY()).</summary>
        public static object ExecuteScalar(SqlConnection conn, SqlTransaction tran, string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = CreateCommand(conn, tran, sql, parameters))
            {
                return cmd.ExecuteScalar();
            }
        }

        private static SqlCommand CreateCommand(SqlConnection conn, SqlTransaction tran, string sql, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, conn, tran);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }
    }
}