using System.ComponentModel.DataAnnotations;

namespace Farm_Central.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ProductName { get; set; }

        public int ProductQty { get; set; }

        public int Price { get; set; }

        public string ProductType { get; set; }

        public DateTime CreatedDate { get; set; }

        public Product()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
    }
}
