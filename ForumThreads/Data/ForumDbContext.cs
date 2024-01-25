using Microsoft.EntityFrameworkCore;

namespace ForumThreads.Data
{
    public class ForumDbContext : DbContext
    {
        public DbSet<ForumThreads.Model.Thread> Threads { get; set; }

        public ForumDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ForumThreads.Model.Thread>();
        }
    }
}