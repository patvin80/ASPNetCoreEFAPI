using LinearAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinearAPI.Service
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Employees { get; }
        Employee this[int id] { get; }
        Employee AddEmployee(Employee emp);
        Employee UpdateEmployee(Employee emp);
        void DeleteEmployee(int id);
    }
}
