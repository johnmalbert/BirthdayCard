using Microsoft.EntityFrameworkCore;
namespace Birthday_Card.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Message> Messages { get; set; }
    }
}