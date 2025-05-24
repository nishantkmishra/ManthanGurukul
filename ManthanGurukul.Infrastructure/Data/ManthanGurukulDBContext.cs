using ManthanGurukul.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManthanGurukul.Infrastructure.Data
{
    public class ManthanGurukulDBContext : DbContext
    {
        public ManthanGurukulDBContext(DbContextOptions<ManthanGurukulDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => new { u.MobileNo, u.Email }).IsUnique();
            var id = Guid.NewGuid();
            var password = "password12";
            Shared.Helpers.PasswordHasher.CreatePassword(password, out byte[] passwordHash, out byte[] passwordSalt);
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = id,
                FirstName = "Nishant",
                LastName = "Mishra",
                MobileNo = 9945654327,
                Email = "nishant.kumar.mishra@gmail.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.Ticks,
                CreatedBy = id,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
