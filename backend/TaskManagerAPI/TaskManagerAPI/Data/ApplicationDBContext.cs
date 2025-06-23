using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<Users>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<TaskItems> TaskItems { get; set; } = null!;
        public override DbSet<Users> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            List<IdentityRole> role = new List<IdentityRole>
            {
                new IdentityRole {Id = "7037e82f-2b9f-43f0-8ec9-96742f257f7d" , Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole {Id = "b9354b65-1140-4a51-949f-f943cb5d4baa", Name = "User", NormalizedName = "USER" }
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);

            // Konfiguracja relacji 1:N
            modelBuilder.Entity<TaskItems>()
                .HasOne(t => t.User)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
