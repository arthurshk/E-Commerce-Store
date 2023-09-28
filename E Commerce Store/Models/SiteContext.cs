using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace E_Commerce_Store.Models
{
    public class SiteContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public SiteContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartProduct> CartProducts { get; set; } = null!;
        public virtual DbSet<Buyer> Buyers { get; set; } = null!;
        
    }
}
