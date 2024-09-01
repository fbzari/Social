using Microsoft.EntityFrameworkCore;
using Social.APi.Models;

namespace Social.APi.Data
{
    public class SocialApiContext : DbContext
    {
        public SocialApiContext(DbContextOptions<SocialApiContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for user roles

            modelBuilder.Entity<User>()
                .HasMany(u => u.SentFriendRequests)
                .WithOne(fr => fr.Sender)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete on SenderId

            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedFriendRequests)
                .WithOne(fr => fr.Receiver)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for received requests

            modelBuilder.Entity<User>()
                .HasMany(u => u.Friendships)
                .WithOne()
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of friendships

            // Role configuration
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for user roles

            // UserRole configuration
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for user roles

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for user roles

            // FriendRequest configuration
            modelBuilder.Entity<FriendRequest>()
                .HasKey(fr => fr.Id);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to avoid multiple cascade paths

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for received friend requests

            modelBuilder.Entity<FriendRequest>()
                .HasIndex(fr => new { fr.SenderId, fr.ReceiverId })
                .IsUnique(); // Prevents duplicate friend requests

            // Friendship configuration
            modelBuilder.Entity<Friendship>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User1)
                .WithMany()
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of friendships

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User2)
                .WithMany()
                .HasForeignKey(f => f.UserId2)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of friendships

            modelBuilder.Entity<Friendship>()
                .HasIndex(f => new { f.UserId1, f.UserId2 })
                .IsUnique(); // Ensures unique friendships
        }
    }
}
