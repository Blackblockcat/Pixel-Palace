using Microsoft.EntityFrameworkCore;

namespace Pixel_Palace.Models
{
    public class PixelEntity : DbContext
    {

        public PixelEntity():base()
        {

        }


        public PixelEntity(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Library> libraries { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Games> games { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentItems> PaymentItems { get; set; }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await categories.ToListAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=PIXELBASE;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder); 
        }


    }
}
