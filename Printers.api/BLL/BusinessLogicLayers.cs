using CompanyPrinters.DAL;
using System.Data;

namespace CompanyPrinters.BLL
{
    public class BusinessLogicLayers
    {
        private readonly DALclass _dal;

        // Constructor receives connection string and passes it to DAL
        public BusinessLogicLayers(string connectionString)
        {
            _dal = new DALclass(connectionString);
        }

        public DataTable Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            return _dal.Login(username.Trim(), password);
        }

        // --- Printer Business Logic ---

        public void InsertPrinterBusiness(string name, string folder, string type, string output, bool active, int makeId)
        {
            ValidatePrinter(name, folder, type, output, makeId);
            _dal.InsertPrinter(name.Trim(), folder.Trim(), type.Trim().ToUpper(), output.Trim(), active, makeId);
        }

        public void UpdatePrinterBusiness(int id, string name, string folder, string type, string output, bool active, int makeId)
        {
            if (id <= 0) throw new ApplicationException("Invalid printer ID.");
            ValidatePrinter(name, folder, type, output, makeId);
            _dal.UpdatePrinter(id, name.Trim(), folder.Trim(), type.Trim().ToUpper(), output.Trim(), active, makeId);
        }

        private void ValidatePrinter(string name, string folder, string type, string output, int makeId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ApplicationException("Printer Name is required.");
            if (string.IsNullOrWhiteSpace(folder)) throw new ApplicationException("Folder is required.");
            if (string.IsNullOrWhiteSpace(type)) throw new ApplicationException("Output Type is required.");

            string[] allowedTypes = { "FTP OUTPUT", "FILE OUTPUT", "PDF" };
            if (!allowedTypes.Contains(type.Trim().ToUpper()))
                throw new ApplicationException("Invalid output type selected.");

            if (makeId <= 0) throw new ApplicationException("Invalid printer make.");
        }

        // --- Nested BLL Classes (Updated with Connection String) ---

        public class PrinterBLL
        {
            private readonly DALclass _dal;
            public PrinterBLL(string connectionString) { _dal = new DALclass(connectionString); }

            public DataTable GetPrinters() => _dal.GetPrinters();

            public DataTable SearchPrinters(int? makeId, DateTime? from, DateTime? to)
            {
                if (to.HasValue) to = to.Value.Date.AddDays(1).AddSeconds(-1);
                return _dal.SearchPrinters(makeId, from, to);
            }
        }

        public class UserBLL
        {
            private readonly DALclass _dal;
            public UserBLL(string connectionString) { _dal = new DALclass(connectionString); }

            public DataTable GetUsers() => _dal.GetUsers();

            public void InsertUser(string fName, string lName, string email, string user, string pass, int desId)
            {
                // You could add ValidateUser call here
                _dal.InsertUser(fName, lName, email, user, pass, desId);
            }

            public void DeleteUser(int id) => _dal.DeleteUser(id);
        }
    }
}





// Users BLL
public class UserBLL
{
    private readonly DALclass _dal;

    // Fix: The constructor now takes the connection string from the parent BLL
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
        // You can add validation here before calling DAL
        _dal.InsertUser(firstName, lastName, email, username, password, designationId);
    }

    public void EditUser(int userId, string firstName, string lastName,
                         string email, string username, string password,
                         int designationId)
    {
        if (userId <= 0) throw new Exception("Invalid User ID");
        _dal.EditUser(userId, firstName, lastName, email, username, password, designationId);
    }

    public void DeleteUser(int userId)
    {
        _dal.DeleteUser(userId);
    }
}









