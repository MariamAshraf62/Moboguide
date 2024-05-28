using Microsoft.EntityFrameworkCore;

namespace Moboguide.Models
{
    public class Pcontext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MoboGuide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;MultipleActiveResultSets=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite Key
            modelBuilder.Entity<MobileUser>()
                .HasKey(e => new { e.MobileName, e.UserId});
        }
        public DbSet<Mobile> mobiles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<MobileUser> mobilesUsers { get; set; }

    }
}
