using Microsoft.AspNetCore.Mvc;
using Farm_Central.Models;

namespace Farm_Central.Controllers
{
    public class ProductController : Controller
    {
        bool Test_Mode = false;
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        

        // Add_product-page: farmers add and view thier products
        public IActionResult Product_Add()
        {
            var userRole = HttpContext.Request.Cookies["UserRole"];
            if (userRole != "Farmer")
            {
                return RedirectToAction("Login", "User", new { area = "" });
            }
            var userID = HttpContext.Request.Cookies["UserID"];

            var allProducts = _productService.GetAllProducts();
            var productsList = allProducts
                .Where(x => x.UserId == Convert.ToInt32(userID))
                .ToList();

            return View(productsList);
        }



        // Add_product-submit: farmers submit newly added product
        [HttpPost]
        public async Task<IActionResult> Product_Add(string productName, int productQty, int price, string productType)
        {
            if (productName != null && productQty > 0 && price > 0 && productType != null)
            {
                // check email is available
                var userID = HttpContext.Request.Cookies["UserID"];
                var userRole = HttpContext.Request.Cookies["UserRole"];
                if (userRole == "Farmer")
                {
                    // submit data
                    var productObj = new Product()
                    {
                        UserId = Convert.ToInt32(userID),
                        ProductName = productName,
                        ProductQty = productQty,
                        Price = price,
                        ProductType = productType,
                    };
                    _productService.CreateProducts(productObj);

                    TempData["msg"] = "Product Added!";
                    return RedirectToAction("Product_Add");
                }
                else
                {
                    TempData["msg"] = "Permission Denied!";
                    return RedirectToAction("Product_Add");
                }
            }
            TempData["msg"] = "All fields required!";
            return RedirectToAction("Product_Add");
        }



        // Delete Product: a farmer deletes product item
        public IActionResult Product_Delete(int Id)
        {
            _productService.DeleteProduct(Id);

            TempData["msg"] = "Product Deleted!";
            return RedirectToAction("Product_Add");
        }



        // Products-page: employees view a specific farmer's products
        public IActionResult Product_List(int Id)
        {
            if (Test_Mode) { Id = 2; }
            else
            {
                var userRole = HttpContext.Request.Cookies["UserRole"];
                if (userRole != "Employee")
                {
                    return RedirectToAction("Login", "User", new { area = "" });
                }
                // store farmer_id as cookie
                HttpContext.Response.Cookies.Append("FarmerID", Id.ToString());
            }
            // get farmer's products
            var allProducts = _productService.GetAllProducts();
            var productsList = allProducts
                .Where(x => x.UserId == Id)
                .ToList();

            return View(productsList);
        }



        // Products-filter: employees can filter a specific farmer's products
        [HttpPost]
        public IActionResult Product_List(string productType, string startDate, string endDate)
        {
            // get farmer-id
            var FarmerID = HttpContext.Request.Cookies["FarmerID"];
            var allProducts = _productService.GetAllProducts();

            if (productType == "All")
            {
                var productsList = allProducts
                    .Where(x => x.UserId == Convert.ToInt32(FarmerID))
                    .ToList();
                return View(productsList);
            }
            //filter by product-type
            else if (productType != null)
            {
                var productsList = allProducts
                .Where(x => x.ProductType == productType && x.UserId == Convert.ToInt32(FarmerID))
                .ToList();

                return View(productsList);
            }
            //filter by date_created
            var productsList2 = allProducts
            .Where(x => x.CreatedDate >= Convert.ToDateTime(startDate) && x.CreatedDate <= Convert.ToDateTime(endDate) && x.UserId == Convert.ToInt32(FarmerID))
            .ToList();

            return View(productsList2);
        }
    }
}