using System.Data;
using Microsoft.Data.SqlClient; // Use Microsoft.Data.SqlClient for modern .NET

namespace CompanyPrinters.DAL
{
    public class DALclass
    {
        private readonly string _connectionString;

        // Constructor captures the string
        public DALclass(string connectionString)
        {
            _connectionString = connectionString;
        }


        // LOGIN 
        public DataTable Login(string username, string password)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UserLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // PRINTERS - READ 
        public DataTable GetPrinters()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPrinters", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 1. Change signature to accept int? printerMakeId
        public DataTable SearchPrinters(int? printerMakeId, DateTime? fromDate, DateTime? toDate)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SearchPrinters", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Use DBNull.Value if the nullable parameters are null
                cmd.Parameters.AddWithValue("@PrinterMakeID", (object)printerMakeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
       
    }


        // For Search
        public DataTable GetPrinterMake()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPrinterMake", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        //  INSERT      
        public void InsertPrinter(
        string printerName,
        string folderToMonitor,
        string outputType,
        string fileOutput,
        bool active,
        int printerMakeId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("InsertPrinter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PrinterName", printerName);
                    cmd.Parameters.AddWithValue("@FolderToMonitor", folderToMonitor);
                    cmd.Parameters.AddWithValue("@OutputType", outputType);
                    cmd.Parameters.AddWithValue("@FileOutput", fileOutput);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@PrinterMakeID", printerMakeId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                throw new Exception("Error inserting printer: " + ex.Message);
            }
        }
        // Update Printer
        public void UpdatePrinter(
        int printerId,
        string printerName,
        string folderToMonitor,
        string outputType,
        string fileOutput,
        bool active,
        int printerMakeId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("UpdatePrinter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PrinterID", printerId);
                    cmd.Parameters.AddWithValue("@PrinterName", printerName);
                    cmd.Parameters.AddWithValue("@FolderToMonitor", folderToMonitor);
                    cmd.Parameters.AddWithValue("@OutputType", outputType);
                    cmd.Parameters.AddWithValue("@FileOutput", fileOutput);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@PrinterMakeID", printerMakeId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                throw new Exception("Error updating printer: " + ex.Message);
            }
        }

        //DELETE
        public void DeletePrinter(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("DeletePrinter", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EngenPrintersID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetUsersWithDesignation(int? designationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetUsersWithDesignation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"DesignationID", (designationId == 0 || designationId == null) ? (object)DBNull.Value : designationId);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        // Users DAL


        // GET USERS
        public DataTable GetUsers()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("GetUsers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        // INSERT USER
        public void InsertUser(string firstName, string lastName, string email,
                               string username, string password, int designationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@DesignationID", designationId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        // UPDATE USER
        public void EditUser(int userId, string firstName, string lastName,
                             string email, string username, string password,
                             int designationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@DesignationID", designationId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        // DELETE USER
        public void DeleteUser(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
   

            public DataTable GetDesignations()
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "GetDesignations", con))
                {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            }
        public DataTable SearchUsersByDesignation(int? designationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SearchUsersByDesignation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DesignationID", designationId ?? (object)DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Check in the database if user exist or not
        public bool UsernameExists(string username, int? userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CheckUsernameExists", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", username);

                if (userId.HasValue)
                    cmd.Parameters.AddWithValue("@UserID", userId.Value);
                else
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

    }
}












