using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockChat.Domain.Models;

namespace StockChat.Infrastructure.Data
{
    public class StocksChatDbContext : IdentityDbContext
    {
        public DbSet<Message> Messages { get; set; }

        public StocksChatDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
