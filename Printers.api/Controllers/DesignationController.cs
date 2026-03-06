using CompanyPrinters.BLL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Printers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")]
    public class DesignationController : ControllerBase
    {
        private readonly DesignationBLL _designationBll;

        public DesignationController(IConfiguration configuration)
        {
            // Get the string from appsettings.json
            string connString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Initialize the BLL
            _designationBll = new DesignationBLL(connString);
        }

        // GET: api/Designation/get
        [HttpGet("get")]
        public IActionResult GetDesignations()
        {
            DataTable dt = _designationBll.GetDesignations();
            return Ok(ConvertDataTableToList(dt));
        }

        // POST: api/Designation/insert
        [HttpPost("insert")]
        public IActionResult AddDesignation([FromBody] DesignationRequest request)
        {
            try
            {
                _designationBll.AddDesignation(request.DesignationName);
                return Ok(new { message = "Designation added successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An internal error occurred." });
            }
        }

        // PUT: api/Designation/update/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateDesignation(int id, [FromBody] DesignationRequest request)
        {
            try
            {
                _designationBll.UpdateDesignation(id, request.DesignationName);
                return Ok(new { message = "Designation updated successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Designation/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDesignation(int id)
        {
            try
            {
                _designationBll.RemoveDesignation(id);
                return Ok(new { message = "Designation deleted successfully." });
            }
            catch (ApplicationException ex)
            {
                // This will catch the "Cannot delete... assigned to users" error from your BLL
                return BadRequest(new { message = ex.Message });
            }
        }

        // Helper Method to convert DataTable to a JSON-friendly List
        [NonAction]
        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                list.Add(dict);
            }
            return list;
        }
    }

    // DTO for incoming Designation data
    public class DesignationRequest
    {
        public string DesignationName { get; set; } = string.Empty;
    }
}
