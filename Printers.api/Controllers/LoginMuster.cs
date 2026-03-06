using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Printers.api.Models;      // Your models namespace
using Printers.api;             // Your DbContext namespace

namespace Printers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginMusterController : ControllerBase
    {
        private readonly PrintersDbContext _context;

        public LoginMusterController(PrintersDbContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserLoginResult>> Login(UserLoginRequest request)
        {
            var userNameParam = new SqlParameter("@UserName", request.UserName);
            var passwordParam = new SqlParameter("@Password", request.Password);

            // Call the stored procedure and materialize on client side
            var user = _context.UserLoginResults
                .FromSqlRaw("EXEC [dbo].[UserLogin] @UserName, @Password", userNameParam, passwordParam)
                .AsNoTracking()      // keep tracking off
                .AsEnumerable()      // materialize the results locally
                .FirstOrDefault();   // now safe to use LINQ

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            return Ok(user);
        }
    }
}
