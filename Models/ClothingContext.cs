using Microsoft.EntityFrameworkCore;

namespace ClothingApi.Models
{
    public class ClothingContext : DbContext
    {
        public ClothingContext(DbContextOptions<ClothingContext> options) : base(options)
        { }

        public DbSet<ClothingItemDto> ClothingItems { get; set; }
    }
}
