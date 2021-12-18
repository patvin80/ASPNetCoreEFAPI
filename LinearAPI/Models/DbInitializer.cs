using LinearAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinearAPI.Models
{
    public static class DbInitializer
    {


        public static void Initialize(EmployeeContext context)
        {

            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Employees.Any())
            {
                return;   // DB has been seeded
            }

            var employees = new Employee[]
            {
            new Employee{FirstName="Carson1",LastName ="Charles", Email="Carson1@gmail.com",Age=29},
            new Employee{FirstName="Carson2",LastName ="Charles", Email="Carson2@gmail.com",Age=29},
            new Employee{FirstName="Carson3",LastName ="Charles", Email="Carson3@gmail.com",Age=29},
            };
            foreach (Employee emp in employees)
            {
                context.Employees.Add(emp);
            }
            context.SaveChanges();
        }
    }
}
