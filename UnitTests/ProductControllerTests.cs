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
    public class ProductControllerTests
    {
        // Arrange
        private readonly Mock<IProductService> productServiceMock;
        private readonly ProductController controller;
        public ProductControllerTests()
        {
            productServiceMock = new Mock<IProductService>();
            controller = new ProductController(productServiceMock.Object);
        }


        // VIEW PRODUCT_ADD PAGE
        [TestMethod]
        public void Product_Add_ReturnsViewWithProductsList()
        {
            // ->mock data
            var products = new List<Product> {
                new Product{ Id = 1,UserId = 2,ProductName = "Banana",ProductQty = 50,Price=55,ProductType = "Fruit"},
                new Product{ Id = 2,UserId = 2,ProductName = "Pears",ProductQty = 85,Price=55,ProductType = "Fruit"},
                new Product{ Id = 3,UserId = 2,ProductName = "Wheat",ProductQty = 20,Price=55,ProductType = "Grain"},
                new Product{ Id = 4,UserId = 2,ProductName = "Apples",ProductQty = 50,Price=55,ProductType = "Fruit"},
                new Product{ Id = 5,UserId = 3,ProductName = "Spinach",ProductQty = 35,Price=55,ProductType = "Vegetable"}
            };

            // ->mock cookie
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.Request.Cookies["UserID"]).Returns("2");
            httpContextMock.SetupGet(c => c.Request.Cookies["UserRole"]).Returns("Farmer");
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            // Arrange
            var expectedUserId = "2";
            productServiceMock.Setup(x => x.GetAllProducts()).Returns(products);

            // Act
            IActionResult redirect = controller.Product_Add();
            var result = controller.Product_Add() as ViewResult;
            var model = result?.Model as List<Product>;

            // Assert
            Assert.IsNotNull(redirect);
            Assert.IsInstanceOfType(redirect, typeof(ViewResult));
            Assert.IsTrue(model.All(p => p.UserId == Convert.ToInt32(expectedUserId)));
        }



        // SUBMIT ADDED PRODUCT
        [TestMethod]
        public async Task Product_Add_ValidData_RedirectsToProductAdd()
        {
            // ->mock cookies
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.Request.Cookies["UserID"]).Returns("2");
            httpContextMock.SetupGet(c => c.Request.Cookies["UserRole"]).Returns("Farmer");
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            // ->mock TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            productServiceMock.Setup(p => p.CreateProducts(It.IsAny<Product>()));
            var result = await controller.Product_Add("Test Product", 10, 100, "Test Type") as ViewResult;

            // Assert
            tempDataMock.VerifySet(t => t["msg"] = "Product Added!");
        }



        // DELETE PRODUCT
        [TestMethod]
        public void Product_Delete_DeletesProduct_RedirectsToProductAdd()
        {
            // ->mock TempData
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            IActionResult result = controller.Product_Delete(1);

            // Assert
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Product_Add", redirectResult.ActionName);
        }



        // VIEW PRODUCT_LIST PAGE
        [TestMethod]
        public void Product_List_ReturnsViewWithProductsList()
        {
            // ->mock data
            var products = new List<Product> {
                new Product{ Id = 1,UserId = 2,ProductName = "Banana",ProductQty = 50,Price=55,ProductType = "Fruit"},
                new Product{ Id = 2,UserId = 2,ProductName = "Pears",ProductQty = 85,Price=55,ProductType = "Fruit"},
                new Product{ Id = 3,UserId = 2,ProductName = "Wheat",ProductQty = 20,Price=55,ProductType = "Grain"},
                new Product{ Id = 4,UserId = 2,ProductName = "Apples",ProductQty = 50,Price=55,ProductType = "Fruit"},
                new Product{ Id = 5,UserId = 3,ProductName = "Spinach",ProductQty = 35,Price=55,ProductType = "Vegetable"}
            };

            // ->mock cookie
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.Request.Cookies["FarmerID"]).Returns("2");
            controller.ControllerContext.HttpContext = httpContextMock.Object;

            // Arrange
            var expectedUserId = "2";
            var expectedType = "Fruit";
            productServiceMock.Setup(x => x.GetAllProducts()).Returns(products);

            // Act-> Product_List VIEW
            IActionResult redirect = controller.Product_List(2);
            var result = controller.Product_List(2) as ViewResult;
            var model = result?.Model as List<Product>;
            // Assert
            Assert.IsNotNull(redirect);
            Assert.IsInstanceOfType(redirect, typeof(ViewResult));
            Assert.IsTrue(model.All(p => p.UserId == Convert.ToInt32(expectedUserId)));

            // Act-> Product_List FILTER
            IActionResult redirect2 = controller.Product_List("Fruit", "01/05/2023", "30/05/2023");
            var result2 = controller.Product_List("Fruit", "01/05/2023", "30/05/2023") as ViewResult;
            var model2 = result?.Model as List<Product>;
            // Assert
            Assert.IsNotNull(redirect2);
            Assert.IsInstanceOfType(redirect2, typeof(ViewResult));
            Assert.IsTrue(model2.All(p => p.UserId == Convert.ToInt32(expectedUserId)));
            Assert.IsTrue(model2.All(p => p.ProductType == expectedType));
        }
    }
}