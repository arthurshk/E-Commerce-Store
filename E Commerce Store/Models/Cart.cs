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
    }
}
