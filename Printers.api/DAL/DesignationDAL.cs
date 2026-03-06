
using System.Data;
using Microsoft.Data.SqlClient; // Use Microsoft.Data.SqlClient for modern .NET

namespace CompanyPrinters.DAL
    {
        public class DesignationDAL
        {
            private readonly string DefaultConnection;

            // Inject the connection string via constructor
            public DesignationDAL(string connectionString)
            {
                DefaultConnection = connectionString;
            }



            public void InsertDesignation(string designationName)
        {
            using (SqlConnection conn = new SqlConnection(DefaultConnection))
            {
                SqlCommand cmd = new SqlCommand("InsertDesignation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DesignationName", designationName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDesignations()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(DefaultConnection))
            {
                SqlCommand cmd = new SqlCommand("GetDesignations", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public void DeleteDesignation(int designationId)
        {
            using (SqlConnection conn = new SqlConnection(DefaultConnection))
            using (SqlCommand cmd = new SqlCommand("DeleteDesignation", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DesignationID", SqlDbType.Int).Value = designationId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDesignation(int designationId, string designationName)
        {
            using (SqlConnection conn = new SqlConnection(DefaultConnection))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateDesignation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DesignationID", designationId);
                cmd.Parameters.AddWithValue("@DesignationName", designationName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
