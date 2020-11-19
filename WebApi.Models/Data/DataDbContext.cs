using Microsoft.EntityFrameworkCore;
using WebApi.Models.Models.Identity;

namespace WebApi.Models
{
    public class DataDbContext : DbContext
    {
        public DataDbContext() { }

        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }


        public DbSet<AccountModel> AccountModels { get; set; }
        public DbSet<MenuModel> MenuModels { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<IdentityUser> IdentityUsers { get; set; }
        public DbSet<LoginAccount> LoginAccounts { get; set; }
        public DbSet<IdentityGroupUser> IdentityGroupUsers { get; set; }
        public DbSet<IdentityPermission> IdentityPermissions { get; set; }
        public DbSet<IdentityRolePermission> IdentityRolePermissions { get; set; }
        public DbSet<IdentityUserGroup> IdentityUserGroups { get; set; }
        public DbSet<IdentityUserGroupRole> IdentityUserGroupRoles { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(@"Server=localhost;database=systemplus;uid=root;pwd=root");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuModel>();
            modelBuilder.Entity<IdentityRole>();
            modelBuilder.Entity<IdentityUser>();
            modelBuilder.Entity<LoginAccount>();
            modelBuilder.Entity<IdentityGroupUser>();
            modelBuilder.Entity<IdentityPermission>();
            modelBuilder.Entity<IdentityRolePermission>();
            modelBuilder.Entity<IdentityUserGroup>();
            modelBuilder.Entity<IdentityUserGroupRole>();
            modelBuilder.Entity<IdentityUserRole>();
        }

    }
}
