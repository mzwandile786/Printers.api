using Company_Printers.api.Models; // Ensure this matches your namespace
using CompanyPrinters.BLL;
using CompanyPrinters.DAL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Printers.api.Models;
using System.Data;

namespace Printers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")]
    public class PrinterController : ControllerBase
    {
        private readonly string DefaultConnection;
        private readonly DALclass _dal;
        private readonly BusinessLogicLayers _bll;

        public PrinterController(IConfiguration configuration)
        {
            // 1. Assign value to the class field so it's not null anymore
            DefaultConnection = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // 2. Pass that initialized string to your BLL and DAL
            _bll = new BusinessLogicLayers(DefaultConnection);
            _dal = new DALclass(DefaultConnection);
        }

        [HttpGet("search")]
        public IActionResult SearchPrinters(int? printerMakeId, DateTime? fromDate, DateTime? toDate)
        {
            // FIX: Use the _bll instance created in the constructor
            // No need to 'new up' a BLL inside the method
            DataTable dt = _dal.SearchPrinters(printerMakeId, fromDate, toDate);

            return Ok(ConvertDataTableToList(dt));
        }

    // ... GetPrinters, CreatePrinter, and UpdatePrinter methods ...


        // GET: api/Printer/get
        [HttpGet("get")]
        public IActionResult GetPrinters()
        {
            DataTable dt = _dal.GetPrinters();
            return Ok(ConvertDataTableToList(dt));
        }

        [HttpGet("printer-makes")]
        public IActionResult GetPrinterMakes()
        {
            var dt = _dal.GetPrinterMake();

            var list = dt.AsEnumerable()
                         .Select(row => new PrinterMake
                         {
                             PrinterMakeID = row.Field<int>("PrinterMakeID"),
                             PrinterMakeName = row.Field<string>("PrinterMakeName")
                         })
                         .ToList();

            return Ok(list);
        }




        // POST: api/Printer/insert
        [HttpPost("insert")]
        public IActionResult CreatePrinter([FromBody] Printer p)
        {
            try
            {
                // Using BLL for the validation logic (Required fields, OutputType check)
                _bll.InsertPrinterBusiness(
                    p.PrinterName, p.FolderToMonitor, p.OutputType,
                    p.FileOutput, p.Active, p.PrinterMakeID
                );
                return Ok(new { message = "Printer created successfully via BLL" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Printer/update/5
        [HttpPut("update/{id}")]
        public IActionResult UpdatePrinter(int id, [FromBody] Printer p)
        {
            try
            {
                _bll.UpdatePrinterBusiness(
                    id, p.PrinterName, p.FolderToMonitor, p.OutputType,
                    p.FileOutput, p.Active, p.PrinterMakeID
                );
                return Ok(new { message = "Printer updated successfully via BLL" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Printer/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeletePrinter(int id)
        {
            _dal.DeletePrinter(id);
            return Ok(new { message = "Printer deleted successfully via DAL" });
        }




























        // Helper Method to convert DataTable to a JSON-friendly List
        private List<object> ConvertDataTableToList(DataTable dt)
        {
            var list = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }
    }
}