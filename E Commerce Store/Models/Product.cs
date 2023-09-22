using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Store.Models
{
    public class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
        }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        
        public virtual Image? MainImage { get; set; }
        public virtual ICollection<Image>? Images { get; set; }

    }
}
