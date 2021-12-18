using LinearAPI.Models;
using LinearAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private IEmployeeRepository _repository;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeRepository repo, ILogger<EmployeeController> logger)
        {
            _repository = repo;
            _logger = logger;
        }

        // GET: api/<EmployeeController>
        /// <summary>
        /// Retrieves all employees
        /// </summary>
        /// <response code="200">Employees retrieved</response>
        /// <response code="404">No Employees Found</response>
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            IList<Employee> employees = null;

            employees = _repository.Employees.ToList();


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
        /// <param name="id" example="123">Employee id</param>
        /// <response code="200">Employee retrieved</response>
        /// <response code="404">Employee not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieves a Unique Employee"
        )]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Employee> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Value must be passed in the request body.");
            }

            var employee = _repository[id];
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
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] Employee emp)
        {
            if (emp == null)
            {
                return BadRequest();
            }
            try
            {
                _repository.AddEmployee(emp);
                return CreatedAtRoute(new { id = emp.Id }, emp);
            }
            catch (DbUpdateException dbUpdateEx)
            {
                // TODO: A Better Exception Handling mechanism
                if (dbUpdateEx.InnerException != null && dbUpdateEx.InnerException != null)
                {
                    if (dbUpdateEx.InnerException is SqlException sqlException)
                    {
                        switch (sqlException.Number)
                        {
                            case 2601:  // Unique constraint error
                                return BadRequest("Duplicate Employee Found");
                            default:
                                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        }
                    }
                    else
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                else
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                return BadRequest("Error saving the result");
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing Employee",
            Description = "Note: Employee Name, Email and Age are Unique"
        )]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Employee> Put(int id, [FromBody] Employee emp)
        {

            if (emp == null )
            {
                return BadRequest();
            }

            if (id == 0)
            {
                return NotFound();
            }
            var savedEmp = _repository.UpdateEmployee(emp);
            if (savedEmp == null)
                return NotFound();
            else
                return Ok(savedEmp);
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes an existing Employee"
        )]
        public void Delete(int id) => _repository.DeleteEmployee(id);


    }
}
