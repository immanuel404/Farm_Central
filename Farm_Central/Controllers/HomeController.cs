using Microsoft.AspNetCore.Mvc;

namespace Farm_Central.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // verify user
            var userRole = HttpContext.Request.Cookies["UserRole"];
            var userName = HttpContext.Request.Cookies["UserName"];

            if (userRole == "Employee")
            {
                // redirect_employee
                TempData["msg"] = "Welcome, " + userName;
                return RedirectToAction("Farmer_List", "User", new { area = "" });
            }
            else if (userRole == "Farmer")
            {
                // redirect_farmer
                TempData["msg"] = "Welcome, " + userName;
                return RedirectToAction("Product_Add", "Product", new { area = "" });
            }

            // redirect to login page
            return RedirectToAction("Login", "User", new { area = "" });
        }
    }
}
