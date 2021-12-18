using LinearAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinearAPI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly EmployeeContext _context;
        private readonly ILogger _logger;

        public EmployeesController(EmployeeContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            IList<Employee> employees = null;

            employees = _context.Employees.ToList<Employee>();
            _logger.LogInformation("Log message in the Get All method");

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        // GET api/<EmployeeController>/5

        /// <summary>
        /// Retrieves a specific employee by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id" example="123">The product id</param>
        /// <response code="200">Employee retrieved</response>
        /// <response code="404">Employee not found</response>
        /// <response code="500">Server Error</response>
        [HttpGet("{id}", Name = "GetEmp")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Employee",
            Description = "Employee Name, Email and Age are Unique"
        )]
        public IActionResult Post([FromBody] Employee emp)
        {
            if (emp == null)
            {
                return BadRequest();
            }
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return CreatedAtRoute("GetEmp", new { id = emp.Id }, emp);
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing Employee",
            Description = "Note: Employee Name, Email and Age are Unique"
        )]
        public async Task<IActionResult> Put(int id, [FromBody] Employee emp)
        {

            if (emp == null)
            {
                return BadRequest();
            }

            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                _context.Update(emp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(emp.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return new NoContentResult();
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes an existing Employee"
        )]
        public async void Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
