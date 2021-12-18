using LinearAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinearAPI.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }
        public Employee this[int id] => _context.Employees.FirstOrDefault(m => m.Id == id);

        public IEnumerable<Employee> Employees => _context.Employees.ToList<Employee>();

        public Employee AddEmployee(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return emp;
        }

        public void DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public Employee UpdateEmployee(Employee emp)
        {
            try
            {
                _context.Update(emp);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(emp.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return emp;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
