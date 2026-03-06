using System;
using System.Data;
using Microsoft.Data.SqlClient; // Note: Use Microsoft.Data.SqlClient for .NET Core/API
using CompanyPrinters.DAL;

namespace CompanyPrinters.BLL
{
    public class DesignationBLL
    {
        private readonly DALclass _dal;
        private readonly DesignationDAL _dedal;

        // FIX: The constructor accepts the connection string and passes it to BOTH DALs
        public DesignationBLL(string connectionString)
        {
            _dal = new DALclass(connectionString);
            _dedal = new DesignationDAL(connectionString);
        }

        public DataTable GetDesignations()
        {
            return _dal.GetDesignations();
        }

        public DataTable GetUsersByDesignation(int? designationId)
        {
            return _dal.SearchUsersByDesignation(designationId);
        }

        public void AddDesignation(string designationName)
        {
            if (string.IsNullOrWhiteSpace(designationName))
                throw new ApplicationException("Designation Name is required.");

            _dedal.InsertDesignation(designationName.Trim());
        }

        public void RemoveDesignation(int designationId)
        {
            if (designationId <= 0)
                throw new ApplicationException("Invalid designation selected.");

            try
            {
                _dedal.DeleteDesignation(designationId);
            }
            catch (SqlException ex)
            {
                // Error 547 is a Foreign Key constraint violation
                if (ex.Number == 547)
                {
                    throw new ApplicationException(
                        "Cannot delete this designation because it is assigned to users.");
                }
                throw new ApplicationException("Failed to delete designation due to a database error.");
            }
        }

        public void UpdateDesignation(int designationId, string designationName)
        {
            if (designationId <= 0)
                throw new ApplicationException("Invalid ID.");
            if (string.IsNullOrWhiteSpace(designationName))
                throw new ApplicationException("Name is required.");

            _dedal.UpdateDesignation(designationId, designationName.Trim());
        }
    }
}