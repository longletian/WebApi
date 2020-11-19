using Microsoft.EntityFrameworkCore;
using WebApi.Models.Models.Identity;

namespace WebApi.Models
{
    public class DataDbContext : DbContext 
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        { 
        
        }


        public DbSet<AccountModel> accounts { get; set; }
        public DbSet<MenuModel> menuModels { get; set; }
        public DbSet<IdentityRole> identityRoles { get; set; }
        public DbSet<IdentityUser> identityUsers { get; set; }
        public DbSet<LoginAccount> loginAccounts { get; set; }
        public DbSet<IdentityGroupUser> identityGroupUsers { get; set; }
        public DbSet<IdentityPermission> identityPermissions { get; set; }
        public DbSet<IdentityRolePermission> identityRolePermissions { get; set; }
        public DbSet<IdentityUserGroup> identityUserGroups { get; set; }
        public DbSet<IdentityUserGroupRole> identityUserGroupRoles { get; set; }
        public DbSet<IdentityUserRole> identityUserRoles { get; set; }

        /// <summary>
        /// 重写实体模型
        /// </summary>
        /// <param name="modelBuilder"></param>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<IdentityRole>()
            //    .ToTable("");
        }

    }
}
