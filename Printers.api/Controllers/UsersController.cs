using CompanyPrinters.BLL; // Ensure this matches your BLL namespace
using CompanyPrinters.DAL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAngularApp")]
public class UsersController : ControllerBase
{
    private readonly UserBLL _userBll;
    private readonly DALclass _dal;
    private readonly DesignationBLL _bll;

    public UsersController(IConfiguration configuration)
    {
        // Get the string from appsettings.json
        string connString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        // Initialize the BLL with the connection string
        _userBll = new UserBLL(connString);
        _dal = new DALclass(connString);
        _bll = new DesignationBLL(connString);
    }

    // GET: api/Users/get
    [HttpGet("get")]
    public IActionResult GetAllUsers()
    {
        DataTable dt = _userBll.GetUsers();
        return Ok(ConvertDataTableToList(dt));
    }

    // New Endpoint to fill your dropdown
    [HttpGet("designations")]
    public IActionResult GetDesignations()
    {
        // Assuming DesignationBLL has a GetDesignations method
        DataTable dt = _bll.GetDesignations();
        return Ok(ConvertDataTableToList(dt));
    }

    [HttpGet("search")]
    public IActionResult GetUsersByDesignation([FromQuery] int? designationId)
    {
        // This will now work because _dal is initialized
        DataTable dt = _dal.SearchUsersByDesignation(designationId);
        return Ok(ConvertDataTableToList(dt));
    }

    // GET: api/Users/check-username?username=admin
    [HttpGet("check-username")]
    public IActionResult CheckUsername([FromQuery] string username, [FromQuery] int? userId)
    {
        bool exists = _dal.UsernameExists(username, userId);
        return Ok(new { exists });
    }

    // POST: api/Users/insert
    [HttpPost("insert")]
    public IActionResult InsertUser([FromBody] UserPostModel model)
    {
        if (model == null)
            return BadRequest(new { message = "Invalid request data." });

        // Basic required field checks
        if (string.IsNullOrWhiteSpace(model.FirstName))
            return BadRequest(new { message = "First Name is required." });

        if (string.IsNullOrWhiteSpace(model.LastName))
            return BadRequest(new { message = "Last Name is required." });

        if (string.IsNullOrWhiteSpace(model.Email))
            return BadRequest(new { message = "Email is required." });

        if (string.IsNullOrWhiteSpace(model.UserName))
            return BadRequest(new { message = "Username is required." });

        if (string.IsNullOrWhiteSpace(model.Password))
            return BadRequest(new { message = "Password is required." });

        if (model.DesignationID <= 0)
            return BadRequest(new { message = "Invalid Designation selected." });

        try
        {
            // Call BLL (BLL will handle email/password validation)
            _userBll.InsertUser(
                model.FirstName,
                model.LastName,
                model.Email,
                model.UserName,
                model.Password,
                model.DesignationID
            );

            return Ok(new { message = "User created successfully." });
        }
        catch (ArgumentException ex)
        {
            // Validation errors from BLL (email format, password strength, etc.)
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Any other unexpected errors (like DB issues)
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                detail = ex.Message // Optional: include for debugging
            });
        }
    }




    // PUT: api/Users/update/5
    [HttpPut("update/{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UserPostModel model)
    {
        try
        {
            _userBll.EditUser(
                id,
                model.FirstName,
                model.LastName,
                model.Email,
                model.UserName,
                model.Password,
                model.DesignationID
            );
            return Ok(new { message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/Users/delete/5
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            _userBll.DeleteUser(id);
            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

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

// Simple DTO to handle incoming JSON data
public class UserPostModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int DesignationID { get; set; }
}