using CompanyPrinters.DAL;
using System.Data;
using System.Text.RegularExpressions;

namespace CompanyPrinters.BLL
{
    public class UserBLL
    {
        private readonly DALclass _dal;

        // FIX 1: Constructor must accept the connection string from the Controller
        public UserBLL(string connectionString)
        {
            _dal = new DALclass(connectionString);
        }

        public DataTable GetUsers()
        {
            return _dal.GetUsers();
        }


        public void InsertUser(string firstName, string lastName, string email,
                               string username, string password, int designationId)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.");

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");

            if (!IsValidPassword(password))
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            _dal.InsertUser(
                firstName.Trim(),
                lastName.Trim(),
                email.Trim(),
                username.Trim(),
                password,
                designationId
            );
        }

        public void EditUser(int userId, string firstName, string lastName,
                             string email, string username, string password,
                             int designationId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User ID");
            _dal.EditUser(userId, firstName, lastName, email, username, password, designationId);
        }

        public void DeleteUser(int userId)
        {
            _dal.DeleteUser(userId);
        }

        public DataTable GetUsersByDesignation(int? designationId)
        {
            DataTable dt = GetUsers();

            if (designationId.HasValue && designationId.Value > 0)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "DesignationID = " + designationId.Value;
                return dv.ToTable();
            }

            return dt;
        }

        public bool UsernameExists(string username, int? userId)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            return _dal.UsernameExists(username.Trim(), userId);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Minimum 8 chars, 1 uppercase, 1 lowercase, 1 number, 1 special char
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

    }
}