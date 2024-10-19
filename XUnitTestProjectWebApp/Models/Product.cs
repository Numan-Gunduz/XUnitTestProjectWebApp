namespace XUnitTestProjectWebApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }

        public double? ProductPrice { get; set; }

        public int? ProductStock { get; set; }

        public string? ProductColor { get; set; }
        public int  CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
