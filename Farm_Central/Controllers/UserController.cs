using Microsoft.AspNetCore.Mvc;
using Farm_Central.Models;

namespace Farm_Central.Controllers
{
    public class UserController : Controller
    {
        bool Test_Mode = false;
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // Logout
        public IActionResult Logout()
        {
            // clear all cookies
            HttpContext.Response.Cookies.Delete("UserID");
            HttpContext.Response.Cookies.Delete("UserRole");
            HttpContext.Response.Cookies.Delete("UserName");
            return RedirectToAction("Login");
        }


        // Route->Register_page
        public IActionResult Register()
        {
            return View();
        }


        // Register-submit: only employees can register
        [HttpPost]
        public async Task<IActionResult> Register(Models.User user)
        {
            try
            {
                if (user.Name != null && user.Surname != null && user.Email != null && user.Password != null && user.Role != null)
                {
                    // check email is available
                    var allUsers = _userService.GetAllUsers();
                    var isEmailFree = allUsers.FirstOrDefault(x => x.Email == user.Email);
                    if (isEmailFree == null)
                    {
                        // submit data
                        var userObj = new Models.User()
                        {
                            Name = user.Name,
                            Surname = user.Surname,
                            Email = user.Email,
                            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                            Role = user.Role,
                        };
                        _userService.CreateUsers(userObj);

                        TempData["msg"] = "Registration success!";
                        return RedirectToAction("Login");
                    }
                    else {
                        TempData["msg"] = "Email already in use!";
                        return RedirectToAction("Register");
                    }
                }
                TempData["msg"] = "All fields required!";
                return RedirectToAction("Register");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Failed: " + e.Message;
                return RedirectToAction("Register");
            }
        }


        // Route->Login_page
        public IActionResult Login()
        {
            return View();
        }


        // Login-submit: employee and farmers can login
        [HttpPost]
        public async Task<IActionResult> Login(Models.User user)
        {
            try
            {
                if (user.Email != null && user.Password != null)
                {
                    var allUsers = _userService.GetAllUsers();
                    foreach (var i in allUsers)
                    {
                        //verify email match
                        if (i.Email == user.Email)
                        {
                            //verify password match
                            bool isVerified = BCrypt.Net.BCrypt.Verify(user.Password, i.Password);
                            if (isVerified)
                            {
                                if (!Test_Mode)
                                {
                                    // create cookies
                                    HttpContext.Response.Cookies.Append("UserID", i.Id.ToString());
                                    HttpContext.Response.Cookies.Append("UserRole", i.Role.ToString());
                                    HttpContext.Response.Cookies.Append("UserName", i.Name.ToString());
                                }

                                // redirect
                                if (i.Role == "Employee")
                                {
                                    // redirect_employee
                                    TempData["msg"] = "Welcome, "+i.Name;
                                    return RedirectToAction("Farmer_List");
                                }
                                else if (i.Role == "Farmer")
                                {
                                    // redirect_farmer
                                    TempData["msg"] = "Welcome, " + i.Name;
                                    return RedirectToAction("Product_Add", "Product", new { area = "" });
                                }
                            }
                            break;
                        }
                    }
                    TempData["msg"] = "Access Denied! Wrong input!";
                    return RedirectToAction("Login");
                }
                TempData["msg"] = "All fields required!";
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Access Denied!" +e.Message;
                return RedirectToAction("Login");
            }
        }


        // Route->Add_farmer_page
        public IActionResult Farmer_Add()
        {
            return View();
        }


        // Add_farmer-submit: employee can add new farmers
        [HttpPost]
        public async Task<IActionResult> Farmer_Add(Models.User user)
        {
            try
            {
                if (user.Name != null && user.Surname != null && user.Email != null && user.Role != null)
                    {
                    var userRole = HttpContext.Request.Cookies["UserRole"];
                    if (userRole == "Employee")
                    {
                        // check email is available
                        var allUsers = _userService.GetAllUsers();
                        var isEmailFree = allUsers.FirstOrDefault(x => x.Email == user.Email);
                        if (isEmailFree == null)
                        {
                            // submit data
                            var userObj = new Models.User()
                            {
                                Name = user.Name,
                                Surname = user.Surname,
                                Email = user.Email,
                                Password = BCrypt.Net.BCrypt.HashPassword("testing@1"),
                                Role = user.Role,
                            };
                            _userService.CreateUsers(userObj);

                            TempData["msg"] = "Farmer added!";
                            return RedirectToAction("Farmer_List");
                        }
                        else
                        {
                            TempData["msg"] = "Email already in use!";
                            return RedirectToAction("Farmer_Add");
                        }
                    }
                    else
                    {
                        TempData["msg"] = "Permission Denied!";
                        return RedirectToAction("Farmer_Add");
                    }
                }
                TempData["msg"] = "All fields required!";
                return RedirectToAction("Farmer_Add");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Failed: " + e.Message;
                return RedirectToAction("Register");
            }
        }


        // Route->Farmers_page: employees can view list of farmers
        public IActionResult Farmer_List()
        {
            if (!Test_Mode)
            {
                var userRole = HttpContext.Request.Cookies["UserRole"];
                if (userRole != "Employee")
                {
                    return RedirectToAction("Login");
                }
            }
            var allUsers = _userService.GetAllUsers();
            var farmersList = allUsers
            .Where(x => x.Role == "Farmer")
            .ToList();

            return View(farmersList);
        }
    }
}
