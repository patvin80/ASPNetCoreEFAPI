using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using LinearAPI.Service;
using LinearAPI.Models;
using LinearAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Assert = Xunit.Assert;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LinearAPITests
{

    public class EmployeeServiceTests
    {

        private EmployeeContext CreateDbContext()
        {
            //TODO: Move the Connection string to the Config file
            var options = new DbContextOptionsBuilder<EmployeeContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EmployeesDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dbContext = new EmployeeContext(options.Options);
            return dbContext;
        }
        [Fact]
        public void Test_OneEmployee()
        {
            using (var context = CreateDbContext())
            {
                // Arrange
                var empRepo = new EmployeeRepository(context);

                // Act
                Employee empResult = empRepo[1];

                // Assert
                Assert.NotNull(empResult);
                Assert.Contains("1", empResult.FirstName);
            }

        }

        [Fact]
        public void Test_EmployeeUniqueConstraint()
        {
            using (var context = CreateDbContext())
            {
                // Arrange
                var empRepo = new EmployeeRepository(context);
                var duplicateEmp = new Employee()
                {
                    FirstName = "Carson1",
                    LastName = "Charles",
                    Email = "Carson1@gmail.com",
                    Age = 29
                };

                // Act and Assert
                var ex = Assert.Throws<DbUpdateException>(() => empRepo.AddEmployee(duplicateEmp));
            }

        }


    }
}
