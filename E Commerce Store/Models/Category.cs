using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce_Store.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string? Title { get; set; } 
        public string? Url { get; set; }
        public virtual Image? Image { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
