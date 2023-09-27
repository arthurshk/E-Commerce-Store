namespace E_Commerce_Store.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new HashSet<CartProduct>();
        }
        public int Id { get; set; }
        public string Uid { get; set; }
        public virtual ICollection<CartProduct> Products { get; set; }  
   
        public decimal TotalProductsPrice()
        {
            return Products.Sum(p => p.TotalPrice());
        }
        public decimal ShippingPrice()
        {
            return TotalProductsPrice() * 0.05M;
        }
        public decimal TotalAmount()
        {
            return TotalProductsPrice() + ShippingPrice();
        }
    }
}
