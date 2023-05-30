using Farm_Central.Models;
using Farm_Central.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UsersControllerTests
    {
        // Arrange
        private readonly Mock<IUserService> userServiceMock;
        private readonly UserController controller;
        public UsersControllerTests()
        {
            userServiceMock = new Mock<IUserService>();
            controller = new UserController(userServiceMock.Object);
        }


        // LOGOUT FUNCTION
        [TestMethod]
        public void Logout_ReturnsRedirectToLogin()
        {
            var httpContextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            var cookiesMock = new Mock<IResponseCookies>();

            //->mock cookie removal
            httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);
            responseMock.SetupGet(r => r.Cookies).Returns(cookiesMock.Object);
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            // Act
            IActionResult result = controller.Logout();

            // Assert
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Login", redirectResult.ActionName);
        }



        // VIEW REGISTER PAGE
        [TestMethod]
        public void Register_ReturnsViewResult()
        {
            IActionResult result = controller.Register();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }



        // SUBMIT REGISTER EMPLOYEE
        [TestMethod]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            //->mock user data
            var user = new User
            {
                Name = "Test",
                Surname = "Doe",
                Email = "test@example.com",
                Password = "testing@1",
                Role = "Employee"
            };
            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "existinguser@example.com"
                }
            };

            //->mock TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(existingUsers);
            userServiceMock.Setup(u => u.CreateUsers(It.IsAny<User>()));
            var result = await controller.Register(user) as ViewResult;

            // Assert
            tempDataMock.VerifySet(t => t["msg"] = "Registration success!");
        }



        // SUBMIT USER LOGIN
        [TestMethod]
        public async Task Login_ValidUser_ReturnsRedirect()
        {
            //->mock user data
            var user = new User
            {
                Email = "john@example.com",
                Password = "testing@1"
            };
            var allUsers = new List<User>
            {
                new User
                {
                    Name = "John",
                    Surname = "Doe",
                    Email = "john@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("testing@1"),
                    Role = "Employee"
                }
            };

            //->mock TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(allUsers);
            var result = await controller.Login(user) as ViewResult;

            // Assert
            tempDataMock.VerifySet(t => t["msg"] = "Welcome, " + allUsers[0].Name);
        }



        // SUBMIT ADD_FARMER
        [TestMethod]
        public async Task Farmer_Add_ValidFarmer_ReturnsOkResult()
        {
            //->mock user data
            var user = new User
            {
                Name = "Test",
                Surname = "Doe",
                Email = "test@example.com",
                Password = "testing@1",
                Role = "Employee"
            };
            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "existinguser@example.com"
                }
            };

            //->mock TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            //->mock cookies
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.Request.Cookies["UserRole"]).Returns("Employee");
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            // Act
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(existingUsers);
            userServiceMock.Setup(u => u.CreateUsers(It.IsAny<User>()));
            var result = await controller.Farmer_Add(user) as ViewResult;

            // Assert
            tempDataMock.VerifySet(t => t["msg"] = "Farmer added!");
        }



        // VIEW LOGIN PAGE
        [TestMethod]
        public void Login_ReturnsViewResult()
        {
            IActionResult result = controller.Login();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }



        // VIEW ADD_FARMERS PAGE
        [TestMethod]
        public void Farmer_Add_ReturnsViewResult()
        {
            IActionResult result = controller.Farmer_Add();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }



        // VIEW LIST_OF_FARMERS PAGE
        [TestMethod]
        public void Farmer_List_ReturnsViewWithProductsList()
        {
            //->mock data
            var users = new List<User> {
                new User{Id = 1,Name = "John",Surname = "Doe",Email = "john@gmail.com",Password = "testing@1",Role = "Employee"},
                new User{Id = 2,Name = "Jane",Surname = "Doe",Email = "jane@gmail.com",Password = "testing@1",Role = "Farmer"}
            };

            // Arrange
            var expectedUserId = "2";
            userServiceMock.Setup(x => x.GetAllUsers()).Returns(users);

            // Act
            IActionResult redirect = controller.Farmer_List();
            var result = controller.Farmer_List() as ViewResult;
            var model = result?.Model as List<User>;

            // Assert
            Assert.IsNotNull(redirect);
            Assert.IsInstanceOfType(redirect, typeof(ViewResult));
            Assert.IsTrue(model.All(u => u.Id == Convert.ToInt32(expectedUserId)));
        }
    }
}