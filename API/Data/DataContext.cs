using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MarkItem> MarkItems { get; set; }
        
    }
}