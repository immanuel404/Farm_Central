namespace Farm_Central.Models
{
    public class ProductRepository : IProductService
    {
        private readonly DBContext _dbContext;
        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // READ PRODUCTS
        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products;
        }

        // CREATE PRODUCT
        public void CreateProducts(Product product)
        {
             _dbContext.Products.Add(product);
             _dbContext.SaveChanges();
        }

        // DELETE PRODUCT
        public void DeleteProduct(int id)
        {
            // Retrieve item
            var item = _dbContext.Products.Find(id);

            if (item == null)
            {
                Console.WriteLine("ERROR: ITEM NOT FOUND!");
            }
            // Remove the item
            _dbContext.Products.Remove(item);
            // Save the changes
            _dbContext.SaveChanges();
        }
    }

    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        void CreateProducts(Product product);
        void DeleteProduct(int id);
    }
}