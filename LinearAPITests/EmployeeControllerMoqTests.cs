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

namespace LinearAPITests
{

    public class EmployeeControllerMoqTests
    {
        private readonly ILogger<EmployeeController> _logger = Mock.Of<ILogger<EmployeeController>>();
        [Fact]
        public void Test_GET_AllEmployees()
        {
            // Arrange
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.Employees).Returns(Multiple());
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(okResult);
            // Making sure the count of result is 3.
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Test_GET_AEmployee_BadRequest()
        {
            // Arrange
            int id = 0;
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((a) => Single(a));
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void Test_GET_AEmployee_Ok()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((a) => Single(a));
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(id, ((Employee)actionValue.Value).Id);
        }

        [Fact]
        public void Test_GET_AEmployee_NotFound()
        {
            // Arrange
            int id = 4;
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((a) => Single(a));
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void Test_PUT_UpdateEmployee()
        {
            // Arrange
            Employee r = new Employee
            {
                Id = 3,
                FirstName = "fname",
                LastName = "lname",
                Age = 33,
                Email = "newEmail@gmai.com"
            };
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.UpdateEmployee(It.IsAny<Employee>())).Returns(r);
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
             var result = controller.Put(r.Id, r);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var emp = ((Employee)actionValue.Value);
            Assert.Equal(3, emp.Id);
            Assert.Equal("fname", emp.FirstName);
            Assert.Equal("lname", emp.LastName);
            Assert.Equal(33, emp.Age);
            Assert.Equal("newEmail@gmai.com", emp.Email);
        }

        [Fact]
        public void Test_DELETE_Employee()
        {
            // Arrange
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.DeleteEmployee(It.IsAny<int>())).Verifiable();
            var controller = new EmployeeController(mockRepo.Object, _logger);

            // Act
            controller.Delete(3);

            // Assert
            mockRepo.Verify();
        }

        #region "Test Helpers"
        private static Employee Single(int id)
        {
            IEnumerable<Employee> employees = Multiple();
            return employees.Where(a => a.Id == id).FirstOrDefault();
        }

        private static IEnumerable<Employee> Multiple()
        {
            var e = new List<Employee>();
            e.Add(new Employee()
            {
                Id = 1,
                FirstName = "FTest One",
                LastName = "LTest One",
                Email = "pvin@hotm.com", 
                Age = 34
            });
            e.Add(new Employee()
            {
                Id = 2,
                FirstName = "Test Two",
                LastName = "LTest Two",
                Email = "pvin@hotm.com",
                Age = 34
            });
            e.Add(new Employee()
            {
                Id = 3,
                FirstName = "Test Three",
                LastName = "LTest Three",
                Email = "pvin@hotm.com",
                Age = 34
            });
            return e;
        }
        #endregion
    }
}
