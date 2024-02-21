namespace Entities
{
    [Serializable]
    public class Product
    {
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double? LastTotalPrice { get; set; }

        public Product()
        {

        }
    }
}
