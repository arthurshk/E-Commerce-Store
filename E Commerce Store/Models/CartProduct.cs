namespace E_Commerce_Store.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice()
        {
            return Price * Quantity;
        }
    }
}
